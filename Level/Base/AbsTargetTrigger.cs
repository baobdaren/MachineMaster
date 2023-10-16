using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;
using System;

[RequireComponent(typeof(Collider2D))]
public abstract class AbsTargetTrigger : MonoBehaviour
{
	// ----------------//
	// --- 序列化
	// ----------------//
	[SerializeField]
	private bool _triggeOnce = true;

	protected abstract List<GameObject> GetTriggerTargets { get; }
	// ----------------//
	// --- 公有成员
	// ----------------//
	[HideInInspector]
	public readonly UnityEvent<GameObject> OnPlayerEnter = new UnityEvent<GameObject>();
	[HideInInspector]
	public readonly UnityEvent<GameObject> OnPlayerExit = new UnityEvent<GameObject>();

	public bool TargetStayIn { get; private set; } = false;
	// ----------------//
	// --- 私有成员
	// ----------------//
	[ReadOnly]
	bool Trigged = false;
	[ReadOnly]
	// ----------------//
	// --- Unity消息
	// ----------------//
	private void Start()
	{
		Debug.Assert(GetTriggerTargets != null, transform.name + "断言失败", gameObject);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		//Debug.Log("进入 对象" + collision.gameObject.name);
		if (Trigged && _triggeOnce) return;
		if (!GetTriggerTargets.Contains(collision.gameObject)) return;
		TargetStayIn = true;
		OnPlayerEnter?.Invoke(collision.gameObject);
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		//Debug.Log("退出 对象" + collision.gameObject.name);
		if (Trigged && _triggeOnce) return;
		if (!GetTriggerTargets.Contains(collision.gameObject)) return;
		OnPlayerExit?.Invoke(collision.gameObject);
		TargetStayIn = false;
		Trigged = true;
	}

	// ----------------//
	// --- 公有方法
	// ----------------//
	public void AddListener(UnityAction<GameObject> enterFunc, UnityAction<GameObject> exitFunc)
	{
		OnPlayerEnter.AddListener(enterFunc);
		OnPlayerExit.AddListener(exitFunc);
	}

	private void OnDestroy()
	{
		OnPlayerEnter.RemoveAllListeners();
		OnPlayerExit.RemoveAllListeners();
	}

	// ----------------//
	// --- 私有方法
	// ----------------//
	//[Button("设为玩家")]
	//private void SetAsPlayer()
	//{
	//	GetTriggerTarget = FindObjectOfType <VehiclePlayer>().gameObject;
	//}
	// ----------------//
	// --- 类型
	// ----------------//
}
