using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskCargo : MonoBehaviour
{
	// ------------------ //
	// --- 序列化
	// ------------------ //


	// ------------------ //
	// --- 公有成员
	// ------------------ //


	// ------------------ //
	// --- 私有成员
	// ------------------ //


	// ------------------ //
	// --- Unity消息
	// ------------------ //
	private void Awake()
	{
		name = GameConfig.Instance.StandardTransportLevelCargoName;
		gameObject.layer = GameLayerManager.DefalutEnv;
	}

	private void FixedUpdate()
	{

	}

	// ------------------ //
	// --- 公有方法
	// ------------------ //


	// ------------------ //
	// --- 私有方法
	// ------------------ //
}
