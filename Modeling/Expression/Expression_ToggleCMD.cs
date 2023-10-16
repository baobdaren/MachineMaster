using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Expression_ToggleCMD : ExpressionCmdBase
{
	// ----------------//
	// --- 序列化
	// ----------------//


	// ----------------//
	// --- 公有成员
	// ----------------//
	public override int InputAmount => 0;
	public override int OutputAmount => 1;
	public override ExpressionID ID => ExpressionID.CMD_TOGGLE;
	public override List<ExpressionSettingItem> SettingItemList => _expressionSettingItems;


	// ----------------//
	// --- 私有成员
	// ----------------//
	private List<ExpressionSettingItem> _expressionSettingItems = new List<ExpressionSettingItem>()
	{
		new ExpressionSettingItem("默认值", true),
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
		return new float[] { args[0] == 0 ? 0 : 1 };
	}
}
