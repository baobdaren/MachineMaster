using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 关卡选择界面
/// </summary>
public class LevelsSelect : MonoBehaviour
{
	// ----------------//
	// --- 序列化
	// ----------------//
	[SerializeField]
	GameObject _chapterSelectView;
	[SerializeField]
	GameObject _levelsSelectView;
	[SerializeField]
	Button _exitButton;

	[SerializeField]
	GameObject _originChapterObject;
	[SerializeField]
	GameObject _originLevelObject;
	[SerializeField]
	RawImage _selectedChapterImage;

	// ----------------//
	// --- 公有成员
	// ----------------//
	//public static string SelectedChapterName { private set; get; }
	public static Dictionary<string, Texture2D> ChaptersLabelToTexture => AssetsManager.ChapterLabelToCoverTexture;

	// ----------------//
	// --- 私有成员
	// ----------------//
	private Dictionary<GameObject, string[]> _createdLevelsItemToArchiveID = new Dictionary<GameObject, string[]>();
	//private Dictionary<string, Texture2D> _chapterLabelToTexture;
	// ----------------//
	// --- Unity消息
	// ----------------//
	protected void Awake()
	{
		if (FindObjectOfType<ES3ReferenceMgr>() == null)
		{
			DontDestroyOnLoad(GameObject.Instantiate(GameConfig.Instance.ES3Mgr));
		}
		Application.quitting += () =>
		{
			AssetBundle.UnloadAllAssetBundles(true);
		};
		_exitButton.onClick.AddListener(OnClicked_Back);
		_originChapterObject.gameObject.SetActive(false);
		_originLevelObject.gameObject.SetActive(false);
	}

	private void Start()
	{
		StartCoroutine(ProgressCreateChapterSelectMenum());
	}

	private void OnEnable()
	{
		if (!string.IsNullOrEmpty(GameManager.Instance.SelectedChapterName))
		{
			UpdateLevelArchiveIconStat();
		}
	}


	// ----------------//
	// --- 公有方法
	// ----------------//


	// ----------------//
	// --- 私有方法
	// ----------------//
	private IEnumerator ProgressCreateChapterSelectMenum()
	{
		SetDisplay(true);
		//if (ChaptersLabelToTexture == null)
		//{
		//	var loadChapterTextureHandler = Addressables.LoadResourceLocationsAsync("ChapterTextures");
		//	yield return loadChapterTextureHandler;
		//	ChaptersLabelToTexture = new Dictionary<string, Texture2D>(loadChapterTextureHandler.Result.Count);
		//	foreach (var item in loadChapterTextureHandler.Result)
		//	{
		//		ChaptersLabelToTexture.Add(Path.GetFileNameWithoutExtension(item.PrimaryKey), (Texture2D)item.Data);
		//	}
		//}
		foreach (var item in ChaptersLabelToTexture)
		{
			GameObject clone = GameObject.Instantiate(_originChapterObject, _originChapterObject.transform.parent);
			clone.GetComponentInChildren<RawImage>().texture = item.Value;
			clone.transform.GetComponentInChildren<Button>().onClick.AddListener(() =>
			{
				OnClicked_SelectChapter(item.Key);
			});
			clone.SetActive(true);
			yield return null;
		}

		if (!string.IsNullOrEmpty(GameManager.Instance.SelectedChapterName))
		{
			OnClicked_SelectChapter(GameManager.Instance.SelectedChapterName);
		}
		if (!string.IsNullOrEmpty(GameManager.Instance.SelectedLevelName) && GameManager.Instance.AutoEnterReplay)
		{
			OnClicked_SelectLevel(GameManager.Instance.SelectedLevelName);
		}
	}

	/// <summary>
	/// 创建该章节下所有关卡的选择按钮
	/// </summary>
	/// <param name="chapterLabel"></param>
	/// <returns></returns>
	private IEnumerator ProgressCreateLevelsItemInChapter(string chapterLabel)
	{
		GameManager.Instance.SelectChapter(chapterLabel);
		Texture chapterTex = ChaptersLabelToTexture[chapterLabel];
		// 这只章节图片
		_selectedChapterImage.GetComponent<RawImage>().texture = chapterTex;
		float heightRatio = 1f * chapterTex.width / chapterTex.height;
		_selectedChapterImage.GetComponent<RectTransform>().localScale = new Vector3(heightRatio, 1);
		_selectedChapterImage.gameObject.SetActive(true);
		// 创建关卡列表
        var loadLevelHandler = Addressables.LoadResourceLocationsAsync(new List<string> { "Chapter", chapterLabel}, Addressables.MergeMode.Intersection);
		yield return loadLevelHandler;
		List<string> levelsName = new List<string>(loadLevelHandler.Result.Count);
		foreach (var item in loadLevelHandler.Result)
		{
			levelsName.Add(item.PrimaryKey);
		}
		CreateLevelSceneButton(GameManager.Instance.SelectedChapterName, levelsName);
		UpdateLevelArchiveIconStat();
		Addressables.Release(loadLevelHandler);
	}

