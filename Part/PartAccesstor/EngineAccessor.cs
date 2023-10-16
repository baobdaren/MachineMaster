using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineAccessor : AbsBasePlayerPartAccessor
{
	// ----------------//
	// --- 序列化
	// ----------------//
	[SerializeField]
	public HingeJoint2D EngineFrameJoint;
	[SerializeField]
	private SpriteRenderer TopRender;
	[SerializeField]
	public GameObject Bottom;

	[HideInInspector]
	public List<GameObject> GearHeads = new List<GameObject>();
	// ----------------//
	// --- 公有成员
	// ----------------//
	public Rigidbody2D TopRigid
	{
		get
		{
			if (_topRigid == null)
			{
				_topRigid = GetComponent<Rigidbody2D>();
			}
			return _topRigid;
		}
	}
	public Rigidbody2D BottomRigid
	{
		get
		{
			if (_bottomRigid == null)
			{
				_bottomRigid = Bottom.GetComponentInChildren<Rigidbody2D>();
			}
			return _bottomRigid;
		}
	}


	// ----------------//
	// --- 私有成员
	// ----------------//
	private Rigidbody2D _topRigid;
	private Rigidbody2D _bottomRigid;



	// ----------------//
	// --- Unity消息
	// ----------------//

	// ----------------//
	// --- 公有方法
	// ----------------//

	// ----------------//
	// --- 私有方法
	// ----------------//
	protected override List<Renderer> InitGetAllRenders()
	{
		return new List<Renderer>()
		{
			Bottom.GetComponent<Renderer>(),
			TopRender
		};
	}


	// ----------------//
	// --- 类型
	// ----------------//

}
