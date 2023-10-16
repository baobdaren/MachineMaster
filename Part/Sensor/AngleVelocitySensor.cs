using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngleVelocitySensor : BaseSensor
{
	// ----------------//
	// --- 序列化
	// ----------------//


	// ----------------//
	// --- 公有成员
	// ----------------//
	public Vector2 Velocity
	{
		get
		{
			return _rigidbody.velocity;
		}
	}
	public float Angle
	{
		get
		{
			return transform.eulerAngles.z;
		}
	}



	// ----------------//
	// --- 私有成员
	// ----------------//
	private Transform _childCenter;
	private Rigidbody2D _rigidbody;


	// ----------------//
	// --- Unity消息
	// ----------------//
	protected void Start()
	{
		_rigidbody = GetComponent<Rigidbody2D>();
		_childCenter = transform.GetChild(0);
		//ConnectableColliders = new List<Collider2D>(GetComponents<Collider2D>());
	}
	private void FixedUpdate ( )
	{
		_childCenter.eulerAngles = Vector3.zero;
	}


	// ----------------//
	// --- 公有方法
	// ----------------//



	// ----------------//
	// --- 私有方法
	// ----------------//

}