	private void CreateLevelSceneButton(string chapterName, List<string> levelNamesWithExtension)
	{
		SetDisplay(false);
		while (_createdLevelsItemToArchiveID.Count < levelNamesWithExtension.Count)
		{
			GameObject levelItemClone = Instantiate(_originLevelObject, _originLevelObject.transform.parent);
			_createdLevelsItemToArchiveID.Add(levelItemClone, new string[2]);
		}
		int levelNameIndex = 0;
		foreach (KeyValuePair<GameObject, string[]> item in _createdLevelsItemToArchiveID)
		{
			item.Key.GetComponentInChildren<Button>().onClick.RemoveAllListeners();
			if (levelNameIndex < levelNamesWithExtension.Count)
			{
				string levelName = Path.GetFileNameWithoutExtension(levelNamesWithExtension[levelNameIndex]);
				item.Key.SetActive(true);
				item.Key.GetComponentInChildren<TextMeshProUGUI>().SetText(levelName);
				_createdLevelsItemToArchiveID[item.Key][0] = chapterName;
				_createdLevelsItemToArchiveID[item.Key][1] = levelName;
				item.Key.GetComponentInChildren<Button>().onClick.RemoveAllListeners();
				item.Key.GetComponentInChildren<Button>().onClick.AddListener(
				() =>
				{
					//Debug.LogError("点击触发" + GetInstanceID());
					OnClicked_SelectLevel(Path.GetFileNameWithoutExtension(levelName));
				});
				item.Key.transform.Find("IconOK/ButtonDeleteArchive").GetComponent<Button>().onClick.AddListener(
				() =>
				{
					OnClicked_DeleteArchive(levelName);
				});
				levelNameIndex++;
			}
			else
			{
				item.Key.SetActive(false);
			}
		}
	}

	/// <summary>
	/// 点击了某一个章节
	/// </summary>
	/// <param name="chapterBundlePath"></param>
	private void OnClicked_SelectChapter(string chapterBundlePath)
	{
		StartCoroutine(ProgressCreateLevelsItemInChapter(chapterBundlePath));
	}

	private void OnClicked_SelectLevel(string levelName)
	{
		if (GameManager.Instance.IsPlayingLevel)
		{
			Debug.LogError("因IsPlaying返回呢");
			return;
		}
		GameManager.Instance.EnterLevel(levelName);
	}

	private void OnClicked_Back()
	{
		if (_levelsSelectView.gameObject.activeSelf)
		{
			_selectedChapterImage.GetComponent<RawImage>().texture = null;
			SetDisplay(true);
		}
		else
		{
			SceneManager.LoadScene(0, LoadSceneMode.Single);
		}
		GameManager.Instance.SelectChapter(null);
	}

	private void OnClicked_DeleteArchive(string deleteLevelName)
	{
		Debug.Assert(ArchiveManager.HasArchive(GameManager.Instance.SelectedChapterName, deleteLevelName));
		ES3.DeleteFile(ArchiveManager.GetSavePath(GameManager.Instance.SelectedChapterName, deleteLevelName));
		//ES3.Save()
		UpdateLevelArchiveIconStat();
	}


	/// <summary>
	/// 设置存档土表状态
	/// </summary>
	private void UpdateLevelArchiveIconStat()
	{
		bool hasArchive;
		foreach (var item in _createdLevelsItemToArchiveID)
		{
			hasArchive = ArchiveManager.HasArchive(item.Value[0], item.Value[1]);
			item.Key.transform.Find("IconOK").gameObject.SetActive(hasArchive);
			item.Key.transform.Find("IconNO").gameObject.SetActive(!hasArchive);
		}
	}

	private void SetDisplay(bool displayChapterView)
	{
		_chapterSelectView.gameObject.SetActive(displayChapterView);
		_levelsSelectView.gameObject.SetActive(!displayChapterView);
	}
}
