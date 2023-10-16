using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringAccessor : AbsBasePlayerPartAccessor
{
	// ----------------//
	// --- 序列化
	// ----------------//

	// ----------------//
	// --- 公有成员
	// ----------------//
	public GameObject Top;
	public GameObject Bottom;
	public SpringJoint2D SpringJoint;
	public SliderJoint2D SliderJoint;

	// ----------------//
	// --- 私有成员
	// ----------------//

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
			Top.GetComponent<Renderer>()
		};
	}

	// ----------------//
	// --- 类型
	// ----------------//

}
