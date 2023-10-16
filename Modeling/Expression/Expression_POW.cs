using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Expression_POW : ExpressionBase
{
	// ---------------- //
	// --  公有成员
	// ---------------- //
	public override int InputAmount => 2;
	public override int OutputAmount => 1;
	public override ExpressionID ID => ExpressionID.MATH_POW;
	public override SorttingType SortType => SorttingType.FUNC;


	// ---------------- //
	// --  私有成员
	// ---------------- //



	// ---------------- //
	// --  公有方法
	// ---------------- //
	public Expression_POW ( ) : base() { }


	// ---------------- //
	// --  私有方法
	// ---------------- //
	 protected override float[] ExpressionFunc ( float[] args, ExpressionSettingsValueSet settingValue)
	{
		if(args.Length >= InputAmount)
		{
			return new float[] { Mathf.Pow(args[0], args[1]) };
		}
		else
		{
			return ZERO;
		}
	}

}
