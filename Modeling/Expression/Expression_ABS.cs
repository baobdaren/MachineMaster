using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Expression_ABS: ExpressionBase
{

	// --------------- //
	// --  公有成员
	// --------------- //
	public override ExpressionID ID => ExpressionID.MATH_ABS;
	public override List<ExpressionSettingItem> SettingItemList => null;
	public override SorttingType SortType => SorttingType.FUNC;
	public override int InputAmount => 1;
	public override int OutputAmount => 1;

	// --------------- //
	// --  私有成员
	// --------------- //

	// --------------- //
	// --  公有方法
	// --------------- //
	public Expression_ABS ( ):base()
	{
	}


	// --------------- //
	// --  私有方法
	// --------------- //
	protected override float[] ExpressionFunc(float[] args, ExpressionSettingsValueSet settingValue)
	{
		if (args.Length >= InputAmount)
		{
			return new float[] { Mathf.Abs(args[0]), -1 * Mathf.Abs(args[0]) };
		}
		else
		{
			return ZERO;
		}
	}


}
