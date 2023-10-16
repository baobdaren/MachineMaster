using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Expression_NUM : ExpressionBase
{
	// ------------- //
	// -- 公有成员
	// ------------- //;
	public override ExpressionID ID => ExpressionID.MATH_NUM;
	public override List<ExpressionSettingItem> SettingItemList => _settingItemList;
	public override SorttingType SortType => SorttingType.OUTPUT;
	public override int InputAmount => 0;
	public override int OutputAmount => 1;

	// ------------- //
	// -- 私有成员
	// ------------- //
	private List<ExpressionSettingItem> _settingItemList = new List<ExpressionSettingItem>()
	{
		new ExpressionSettingItem("数值", 0)
	};

	// ------------- //
	// -- 公有方法
	// ------------- //


	// ------------- //
	// -- 私有方法
	// ------------- //
	protected override float[] ExpressionFunc ( float[] args, ExpressionSettingsValueSet settingValue)
	{
		return new float[] { settingValue.SettingValue[0] };
	}
	//protected override List<ExpressionSetting> CreateSettingList ( )
	//{
	//	return new List<ExpressionSetting>()
	//	{
	//		new ExpressionSetting(NUM, ExpressionSettingType.NUM, 1)
	//	};
	//}
}
