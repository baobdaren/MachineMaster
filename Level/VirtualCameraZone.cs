using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Cinemachine;

[RequireComponent(typeof(PlayerTrigger))]
public class VirtualCameraZone : SerializedMonoBehaviour
{
	// ----------------//
	// --- 序列化
	// ----------------//
	// ----------------//
	// --- 公有成员
	// ----------------//
	public CinemachineVirtualCamera VirtualCamera { get => _virtualCamera; }

	// ----------------//
	// --- 私有成员
	// ----------------//
	private CinemachineVirtualCamera _virtualCamera;
	private CinemachineConfiner2D _virtualCameraConfiner;
	// ----------------//
	// --- Unity消息
	// ----------------//
	private void Awake()
	{
		_virtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
		_virtualCameraConfiner = GetComponentInChildren<CinemachineConfiner2D>();
		VirtualCamera.enabled = false;
	}

	private void Start()
	{
		GetComponentInChildren<PlayerTrigger>().AddListener(OnPlayerEnter, OnPlayerExit);
	}

	private void OnPlayerEnter(GameObject target)
	{
		//Debug.Log("进入 对象" + collision.gameObject.name);
		//if (Trigged && TriggerType == TriggerTypes.Once) return;
		//if (collision.gameObject != TriggerObject) return;

		VirtualCamera.enabled = true;
		CameraActor.Instance.CurrentVirtualCameraZone = this;
		VirtualCamera.Follow = target.transform;
		if (_virtualCameraConfiner)
			_virtualCameraConfiner.m_BoundingShape2D = CameraActor.Instance.mainVirtualCameraConfiner.m_BoundingShape2D;
		Debug.Log("进入 虚拟相机范围");
	}

	private void OnPlayerExit(GameObject target)
	{
		//Debug.Log("退出 对象" + collision.gameObject.name);
		//if (Trigged && TriggerType == TriggerTypes.Once) return;
		//if (collision.gameObject != TriggerObject)
		//{
		//	return;
		//}
		VirtualCamera.Follow = null;
		VirtualCamera.enabled = false;
		if (CameraActor.Instance.CurrentVirtualCamera == VirtualCamera)
		{
			CameraActor.Instance.CurrentVirtualCameraZone = null;
		}
		Debug.Log("退出 虚拟相机范围");
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
