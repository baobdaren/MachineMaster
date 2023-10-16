using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Expression_PRD : ExpressionBase
{
	// ------------- //
	// -- 公有成员
	// ------------- //
	public override int InputAmount => 2;
	public override int OutputAmount => 1;
	public override ExpressionID ID => ExpressionID.MATH_PRD;
	public override SorttingType SortType => SorttingType.FUNC;


	// ------------- //
	// -- 私有成员
	// ------------- //


	// ------------- //
	// -- 公有方法
	// ------------- //
	public Expression_PRD ( ) : base() { }


	// ------------- //
	// -- 私有方法
	// ------------- //
	protected override float[] ExpressionFunc ( float[] args, ExpressionSettingsValueSet settingValue)
	{
		if(args.Length >= 1)
		{
			float result = 1.0f;
			for(int i = 0; i < args.Length; i++)
			{
				result *= args[i];
			}
			return new float[] { result };
		}
		else
		{
			return ZERO;
		}
	}


}
