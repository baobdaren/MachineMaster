using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Spring : PlayerPartBase
{
	// -------------- //
	// --- 序列化
	// -------------- //

	// -------------- //
	// --- 私有成员
	// -------------- //
	private bool isBrokenFlag = false;

	// -------------- //
	// --- 公有成员
	// -------------- //
	public SpringAccessor MyAccesstor { get => Accessor as SpringAccessor; }


	// -------------- //
	// --- Unity消息
	// -------------- //
	protected void FixedUpdate()
	{
		if (isBrokenFlag)
		{
			return;
		}
		if (MyAccesstor.SpringJoint == null)
		{ 
			if (MyAccesstor.SliderJoint != null)
			{ 
				Destroy(MyAccesstor.SpringJoint);
				enabled = false;
				return;
			}
		}
		if (MyAccesstor.SliderJoint == null)
		{ 
			if (MyAccesstor.SpringJoint != null)
			{ 
				Destroy(MyAccesstor.SliderJoint);
				enabled = false;
			}
		}
	}


	// -------------- //
	// --- 私有方法
	// -------------- //


	// -------------- //
	// --- 公有方法
	// -------------- //


	// -------------- //
	// --- 类型
	// -------------- //



}
