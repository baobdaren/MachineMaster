using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Expression_VeclovityAngleSensor : ExpressionSensorBase
{

	// ------------- //
	// -- 序列化
	// ------------- //


	// ------------- //
	// -- 公有成员
	// ------------- //
	public override int InputAmount => 0;
	public override int OutputAmount => 3;
	public override ExpressionID ID => ExpressionID.SENSOR_GYRO;
	public override SorttingType SortType => SorttingType.SENSOR;

	// ------------- //
	// -- 私有成员
	// ------------- //
	protected override Type SensorExpressionType => typeof(AngleVelocitySensor);



	// ------------- //
	// -- 公有方法
	// ------------- //


	// ------------- //
	// -- 私有方法
	// ------------- //
	protected override float[] ExpressionFunc ( float[] args, ExpressionSettingsValueSet settingValue)
	{
		AngleVelocitySensor sensor = UsingSensor<AngleVelocitySensor>();
		Debug.LogError($"陀螺仪结果：X速度{sensor.Velocity.x}，Y速度{sensor.Velocity.y}，角度{sensor.Angle}");
		return new float[] { sensor.Velocity.x, sensor.Velocity.y, sensor.Angle };
		//return new float[] { SensorRigid.velocity.x, SensorRigid.velocity.y, Vector3.Magnitude(SensorRigid.velocity), SensorRigid.transform.eulerAngles.z };
	}

}
