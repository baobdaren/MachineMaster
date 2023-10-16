using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;

public class EditorBehaviour : SerializedMonoBehaviour
{
	// ----------------//
	// --- 序列化
	// ----------------//

	// ----------------//
	// --- 公有成员
	// ----------------//

	// ----------------//
	// --- 私有成员
	// ----------------//

	// ----------------//
	// --- Unity消息
	// ----------------//
	private void Update()
	{
		if (ModelPlayerPartEditor.Instance.EditingPlayerPartCtrl != null)
		{
		}
		else
		{
			GetComponentInChildren<TextMeshProUGUI>().text = "NULL";
		}
	}

	// ----------------//
	// --- 公有方法
	// ----------------//

	// ----------------//
	// --- 私有方法
	// ----------------//

	// ----------------//
	// --- 类型
	// ----------------//
}
