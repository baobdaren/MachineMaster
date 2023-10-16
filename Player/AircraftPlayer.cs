using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.InputSystem;

public class AircraftPlayer:PlayerBase
{
	// ----------------//
	// --- 序列化
	// ----------------//
	[SerializeField]
	[Range(0, 3)]
	private float MoveForce = 1;

	[SerializeField]
	[Range(0, 1)]
	private float TorqueForce = 1;

	[ReadOnly]
	[SerializeField]
	private float CurentTorque;
	// ----------------//
	// --- 公有成员
	// ----------------//
	public bool Working
	{
		private set
		{
			m_working = value;
			m_rigidbody.gravityScale = m_working ? 0 : 1;
			m_rigidbody.drag = m_working ? 1 : 0.1f;
			m_rigidbody.angularDrag = m_working ? 5 : 0.1f;
		}
		get => m_working;
	}
	private bool m_working = false;

	// ----------------//
	// --- 私有成员
	// ----------------//
	private Key RightMoveKey = Key.D;
	private Key LeftMoveKey = Key.A;
	private Key UpMoveKey = Key.W;
	private Key DownMoveKey = Key.S;

	private Rigidbody2D m_rigidbody { get { if (mm_rigidbody == null) mm_rigidbody = GetComponent<Rigidbody2D>(); return mm_rigidbody; } }
	private Rigidbody2D mm_rigidbody;

	private float m_moveTime = 0;
	private float m_accelerationTime = 0.3f; // 加速度从0到满的时间
	private Vector2 m_TargetUpDir = Vector2.up;
	private readonly Vector2 m_RightMoveStateDir = new Vector2(1, 2);
	private readonly Vector2 m_LeftMoveStateDir = new Vector2(-1, 2);

	// ----------------//
	// --- Unity消息
	// ----------------//
	private void Awake()
	{
		m_rigidbody.gravityScale = 0;
	}

	private void FixedUpdate()
	{
		UpdateFlyActor();
	}

	private void UpdateFlyActor()
	{
		// 飞行控制
		Vector2 moveDir = Vector2.zero;
		if (Keyboard.current[RightMoveKey].isPressed) moveDir += Vector2.right;
		if (Keyboard.current[LeftMoveKey].isPressed) moveDir += Vector2.left;
		if (Keyboard.current[UpMoveKey].isPressed) moveDir += Vector2.up;
		if (Keyboard.current[DownMoveKey].isPressed) moveDir += Vector2.down;
		// 旋转力矩
		if (moveDir.x == 0)
		{
			m_TargetUpDir = Vector2.up;
		}
		else
		{ 
			m_TargetUpDir = moveDir.x > 0 ? m_RightMoveStateDir : m_LeftMoveStateDir;
		}
		float curUpAngle = Vector2.SignedAngle(m_TargetUpDir, transform.up);
		m_rigidbody.AddTorque(-curUpAngle * TorqueForce); // 可以视为经验值或者力臂
		// 重力调整(倾斜角过大时，模拟升力减小)
		if (moveDir != Vector2.zero)
		{
			float forceAtXAxis = Mathf.Abs(MoveForce * transform.up.x / 1f);
			// 第一:重力的存在,使用0比例重力模拟时,需要在上升力上减少而下降力上增加
			// 第二:x轴向施加的力,是按照飞机自身Y轴方向在世界X轴向上的投影长占总长的比例,所以X轴向最大力为最大值的一半
			float forceAtUpYAxis = Mathf.Abs(MoveForce * transform.up.y / 1f * 0.8f / 2.5f) ;
			float forceAtDownYAxis = Mathf.Abs(MoveForce * transform.up.y / 1f * 1.2f/ 2.5f) ;
			Vector2 force = new Vector2(moveDir.x * forceAtXAxis, moveDir.y * (moveDir.y > 0 ? forceAtUpYAxis : forceAtDownYAxis));
			m_rigidbody.AddForce(force);
		}
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
