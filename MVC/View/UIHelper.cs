using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class UIHelper
{
	public static bool MouseInScreen
	{
		get
		{ 
			Vector2 pos = Mouse.current.position.ReadValue();
			return pos.x >= 0 && pos.x <= Screen.width && pos.y >= 0 && pos.y <= Screen.height;
		}
	}


	public static bool ClickedUI(Vector2 clickScreenPos)
	{
		if (Time.frameCount != _resultFrame)
		{
			_clickedUIResult = ClickedUI(clickScreenPos, out string _);
			_resultFrame = Time.frameCount;
		}
		return _clickedUIResult;
	}

	public static bool ClickedUI(Vector2 clickScreenPos, out string castName)
	{
		if (EventSystem.current == null)
		{
			Debug.LogError("EventSystem 为空");
			castName = string.Empty;
			return false;
			//CameraActor.Instance.MainCamera.gameObject.AddComponent<EventSystem>();
		}
		PointerEventData pointerEventData = new PointerEventData(EventSystem.current)
		{
			position = clickScreenPos
		};
		List<RaycastResult> result = new List<RaycastResult>();
		EventSystem.current.RaycastAll(pointerEventData, result);
		for (int i = 0; i < result.Count; i++)
		{
			if (result[i].gameObject.GetComponent<RectTransform>())
			{
				castName = result[0].gameObject.name;
				return true;
			}
		}
		castName = string.Empty;
		return false;
	}

	public static bool ClickedUI()
	{
		return ClickedUI(Input.mousePosition);
	}

	private static int _resultFrame = -1;
	private static bool _clickedUIResult;
}
