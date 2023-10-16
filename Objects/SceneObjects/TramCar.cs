using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class TramCar : SerializedMonoBehaviour
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
	private bool Running = false;
	[SerializeField]
	private Vector3 _hidePos;

	// ----------------//
	// --- Unity消息
	// ----------------//
	private void Awake()
	{
		GetComponent<Rigidbody2D>().simulated = false;
	}

	private void OnBecameVisible()
	{
		if (Running) return;
		Running = true;
		GetComponent<Rigidbody2D>().simulated = true;
		GetComponentInChildren<HingeJoint2D>().useMotor = true;
		StartCoroutine(DesCor());
	}

	private void OnDrawGizmos()
	{
		Debug.DrawLine(transform.position, _hidePos, Color.red);
	}

	// ----------------//
	// --- 公有方法
	// ----------------//

	// ----------------//
	// --- 私有方法
	// ----------------//
	private IEnumerator DesCor()
	{
		_hidePos.Set(_hidePos.x, _hidePos.y, 0);
		while (Vector2.SqrMagnitude(transform.position - _hidePos) > 1)
		{
			yield return null;
		}
		foreach (var item in GetComponentsInChildren<Rigidbody2D>())
		{
			item.simulated = false;
		}
	}

	// ----------------//
	// --- 类型
	// ----------------//
}
