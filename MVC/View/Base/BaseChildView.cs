using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public abstract class BaseChildView : SerializedMonoBehaviour
{
	// ----------------//
	// --- 序列化
	// ----------------//

	// ----------------//
	// --- 公有成员
	// ----------------//
	[Header("子视图所使用的RectTransform")]
	[GUIColor("FiledColor")]
	public GameObject MyView;

	// ----------------//
	// --- 私有成员
	// ----------------//
	private Color FiledColor { get => MyView == null ? Color.red : Color.green; }

	// ----------------//
	// --- Unity消息
	// ----------------//

	// ----------------//
	// --- 公有方法
	// ----------------//
	public virtual void OnParentViewEnter()
	{
		MyView.SetActive(true);
	}

	public virtual void OnHideParentView()
	{ 
		MyView.gameObject.SetActive(false);
	}

	public virtual void OnParentViewExit()
	{ 
		MyView.gameObject.SetActive(false);
	}

	// ----------------//
	// --- 私有方法
	// ----------------//
	[ExecuteInEditMode]
	private void LogNull()
	{
		if (MyView == null)
		{
			Debug.LogError(this + " 没有设置主视图");
		}
	}

	// ----------------//
	// --- 类型
	// ----------------//
}
