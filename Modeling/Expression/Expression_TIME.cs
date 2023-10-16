using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Expression_TIME : ExpressionBase
{
	// ------------- //
	// -- 公有成员
	// ------------- //
	public override int InputAmount => 0;
	public override int OutputAmount => 1;
	public override ExpressionID ID => ExpressionID.MATH_TIME;
	public override List<ExpressionSettingItem> SettingItemList => _expressionSettingItems;
	public override SorttingType SortType => SorttingType.OUTPUT;

	// ----------------//
	// --- 私有成员
	// ----------------//
	private List<ExpressionSettingItem> _expressionSettingItems = new List<ExpressionSettingItem>()
	{
		new ExpressionSettingItem("缩放系数", 1),
	};


	// ------------- //
	// -- 公有方法
	// ------------- //
	public Expression_TIME ( ) : base() { }


	// ------------- //
	// -- 私有方法
	// ------------- //
	 protected override float[] ExpressionFunc ( float[] args, ExpressionSettingsValueSet settingValue)
	{
		return new float[] { Time.time };
	}
}
