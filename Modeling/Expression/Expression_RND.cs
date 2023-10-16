using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Expression_RND : ExpressionBase
{
	// ------------- //
	// --  公有成员
	// ------------- //
	public override int InputAmount => 0;
	public override int OutputAmount => 1;
	public override ExpressionID ID => ExpressionID.MATH_RND;
	public override List<ExpressionSettingItem> SettingItemList => _expressionSettingItems;
	public override SorttingType SortType => SorttingType.OUTPUT;


	// ------------- //
	// --  私有成员
	// ------------- //
	public List<ExpressionSettingItem> _expressionSettingItems = new List<ExpressionSettingItem>()
	{
		new ExpressionSettingItem("下限", 0),
		new ExpressionSettingItem("上限", 1)
	};

	// ------------- //
	// --  公有方法
	// ------------- //
	public Expression_RND ( ) : base() { }


	// ------------- //
	// --  私有方法
	// ------------- //
	 protected override float[] ExpressionFunc ( float[] args, ExpressionSettingsValueSet settingValue)
	{
		float _lastValue = UnityEngine.Random.Range(Math.Min(args[0], args[1]), Mathf.Max(args[0],args[1]));
		return new float[] { _lastValue };
	}

	//protected override List<ExpressionSetting> CreateSettingList ( )
	//{
	//	return new List<ExpressionSetting>()
	//	{
	//		new ExpressionSetting(RND_MAX, ExpressionSettingType.NUM, 1),
	//		new ExpressionSetting(RND_MIN, ExpressionSettingType.NUM, 0),
	//		new ExpressionSetting(RND_STEP, ExpressionSettingType.NUM, 0.01f)
	//	};
	//}
}
