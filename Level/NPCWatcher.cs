using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine.InputSystem;

public class NPCWatcher : SerializedMonoBehaviour
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
	[SerializeField]
	private List<string> DialogContent = new List<string>();

	// ----------------//
	// --- Unity消息
	// ----------------//\
	private void Start()
	{
		GetComponentInChildren<AbsTargetTrigger>().OnPlayerEnter.AddListener(OnPlayerEnter);
	}

	private void OnEnable()
	{
		GetComponentInChildren<TMP_Text>().text = "1";
	}


	private void Reset()
	{
		OnEnable();
	}
	// ----------------//
	// --- 公有方法
	// ----------------//

	// ----------------//
	// --- 私有方法
	// ----------------//
	public void OnPlayerEnter(GameObject target)
	{
		StartCoroutine(Cor_Dialog());
	}

	private IEnumerator Cor_Dialog()
	{
		foreach (var item in DialogContent)
		{
			GetComponentInChildren<TMP_Text>().text = item;
			yield return null;
			while (!Mouse.current.leftButton.wasPressedThisFrame)
			{
				yield return null;
			}
		}
		GetComponentInChildren<TMP_Text>().text = "0";
	}

	// ----------------//
	// --- 类型
	// ----------------//
}
