using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameStarter : MonoBehaviour
{
	// ----------------//
	// --- 序列化
	// ----------------//
	[SerializeField]
	private TextMeshProUGUI _tipsText;
	[SerializeField]
	Image _bgImage;
	[SerializeField]
	CanvasGroup _buttonsCanvasGroup;
	[SerializeField]
	Button _buttonStartGame;
	[SerializeField]
	Button _buttonExitGame;
	[SerializeField]
	Button _buttonResolution;
	[SerializeField]
	Slider _loadProgressSlider;
	[SerializeField]
	TextMeshProUGUI _progressText;
	[SerializeField]
	TextMeshProUGUI _debugText;
	[SerializeField]
	TMP_Dropdown _dropDownResolutionList;

	[SerializeField]
	private CanvasGroup _centerGearCanvasGroup;
	// ----------------//
	// --- 公有成员
	// ----------------//

	// ----------------//
	// --- 私有成员
	// ----------------//
	private bool displayingButtons = false; // 是否显示过了
	private bool canDisplayButtons = false;

	private Coroutine TipsCor;
	// ----------------//
	// --- Unity消息
	// ----------------//
	private void Awake()
	{
		StartCoroutine(ProgressPlayEnterMovie());
		canDisplayButtons = false;
		// 创建相机和事件系统
		_loadProgressSlider.gameObject.SetActive(false);
		Screen.sleepTimeout = 0;
		_progressText.gameObject.SetActive(false);
		_bgImage?.gameObject.SetActive(true);
		Application.targetFrameRate = 60;

		_buttonResolution.onClick.AddListener(() => {
			_dropDownResolutionList.gameObject.SetActive(!_dropDownResolutionList.gameObject.activeSelf);
		});

		_buttonsCanvasGroup.alpha = 0;
		_buttonsCanvasGroup.gameObject.SetActive(false);
		// 分辨率列表
		_dropDownResolutionList.options = new List<TMP_Dropdown.OptionData>();
		var optionDatas = new List<TMP_Dropdown.OptionData>();
		for (int i = Screen.resolutions.Length - 1; i >= 0; i--)
		{
			Resolution item = Screen.resolutions[i];
			_dropDownResolutionList.options.Add(new TMP_Dropdown.OptionData($"{item.width}X{item.height}"));
		}
		_dropDownResolutionList.onValueChanged.AddListener(On_SetResoulution);
		_dropDownResolutionList.gameObject.SetActive(false);
		_tipsText.gameObject.SetActive(true);
		
	}

	private void OnEnable()
	{
		TipsCor = StartCoroutine(ProgressGradualTipsColor());
	}

	private void Start()
	{
		_buttonStartGame.onClick.AddListener(On_ClickStartGame);
		_loadProgressSlider.handleRect.gameObject.GetComponent<MaskableGraphic>().raycastTarget = false;
		_buttonExitGame.onClick.AddListener(() =>
		{
			Application.Quit();
		});
		int selectCurrentResolution = _dropDownResolutionList.options.FindIndex(option => option.text == $"{Screen.width}X{Screen.height}");
		_dropDownResolutionList.SetValueWithoutNotify(selectCurrentResolution);
	}

	private void Update()
	{
		if (Mouse.current.leftButton.wasPressedThisFrame && !displayingButtons && canDisplayButtons)
		{
			_buttonsCanvasGroup.gameObject.SetActive(true);
			StopCoroutine(TipsCor);
			displayingButtons = true;
			// 渐变显示按钮组
			DOTween.To(() => _buttonsCanvasGroup.alpha, x => _buttonsCanvasGroup.alpha = x, 1f, 2f);
			// 
			_tipsText.DOColor(new Color(0, 0, 0, 0), 0.8f)/*.onComplete += () => { _tipsText.gameObject.SetActive(false); }*/;
		}
	}

	//private void Update()
	//{
	//	//_debugText.SetText(Time.time.ToString());
	//	if (Input.GetKeyDown(KeyCode.F))
	//	{
	//		Debug.LogError("Resources Load Game");
	//		Resources.Load("Game");
	//		//GameManager.Instance.CreateFirstGameObject();
	//		GetComponent<Canvas>().worldCamera = CameraActor.Instance.MainCamera;
	//	}
	//}

	// ----------------//
	// --- 公有方法
	// ----------------//

	// ----------------//
	// --- 私有方法
	// ----------------//
	private void On_SetResoulution(int selectedIndex)
	{
		TMP_Dropdown.OptionData selectedOption = _dropDownResolutionList.options[selectedIndex];
		string[] msgs = selectedOption.text.Split('X');
		Debug.Log("分辨率选择" + selectedOption.text);
		Screen.SetResolution(int.Parse(msgs[0]), int.Parse(msgs[1]), true);
		Debug.Log("当前分辨率" + Screen.currentResolution);
	}

	private void OnClick_HideGearIcon()
	{
		//// 渐变隐藏图标
		//DOTweenModuleUI.DOColor(_gearIconButton.GetComponentInChildren<TextMeshProUGUI>(), new Color(0, 0, 0, 0), 2).onComplete += 
		//	() => { _gearIconButton.gameObject.SetActive(false); };
		//// 渐变显示按钮
		//Color startButtonColor = _buttonStartGame.GetComponentInChildren<TextMeshProUGUI>().color;
		//Color exitButtonColor = _buttonExitGame.GetComponentInChildren<TextMeshProUGUI>().color;
		//_buttonStartGame.gameObject.SetActive(true);
		//_buttonExitGame.gameObject.SetActive(true);
		//DOTweenModuleUI.DOColor(_buttonStartGame.GetComponentInChildren<TextMeshProUGUI>(), startButtonColor, 3000);
		//DOTweenModuleUI.DOColor(_buttonExitGame.GetComponentInChildren<TextMeshProUGUI>(), exitButtonColor, 3000);
	}

	private void On_ClickStartGame()
	{
		_buttonStartGame.interactable = false;
		GameManager.Instance.EnterLevelSelectScene();
	}

	private IEnumerator ProgressLoadScene(AsyncOperation op)
	{
		_loadProgressSlider.gameObject.SetActive(true);
		_progressText.gameObject.SetActive(true);
		_loadProgressSlider.value = 0;
		op.allowSceneActivation = false;
		float displayValue = 0;
		while (displayValue < 1)
		{
			SetBitProgress(displayValue);
			yield return new WaitForEndOfFrame();
			displayValue += 0.01f;
		}
		yield return new WaitForSeconds(0.5f);
		SetBitProgress(1);
		yield return new WaitForEndOfFrame();
		op.allowSceneActivation = true;
		_loadProgressSlider.gameObject.SetActive(false);
		_progressText.gameObject.SetActive(false);
	}
	private void SetBitProgress(float value)
	{
		const int max = 255;
		const float pauseValue = 0.9f;
		string maxStr = "/" + Convert.ToString(max, 2);
		if (value == 1)
		{
			_loadProgressSlider.value = 1;
		}
		else
		{
			_loadProgressSlider.value = Mathf.Clamp(value / pauseValue, 0, 0.999999f);
		}
		//_progressText.text = _loadProgressSlider.value.ToString();
		_progressText.text = Convert.ToString((int)(_loadProgressSlider.value * max), 2) + maxStr;
	}

	private IEnumerator ProgressGradualTipsColor()
	{
		while (_tipsText.gameObject.activeSelf)
		{
			float alphaValue = (MathF.Cos(Time.realtimeSinceStartup * 3) + 1) / 2 + 0.2f;
			_tipsText.color = new Color(_tipsText.color.r, _tipsText.color.g, _tipsText.color.b, alphaValue);
			yield return null;
		}
	}

	private IEnumerator ProgressPlayEnterMovie()
	{
		_centerGearCanvasGroup.GetComponent<CanvasGroup>().alpha = 0;
		GetComponent<CanvasGroup>().alpha = 0;
		yield return new WaitForSeconds(1);
		yield return new WaitUntil(() => 1 / Time.deltaTime > Application.targetFrameRate - 5);
		float startTime = Time.time;
		// 渐变显示 齿轮logo
		yield return new WaitUntil(() => (_centerGearCanvasGroup.GetComponent<CanvasGroup>().alpha = (Time.time - startTime) / 2) > 1);
		// 完全显示后,开始加载资源
		startTime = Time.time;
		yield return AssetsManager.InitAssets();

		// 至少显示2秒
		yield return new WaitForSeconds(Mathf.Clamp(Time.time - startTime, 2, float.MaxValue));
		//startTime = Mathf.Clamp(startTime, startTime + 2, float.MaxValue);
		// 渐变隐藏 齿轮logo
		startTime = Time.time + 1.8f;
		yield return new WaitUntil(() => (_centerGearCanvasGroup.GetComponent<CanvasGroup>().alpha = (startTime - Time.time) / 1.5f) < 0);
		yield return new WaitForSeconds(1);
		startTime = Time.time;
		// 渐变显示 主要界面
		yield return new WaitUntil(() => (GetComponent<CanvasGroup>().alpha = (Time.time - startTime) / 0.8f) > 1);
		canDisplayButtons = true;
	}
}
