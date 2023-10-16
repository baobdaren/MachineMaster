using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Expression_CMP : ExpressionBase
{
	public override ExpressionID ID => ExpressionID.MATH_CMP;
	public override SorttingType SortType => SorttingType.FUNC;
	public override int OutputAmount => 1;
	public override int InputAmount => 2;


	// ------------- //
	// -- 序列化
	// ------------- //


	// ------------- //
	// -- 公有成员
	// ------------- //


	// ------------- //
	// -- 私有成员
	// ------------- //


	// ------------- //
	///// Unity消息
	// ------------- //


	// ------------- //
	// -- 公有方法
	// ------------- //


	// ------------- //
	// -- 私有方法
	// ------------- //
	protected override float[] ExpressionFunc(float[] args, ExpressionSettingsValueSet settingValue)
	{
		return new float[] { args[0] > args[1] ? 1 : -1 };
	}
}
