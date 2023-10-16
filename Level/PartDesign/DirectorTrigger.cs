using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Cinemachine;

public class DirectorTrigger : SerializedMonoBehaviour
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
	private CinemachineVirtualCamera virtualCamera;

	// ----------------//
	// --- Unity消息
	// ----------------//
	private void Awake()
	{
		CameraActor.Instance.MainCameraDirector.SetGenericBinding(gameObject, gameObject);
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
