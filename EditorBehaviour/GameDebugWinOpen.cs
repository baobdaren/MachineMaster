using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class GameDebugWinOpen1 : SerializedMonoBehaviour
{
	// ----------------//
	// --- 序列化
	// ----------------//
	[SerializeField]
	private GameObject OpenTarget;
	// ----------------//
	// --- 公有成员
	// ----------------//

	// ----------------//
	// --- 私有成员
	// ----------------//

	// ----------------//
	// --- Unity消息
	// ----------------//
	private void Awake()
	{
		GetComponent<Button>().onClick.AddListener(() => { OpenTarget.gameObject.SetActive(!OpenTarget.activeSelf); });
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
