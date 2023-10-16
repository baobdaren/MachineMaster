using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JETEngine : AbsProgrammablePartBase
{
	// ------------- //
	// --  私有成员
	// ------------- //
	private float _targetForce;

	private float _targetDirect;
	private float _currentDirect;

	private Rigidbody2D EngineRigidBody 
	{
		get
		{
			if (_engineBodyCache == null)
				_engineBodyCache = GetComponentInChildren<Rigidbody2D>();
			return _engineBodyCache;
		}
	}
	private Rigidbody2D _engineBodyCache;
	private JETEngineInterpreter MyInterpreter = new JETEngineInterpreter();
	protected override ExpressionID GetRootExpression { get => ExpressionID.INPUT_JETEng; }
	protected override AbsInterpreter GetInterpreter { get => MyInterpreter as AbsInterpreter; }

	// ------------- //
	// -- 公有成员
	// ------------- // 
	public JETEngineAccessor MyAccesstor { get => Accessor as JETEngineAccessor; }



	// ------------- //
	// --  Unity消息
	// ------------- //
	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		if (MyInterpreter.GetPushDirection != _currentDirect)
		{
			float rotateAngle = MyInterpreter.GetPushDirection - _currentDirect;
			rotateAngle = Mathf.Clamp(rotateAngle, rotateAngle - PartConfig.Instance.JETEngineConfig.AnglePerSecond, rotateAngle + PartConfig.Instance.JETEngineConfig.AnglePerSecond);
			//MyAccesstor.FlameVFX.transform.Rotate(Vector3.forward, rotateAngle);
		}
		float pushForce = MyInterpreter.GetPushForce / Time.fixedDeltaTime;
		if (pushForce != 0)
		{
			//Debug.LogError("施加力" + pushForce);
			EngineRigidBody.AddRelativeForce(pushForce * Vector2.left, ForceMode2D.Force);
		}
		// VFX 的方式 由于VFX不支持2D URP 以后Unity支持了在用
		//MyAccesstor.FlameVFX.SetFloat("speed", _targetForce / PartConfig.Instance.JETEngineConfig.SliderMaxValue);
		//Debug.LogError("设置speed" + MyAccesstor.FlameVFX.GetFloat("speed"));
		// 旧版particle方式
		var cmpnt = MyAccesstor.FlameVFX.velocityOverLifetime;
		cmpnt.speedModifier = (_targetForce / PartConfig.Instance.JETEngineConfig.SpecLower * 0.5f);

		//var mainCmpnt = MyAccesstor.TrialParticleSystem.main;
		//mainCmpnt.startColor = _speedToColor[(int)(_targetForce / PartConfig.Instance.JETEngineConfig.SliderMaxValue) * _speedToColor.Count];
	}

	// ------------- //
	// -- 公有方法
	// ------------- //

	public override void On_InterpreterUpdated()
	{
		_targetForce = MyInterpreter.GetPushForce;
		_targetDirect = MyInterpreter.GetPushDirection;
	}


	// ------------- //
	// --  私有方法
	// ------------- //

	private static List<Color> _speedToColor = new List<Color>()
	{
		new Color(1.000f, 0.176f, 0.172f, 1),
		new Color(1.000f, 0.268f, 0.176f, 1),
		new Color(0.968f, 0.733f, 0.141f, 1),
		new Color(0.988f, 0.866f, 0.623f, 1),
		new Color(0.415f, 0.749f, 0.956f, 1),
		new Color(0.105f, 0.647f, 0.925f, 1),
		new Color(0.105f, 0.647f, 0.925f, 1),
	};
}
