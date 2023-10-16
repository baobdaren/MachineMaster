using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameSettingUI : MonoBehaviour
{
	// ------------------ //    
	// --- 序列化    
	// ------------------ //
	[SerializeField]
	GameObject Window;
	[SerializeField]
	Button ButtonOpenWindow;
	[SerializeField]
	Button ButtonExitWindow;
    [SerializeField]
    Slider VolumeSlider;
    [SerializeField]
    Slider SliderScrollerSpeed;
	[SerializeField]
	Slider SliderFPS;
	[SerializeField]
	TextMeshProUGUI TextFPSValue;
	[SerializeField]
	Button ButtonExitGame;
	[SerializeField]
	Button ButtonBackToStartScene;
	[SerializeField]
	Toggle ToggleVSync;

	// ------------------ //    
	// --- 公有成员    
	// ------------------ //

	// ------------------ //   
	// --- 私有成员    
	// ------------------ //

	// ------------------ //    
	// --- Unity消息    
	// ------------------ //
	protected void Awake()
	{
		Window.SetActive(false);

		float tmpValue = GameManager.Instance.Volume;
		VolumeSlider.maxValue = 1; 
		VolumeSlider.minValue = 0;
		VolumeSlider.value = tmpValue;

		tmpValue = GameManager.Instance.MouseScrollSpeed;
		SliderScrollerSpeed.minValue = 0.001f;
		SliderScrollerSpeed.maxValue = 0.03f;
		SliderScrollerSpeed.value = tmpValue;

		SliderFPS.onValueChanged.AddListener(On_TargetPFSChanged);

		VolumeSlider.onValueChanged.AddListener(On_VolumeSlider);
		SliderScrollerSpeed.onValueChanged.AddListener(On_ScrollSpeed);
		ButtonExitWindow.onClick.AddListener(()=>SetDisplayWindow(false));
		ButtonOpenWindow.onClick.AddListener(()=>SetDisplayWindow(true));
		ButtonBackToStartScene.onClick.AddListener(OnClicked_BackToStartScene);
		ButtonExitGame.onClick.AddListener(OnClicked_ExitApplication);

		ToggleVSync.onValueChanged.AddListener(On_VSyncOpen);
	}

	private void Update()
	{
		if (Keyboard.current.escapeKey.wasPressedThisFrame)
		{
			SetDisplayWindow(!Window.activeSelf);
		}
	}

	// ------------------ //    
	// --- 公有方法   
	// ------------------ //    
	public void SetDisplayWindow(bool display)
	{
		Window.SetActive(display);
		TimeManager.Instance.IsPausing = Window.activeSelf;
	}

	// ------------------ //   
	// --- 私有方法
	// ------------------ //

	private void On_VolumeSlider(float value)
    {
		GameManager.Instance.Volume = value;
	}

	private void On_ScrollSpeed(float value)
	{
		GameManager.Instance.MouseScrollSpeed = value;
	}

	private void On_VSyncOpen(bool open)
	{
		QualitySettings.vSyncCount = open ? 1 : 0;
	}

	//private void On_VerticalHoldToggleValueChanged(bool ison)
	//{
	//	Debug.Log("VScount" + QualitySettings.vSyncCount);
	//	GamePersistentData.Instance.VerticalHoldData = ison;
	//	Application.targetFrameRate = 120;
	//	QualitySettings.vSyncCount = (GamePersistentData.Instance.VerticalHoldData) ? 1 : 0;
	//}

	private void On_TargetPFSChanged(float fps)
	{
		TextFPSValue.color = new Color(TextFPSValue.color.r, TextFPSValue.color.g, TextFPSValue.color.b, 1);
		Application.targetFrameRate = (int)(fps == SliderFPS.maxValue ? -1 : fps) * 30;
		TextFPSValue.text = Application.targetFrameRate < 0 ? "--" : Application.targetFrameRate.ToString();
		TextFPSValue.DOColor(new Color(TextFPSValue.color.r, TextFPSValue.color.g, TextFPSValue.color.b, 0), 3f);
	}

	//private void OnClicked_BacktoGameStartScene()
	//{
	//	GameManager.Instance.EnterLevelSelectScene(null, null, false);
	//}

	private void OnClicked_BackToStartScene()
	{
		if (GameManager.Instance.IsPlayingLevel)
		{
			TimeManager.Instance.IsPausing = false;
			GameManager.Instance.On_LevelFinish(true, false);
		}
	}

	private void OnClicked_ExitApplication()
	{
		TimeManager.Instance.IsPausing = false;
#if UNITY_EDITOR
		if (Application.isEditor)
		{
			UnityEditor.EditorApplication.isPlaying = false;
		}
#else
		Application.Quit();
#endif
	}
}
