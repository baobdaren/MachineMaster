using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpressionManager/*:ResetAbleInstance<ExpressionManager>*/
{
	public static ExpressionManager Instance
	{
		get
		{
			if (_instance == null)
			{
				Resources.Load("DataFile/ExpressionConfig");
				_instance = new ExpressionManager();
			}
			return _instance;
		}
	}
	private static ExpressionManager _instance;

	public Dictionary<ExpressionID, ExpressionBase> ALLExpressions { private set; get; } = new Dictionary<ExpressionID, ExpressionBase>();

	public void RegistExpression<T>(ExpressionID expressionType) where T : ExpressionBase, new()
	{
		if (ALLExpressions.ContainsKey(expressionType))
		{
			ALLExpressions.Add(expressionType, new T());
		}
	}


	/// <summary>
	/// 注册所有可以创建的表达式类型
	/// </summary>
	public void InitExpression()
	{
		RegistExpression<Expression_NUM>(ExpressionID.MATH_NUM);
		RegistExpression<Expression_CMP>(ExpressionID.MATH_CMP);
		RegistExpression<Expression_ABS>(ExpressionID.MATH_ABS);
		RegistExpression<Expression_POW>(ExpressionID.MATH_POW);
		RegistExpression<Expression_PRD>(ExpressionID.MATH_PRD);
		RegistExpression<Expression_RND>(ExpressionID.MATH_RND);
		RegistExpression<Expression_SIN>(ExpressionID.MATH_SIN);
		RegistExpression<Expression_SUM>(ExpressionID.MATH_SUM);
		RegistExpression<Expression_TIME>(ExpressionID.MATH_TIME);
		//RegistExpression<Expression_EngineInput>();
		//RegistExpression<Expression_PresserInput>();
		RegistExpression<Expression_DistanceSensor>(ExpressionID.SENSOR_DISTANCE);
		RegistExpression<Expression_VeclovityAngleSensor>(ExpressionID.SENSOR_GYRO);
		RegistExpression<Expression_ToggleCMD>(ExpressionID.CMD_TOGGLE);
		RegistExpression<Expression_SliderCmd>(ExpressionID.CMD_SLIDER);
	}
	public string GetExpressionType<T>() where T : ExpressionBase
	{
		return typeof(T).ToString();
	}
}


