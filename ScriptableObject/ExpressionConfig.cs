using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ExpressionConfig", menuName = "创建数据文件/创建表达式数据文件", order = 20)]
public class ExpressionConfig : UniqueConfigBase
{
	// ----------------//
	// --- 公有成员
	// ----------------//
	public static ExpressionConfig Instance
	{
		set => _instance = value;
		get => _instance;
	}
	private static ExpressionConfig _instance;

	[Header("数学")]
	public Expression_ABS Math_ABS;
	public Expression_CMP Math_CMP;
	public Expression_POW Math_POW;
	public Expression_PRD Math_PRD;
	public Expression_SIN Math_SIN;
	public Expression_SUM Math_SUM;
	[Header("输出")]
	public Expression_RND Math_RND;
	public Expression_NUM Math_NUM;
	public Expression_TIME Math_TIME;
	[Header("输入符号")]
	public Expression_AircraftEngineInput Input_AirEng;
	public Expression_EngineInput Input_Engine;
	public Expression_PresserInput Input_Presser;
	[Header("传感器")]
	public Expression_DistanceSensor Sensor_Distance;
	public Expression_VeclovityAngleSensor Sensor_VecAng;
	[Header("控制台")]
	public Expression_SliderCmd Cmd_Slider;
	public Expression_ToggleCMD Cmd_Toggle;
	//public Expression 
	//public Expression_ABS2 absExpresion;

	// ----------------//
	// --- 私有成员
	// ----------------//
	[HideInInspector]
	public Dictionary<ExpressionID, ExpressionBase> AllExpressionDict = new Dictionary<ExpressionID, ExpressionBase>();

	// ----------------//
	// --- Unity消息
	// ----------------//

	// ----------------//
	// --- 公有方法
	// ----------------//
	public ExpressionBase GetExpression(ExpressionID id)
	{
		return AllExpressionDict[id];
	}

	// ----------------//
	// --- 私有方法
	// ----------------//
	private void QuickRegist(ExpressionBase expression)
	{
		AllExpressionDict.Add(expression.ID, expression);
	}

	public override void InitInstance()
	{
		Instance = this;
		AllExpressionDict.Clear();
		QuickRegist(Math_ABS);
		QuickRegist(Math_CMP);
		QuickRegist(Math_NUM);
		QuickRegist(Math_POW);
		QuickRegist(Math_PRD);
		QuickRegist(Math_RND);
		QuickRegist(Math_SIN);
		QuickRegist(Math_SUM);
		QuickRegist(Math_TIME);
		QuickRegist(Input_AirEng);
		QuickRegist(Input_Engine);
		QuickRegist(Input_Presser);
		QuickRegist(Sensor_Distance);
		QuickRegist(Sensor_VecAng);
		QuickRegist(Cmd_Slider);
		QuickRegist(Cmd_Toggle);
	}

	// ----------------//
	// --- 类型
	// ----------------//
}

public enum ExpressionID
{
	CMD_SLIDER,
	CMD_TOGGLE,

	MATH_ABS, // 绝对值
	MATH_CMP, // 大小比较
	MATH_NUM, // 常数
	MATH_POW, // 次方
	MATH_PRD, // 乘以
	MATH_RND, // 随机数字
	MATH_SIN, // 正弦
	MATH_SUM,
	MATH_TIME,// 时间

	INPUT_JETEng, // 喷气发动机
	INPUT_ENGINE, // 电机
	INPUT_PRESSER,// 液压柱

	SENSOR_DISTANCE, // 距离传感器
	SENSOR_GYRO // 速度角度传感器
}