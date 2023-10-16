using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Expression_SliderCmd : ExpressionCmdBase
{
	// ----------------//
	// --- 序列化
	// ----------------//


	// ----------------//
	// --- 公有成员
	// ----------------//
	public override int InputAmount => 0;
	public override int OutputAmount => 1;
	public override ExpressionID ID => ExpressionID.CMD_SLIDER;
	public override List<ExpressionSettingItem> SettingItemList => _expressionSettingItems;


	// ----------------//
	// --- 私有成员
	// ----------------//
	private List<ExpressionSettingItem> _expressionSettingItems = new List<ExpressionSettingItem>()
	{
		new ExpressionSettingItem("起始值", 0),
		new ExpressionSettingItem("最终值", 1)
	};


	// ----------------//
	// --- Unity消息
	// ----------------//


	// ----------------//
	// --- 公有方法
	// ----------------//


	// ----------------//
	// --- 私有方法
	// ----------------//
	protected override float[] ExpressionFunc(float[] args, ExpressionSettingsValueSet settingValue)
	{
		return new float[] { Mathf.Clamp( args[0], settingValue[0], settingValue[1] )};
	}
}
