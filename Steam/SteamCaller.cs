using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Steamworks;
using UnityEngine;

public class SteamCaller: MonoBehaviour
{
	// ------------- //
	// -- 序列化
	// ------------- //

	// ------------- //
	// -- 私有成员
	// ------------- //

	// ------------- //
	// -- 公有成员
	// ------------- //
	public static SteamCaller Instance { get; private set; }

	// ------------- //
	// -- Unity 消息
	// ------------- //
	private void Awake()
	{
		Debug.Assert(Instance != null);
		Instance = this;
		GameObject.DontDestroyOnLoad(gameObject);
	}

	private void Update()
	{
		SteamClient.RunCallbacks();
	}
	// ------------- //
	// -- 公有方法
	// ------------- //

	// ------------- //
	// -- 私有方法
	// ------------- //

	// ------------- //
	// -- 类型
	// ------------- //
}

