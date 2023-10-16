using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Expression_DistanceSensor : ExpressionSensorBase
{

	//public override string FuncText => $"距离传感器(X：最近距离，Y：最远距离；A：当前距离)";

	//public override int InputAmount => 2;

	//public override int OutputAmount => 1;

	protected override float[] ExpressionFunc ( float[] args, ExpressionSettingsValueSet settingValue)
	{
		if(UsingSensor<DistanceSensor>() == null)
		{
			Debug.LogError("传感器没有设置");
			return new float[] { 0 };
		}
		float min = 0;
		float max = float.MaxValue;
		if(args.Length > 0)
		{
			min = args[0];
		}
		if(args.Length > 1)
		{
			max = args[1];
		}
		Debug.LogError("距离传感器表达式输出距离为：" + UsingSensor<DistanceSensor>().DectedDistance);
		return new float[] { Mathf.Clamp(UsingSensor<DistanceSensor>().DectedDistance, min, max) };
	}

	// ------------- //
	// -- 序列化
	// ------------- //


	// ------------- //
	// -- 公有成员
	// ------------- //
	public override int InputAmount => 1;

	public override int OutputAmount => 1;
	public override ExpressionID ID => ExpressionID.SENSOR_DISTANCE;
	public override SorttingType SortType => SorttingType.SENSOR;

	// ------------- //
	// -- 私有成员
	// ------------- //
	protected override Type SensorExpressionType => typeof(DistanceSensor);


	// ------------- //
	///// Unity消息
	// ------------- //

	// ------------- //
	// -- 公有方法
	// ------------- //

	// ------------- //
	// -- 私有方法
	// ------------- //

}
