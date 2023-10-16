using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;
using TMPro;
using UnityEngine.InputSystem;
using DG.Tweening.Plugins.Options;
using DG.Tweening.Core;

public class GameMessage : SerializedMonoBehaviour
{
	// ----------------//
	// --- 序列化
	// ----------------//

	public GameObject TextWorldCanvas;
	public GameObject ImageWorldCanvas;

	// ----------------//
	// --- 公有成员
	// ----------------//
	public static GameMessage Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = GameObject.FindObjectOfType<GameMessage>();
			}
			return _instance;
		}
	}

	private static GameMessage _instance;

	// ----------------//
	// --- 私有成员
	// ----------------//
	private List<(string, Vector2)> messageToPrintAtCenter = new List<(string, Vector2)>();

	// ----------------//
	// --- Unity消息
	// ----------------//
	private void Awake()
	{
		TextWorldCanvas.gameObject.SetActive(false);
		ImageWorldCanvas.gameObject.SetActive(false);
	}

	private void Start()
	{
		StartCoroutine(PrintMessage());
	}
	// ----------------//
	// --- 公有方法
	// ----------------//
	public void ShowErrorArea(Vector3 worldPos)
	{
		for (int i = 0; i < 3; i++)
		{
			GameObject errorAreaClone = GameObject.Instantiate(ImageWorldCanvas, ImageWorldCanvas.transform.parent);
			errorAreaClone.SetActive(true);
			errorAreaClone.transform.position = worldPos;
			errorAreaClone.transform.DOMove(worldPos, 0.4f+i*0.1f).onComplete += () => 
			{
				var op = errorAreaClone.transform.DOScale(Vector3.zero, 0.5f); 
				op.onComplete += () => { GameObject.Destroy(errorAreaClone); };
			};
		}
	}

	public void PrintMessageAtPos(string msg, Vector2 pos)
	{
		messageToPrintAtCenter.Add((msg, pos));
	}

	public void PrintMessageAtScreenCenter(string msg, bool redOrGreen = true)
	{
		Vector2 displayPos = CameraActor.ScreenTo2DWorldPosition(new Vector2(Screen.width / 2, Screen.height / 2));
		messageToPrintAtCenter.Add(($"<color={(redOrGreen ? "#ff0000" : "#00ff00")}>{msg}</color>", displayPos));
	}

	public void PrintMessageAtMousePos(string msg)
	{
		messageToPrintAtCenter.Add((msg, CameraActor.Instance.MouseWorldPos));
	}
	// ----------------//
	// --- 私有方法
	// ----------------//
	private Vector3 GetBestScale
	{
		get => (Vector3.one * CameraActor.Instance.MainCamera.orthographicSize);
	}

	private IEnumerator PrintMessage()
	{
		while (true)
		{
			if (messageToPrintAtCenter.Count > 0)
			{
				Print(messageToPrintAtCenter[0].Item1, messageToPrintAtCenter[0].Item2);
//#if UNITY_EDITOR
//				//Debug.Log(messageToPrintAtCenter[0].Item1 + "/n" + StackTraceUtility.ExtractStackTrace());
//#endif
				messageToPrintAtCenter.RemoveAt(0);
			}
			//yield return new WaitForSeconds(0.1f);
			yield return new WaitForSeconds(0.085f);
		}
	}

	private void Print(string msg, Vector2 pos)
	{
		GameObject errorTipsClone = GameObject.Instantiate(TextWorldCanvas, TextWorldCanvas.transform.parent);
		errorTipsClone.GetComponentInChildren<TextMeshProUGUI>().text = msg;
		errorTipsClone.GetComponentInChildren<TextMeshProUGUI>().fontSize = 0.2f * CameraActor.Instance.CameraViewSize / CameraActor.Instance.MaxCameraViewSize;
		errorTipsClone.SetActive(true);
		TweenerCore<Vector3, Vector3, VectorOptions> op;
		errorTipsClone.transform.position = pos;
		op = errorTipsClone.transform.DOMoveY(pos.y + 1, 1);
		op.onComplete += () =>
		{
			GameObject.Destroy(errorTipsClone);
		};
	}
	// ----------------//
	// --- 类型
	// ----------------//
}
