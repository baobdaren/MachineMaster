using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]

public class Expression_AircraftEngineInput : ExpressionBase
{
	// ------------- //
	// -- 序列化
	// ------------- //


	// ------------- //
	// -- 公有成员
	// ------------- //
	public override ExpressionID ID => ExpressionID.INPUT_JETEng;
	public override SorttingType SortType => SorttingType.INPUT;
	public override int InputAmount => 1;
	public override int OutputAmount => 1;

	// ------------- //
	// -- 私有成员
	// ------------- //


	// ------------- //
	// -- Unity消息
	// ------------- //


	// ------------- //
	// -- 公有方法
	// ------------- //

	// ------------- //
	// -- 私有方法
	// ------------- //
	protected override float[] ExpressionFunc(float[] args, ExpressionSettingsValueSet settingValue)
	{
		return args;
	}

}
