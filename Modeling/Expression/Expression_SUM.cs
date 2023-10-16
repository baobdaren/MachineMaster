using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Expression_SUM : ExpressionBase
{
	// ------------- //
	// -- 公有成员
	// ------------- //
	public override int InputAmount => 2;
	public override int OutputAmount => 1;
	public override ExpressionID ID => ExpressionID.MATH_SUM;
	public override SorttingType SortType => SorttingType.FUNC;


	// ------------- //
	// -- 私有成员
	// ------------- //



	// ------------- //
	// -- 公有方法
	// ------------- //
	public Expression_SUM ( ) : base() { }

	// ------------- //
	// -- 私有方法
	// ------------- //
	 protected override float[] ExpressionFunc ( float[] args, ExpressionSettingsValueSet settingValue)
	{
		if(args.Length == 0)
		{
			return ZERO;
		}
		float result = 0;
		for(int i = 0; i < args.Length; i++)
		{
			result += args[i];
		}
		return new float[] { result };
	}
}
