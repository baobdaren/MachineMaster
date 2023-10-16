using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Expression_SIN : ExpressionBase
{
	// ------------- //
	// -- 公有成员
	// ------------- //
	public override int InputAmount => 1;
	public override int OutputAmount => 1;
	public override ExpressionID ID => ExpressionID.MATH_SIN;
	public override List<ExpressionSettingItem> SettingItemList => _expressionSettingItems;
	public override SorttingType SortType => SorttingType.FUNC;


	// ------------- //
	// -- 私有成员
	// ------------- //
	private List<ExpressionSettingItem> _expressionSettingItems = new List<ExpressionSettingItem>()
	{
		new ExpressionSettingItem("振幅", 1)
	};


	// ------------- //
	// -- 公有方法
	// ------------- //
	public Expression_SIN ( ) : base() { }



	// ------------- //
	// -- 私有方法
	// ------------- //
	protected override float[] ExpressionFunc ( float[] args, ExpressionSettingsValueSet settingValue)
	{
		if(args.Length != 0)
		{
			return new float[] { Mathf.Sin(args[0]) };
		}
		else
		{
			return ZERO;
		}
	}

}
