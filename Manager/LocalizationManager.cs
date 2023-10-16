using System.Collections.Generic;
using UnityEngine;

public class LocalizationManager
{
	// ------------- //
	// -- 序列化
	// ------------- //
	public static LocalizationManager Instance = new LocalizationManager();

	// ------------- //
	// -- 私有成员
	// ------------- //
	private Dictionary<string, string[]> translateDic
	{
		get
		{
			if (_translateDic == null)
				_translateDic = LoadLocalizationTextAsset();
			return _translateDic;
		}
	}
	public Dictionary<string, string[]> _translateDic;

	// ------------- //
	// -- 公有成员
	// ------------- //
	public LanguageTypes Language { get; private set; } = LanguageTypes.English;

	// ------------- //
	// -- 公有方法
	// ------------- //
	public void SetLanguage(LanguageTypes language)
	{ 
		Language = language;
		var findResults = GameObject.FindObjectsByType<NiceText>(FindObjectsInactive.Include, FindObjectsSortMode.None);
		foreach (var item in findResults)
		{
			item.ForceTranslateText();
		}
	}

	/// <summary>
	/// 翻译
	/// </summary>
	/// <param name="originalText">中文原文</param>
	/// <returns></returns>
	public string Translate(string originalText)
	{
		if (!Application.isPlaying) return originalText;
		originalText = originalText.Replace(" ", "");
		if (translateDic.TryGetValue(originalText, out string[] foundItem))
		{
			return foundItem[(int)Language]; // 按照枚举序号，中文既是键也是值数组第0条
		}
		Debug.LogError("无法翻译" + originalText);
		return originalText + " 无法翻译";
	}
	// ------------- //
	// -- 私有方法
	// ------------- //
	private LocalizationManager() {  }

	private Dictionary<string, string[]> LoadLocalizationTextAsset()
	{
		TextAsset localizationAsset = Resources.Load<TextAsset>("TextAssets/Localization");
		string[] dicList = localizationAsset.text.Split('\n');
		Dictionary<string, string[]> result = new Dictionary<string, string[]>(dicList.Length);
		foreach (var item in dicList)
		{
			string[] r = item.Split(',');
			result.Add(r[0], r);
		}
		return result;
	}

	// ------------- //
	// -- 类型
	// ------------- //
	public enum LanguageTypes
	{
		Chinese = 0,
		English
	}
}
