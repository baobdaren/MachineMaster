using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ExpressionSensorBase : ExpressionBase
{
	////////////////////
	// -- 序列化
	////////////////////


	////////////////////
	// -- 公有成员
	////////////////////
	//public sealed override IOType GetIOType => IOType.SENSOR;
	// 提供这个传感器表达式所代表的传感器的类型的字符串形式
	// 用于在预览传感器时区分不同传感器，sensormanager保存时也使用传感器类型的字符串形式为字典键值对的值
	public string GetSensorType
	{
		get
		{
			return SensorExpressionType.ToString();
		}
	}
	// 传感器表达式使用的传感器。
	public BaseSensor Sensor
	{
		set
		{
			_sensor = value;
		}
	}
	private BaseSensor _sensor;
	public T UsingSensor<T> ( ) where T : BaseSensor
	{
		return _sensor as T;
	}
	// 子类指定到底使用哪种类型
	protected abstract System.Type SensorExpressionType
	{
		get;
	}
	////////////////////
	// -- 私有成员
	////////////////////


	////////////////////
	///// Unity消息
	////////////////////


	////////////////////
	// -- 公有方法
	////////////////////


	////////////////////
	// -- 私有方法
	////////////////////

}
