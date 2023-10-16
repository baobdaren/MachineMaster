using DG.Tweening;
using DG.Tweening.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGameAnimation : MonoBehaviour
{
	// ----------------//
	// --- 序列化
	// ----------------//
	[SerializeField]
	private NiceButton EnterButton;
	

	// ----------------//
	// --- 公有成员
	// ----------------//


	// ----------------//
	// --- 私有成员
	// ----------------//
	private AsyncOperation loadSceneOp;
	// ----------------//
	// --- Unity消息
	// ----------------//
	private void Awake()
	{
		EnterButton.OnLeftClick.AddListener(OnClicked_LoadScene);
	}

	private void Start()
	{
		//GameConsole.Instance.StartUse();
		//DOGetter<float> CameraSizeGetter = new DOGetter<float>(() => { return Camera.; });
		//DOSetter<float> CamearSizeSetter = new DOSetter<float>((float setValue) => { Camera.main.orthographicSize = setValue; });
		//DOTween.To(CameraSizeGetter, CamearSizeSetter, 25, 3);
		//CameraActor.Instance.MainCamera.transform.position = new Vector3(0, 0, -1);
		//CameraActor.Instance.MainCamera.transform.DOMoveZ(-46, 2f);
	}

	private void OnClicked_LoadScene()
	{
		EnterButton.interactable = false;
		Invoke("DelayLoadScene", 2f);

	}
	// ----------------//
	// --- 公有方法
	// ----------------//


	// ----------------//
	// --- 私有方法
	// ----------------//
	private void DelayLoadScene()
	{
		if (loadSceneOp == null)
		{
			loadSceneOp = SceneManager.LoadSceneAsync("GameStart");
		}
		else
		{
			//float cur = progressBg.GetComponent<RectTransform>().rect.width * loadSceneOp.progress;
			//progressFill.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, cur);
			//progressFill.fillAmount = cur;
		}
	}

	// ----------------//
	// --- 类型
	// ----------------//

}
