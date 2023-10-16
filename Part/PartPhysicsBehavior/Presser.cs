using System;
using System.Collections.Generic;
using UnityEngine;

public class Presser : AbsProgrammablePartBase
{
	// ------------- //
	// 序列化成员
	// ------------- //


	// ------------- //
	// 公有成员
	// ------------- //
	public PresserAccessor MyAccesstor { get => Accessor as PresserAccessor; }
	//public event Action OnUpdateSpeed;
	public bool IsBroken { get => !MyAccesstor.SliderJoint || !MyAccesstor.SpringJoint; }


	// ------------- //
	// 私有成员
	// ------------- //

	private float LimitMin { get => PartConfig.Instance.PresserConfig.SpringDistanceMinValue; } // 实际测试值
	private float LimitMax { get => PartConfig.Instance.PresserConfig.SpringDistanceMaxValue; }
	private float StepLimit { get => (LimitMax - LimitMin) * 0.03f * Time.fixedDeltaTime; }
	private float PressDistance
	{
		set
		{
			if (!IsBroken)
			{
				MyAccesstor.SpringJoint.distance = Mathf.Clamp(value, LimitMin, LimitMax);
			}
		}
		get
		{
			return IsBroken ? 0 : MyAccesstor.SpringJoint.distance;
		}
	}
	protected override ExpressionID GetRootExpression => ExpressionID.INPUT_PRESSER;
	protected override AbsInterpreter GetInterpreter => MyInterpreter;
	private PresserInterpreter MyInterpreter = new PresserInterpreter();

	// ------------- //
	// --- Unity消息
	// ------------- //

	protected void Start()
	{
		foreach (var selfCollider in MyAccesstor.Top.GetComponentsInChildren<Collider2D>())
		{
			foreach (var bottomCollider in MyAccesstor.Bottom.GetComponentsInChildren<Collider2D>())
			{
				Physics2D.IgnoreCollision(selfCollider, bottomCollider);
			}
		}
	}

	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		if (IsBroken)
		{
			if (MyAccesstor.SpringJoint)
				Destroy(MyAccesstor.SpringJoint);
			if (MyAccesstor.SliderJoint)
				Destroy(MyAccesstor.SliderJoint);
		}
	}

	// ------------- //
	// --  私有
	// ------------- //
	private void SetPresser(float rate)
	{
		if (!IsBroken)
		{
			//Debug.LogError("设置液压器"+rate);
			rate = Mathf.Clamp01(rate);
			float tarDis = rate * (LimitMax - LimitMin) + LimitMin;
			PressDistance = Mathf.Clamp(tarDis, PressDistance - StepLimit, PressDistance + StepLimit);
		}
	}


	// ------------- //
	// 公有方法
	// ------------- //
	public override void On_InterpreterUpdated()
	{
		SetPresser(MyInterpreter.Distance);
	}
}
