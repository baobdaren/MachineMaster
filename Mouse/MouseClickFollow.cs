using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseClickFollow : MonoBehaviour
{
	// ----------------//
	// --- 序列化
	// ----------------//

	// ----------------//
	// --- 公有成员
	// ----------------//
	public event Action<Vector3> OnFollow;

	// ----------------//
	// --- 私有成员
	// ----------------//
	private Transform FollowTarget;

	// ----------------//
	// --- Unity消息
	// ----------------//

	// ----------------//
	// --- 公有方法
	// ----------------//
	public void StartFollow(Transform followTarget = null)
	{
		FollowTarget = followTarget;
		StartCoroutine(CorFollow());
	}

	// ----------------//
	// --- 私有方法
	// ----------------//
	private IEnumerator CorFollow()
	{
		do
		{
			transform.position = FollowTarget == null ? (CameraActor.Instance.MouseWorldPos) : (FollowTarget.position);
			OnFollow?.Invoke(transform.position);
			yield return 0;
		} while (!Mouse.current.leftButton.isPressed || !Keyboard.current.escapeKey.isPressed);
		FollowTarget = null;
	}


	// ----------------//
	// --- 类型
	// ----------------//
}
