using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Expression_PresserInput : ExpressionBase
{
	// ------------- //
	// -- 公有成员
	// ------------- //
	public override int InputAmount => 1;
	public override int OutputAmount => 0;
	public override ExpressionID ID => ExpressionID.INPUT_PRESSER;
	public override SorttingType SortType => SorttingType.INPUT;

	// ------------- //
	// -- 私有成员
	// ------------- //


	// ------------- //
	// -- 公有方法
	// ------------- //


	// ------------- //
	// -- 私有方法
	// ------------- //
	protected override float[] ExpressionFunc ( float[] args, ExpressionSettingsValueSet settingValue)
	{
		return args;
	}

}
