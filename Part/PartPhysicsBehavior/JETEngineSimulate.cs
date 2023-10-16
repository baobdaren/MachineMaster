using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JETEngineSimulate/*:AbsProgrammablePartBase*/
{

	// ----------------//
	// --- 序列化
	// ----------------//


	// ----------------//
	// --- 公有成员
	// ----------------//


	// ----------------//
	// --- 私有成员
	// ----------------//
	//private JETEngineAccesstor MyAccesstor { get => base.Accesstor as JETEngineAccesstor; }

	//protected override ExpressionID GetRootExpression => ExpressionID.INPUT_JETEng;

	//protected override IInterpreter GetInterpreter => MyInterpreter;

	//private float _targetTailAngle = 0f;
	//private JETEngineInterpreter MyInterpreter = new JETEngineInterpreter();

	//// ----------------//
	//// --- Unity消息
	//// ----------------//
	//protected override void Start()
	//{
	//	base.Start();
	//	MyAccesstor.TrialParticleSystem.gameObject.SetActive(true);
	//	MyAccesstor.TrialParticleSystem.Play();
	//	StartCoroutine(Cor_RotateTail());
	//}


	// ----------------//
	// --- 公有方法
	// ----------------//
	//public override void OnProgramablePartSet(float[] args)
	//{

	//}


	//// ----------------//
	//// --- 私有方法
	//// ----------------//
	//private Color GetFireColor(float force)
	//{
	//	force = Mathf.Clamp(force, 0, MyAccesstor._maxForce);
	//	int percent = (int)(force / MyAccesstor._maxForce * _speedToColor.Count - 1);
	//	percent = Mathf.Clamp(percent, 0, _speedToColor.Count - 1);
	//	//Debug.LogError("速度占比" + percent);
	//	return _speedToColor[percent];
	//}
	//private IEnumerator Cor_RotateTail()
	//{
	//	while (true)
	//	{
	//		yield return 0;
	//		float a = Mathf.Clamp(Mathf.Clamp(_targetTailAngle, -20, 20) - MyAccesstor._engineJoint.limits.max, -0.1f, 0.1f);
	//		a += MyAccesstor._engineJoint.limits.max;
	//		MyAccesstor._engineJoint.limits = new JointAngleLimits2D() { max = a, min = a };
	//	}
	//}

	//public override void On_InterpreterUpdated()
	//{
	//	Vector2 addForce = MyAccesstor._engineTail.TransformDirection(Vector2.left);
	//	if (args.Length >= 1)
	//	{
	//		float force = Mathf.Clamp(args[0], 0, 10000);
	//		addForce *= force;
	//		var psTrails = MyAccesstor.TrialParticleSystem.trails;
	//		psTrails.colorOverTrail = new ParticleSystem.MinMaxGradient(GetFireColor(addForce.magnitude));
	//		var psMain = MyAccesstor.TrialParticleSystem.main;
	//		psMain.startSpeed = MyAccesstor._maxFireSpeed * ((force / MyAccesstor._maxForce) * 0.5f + 0.5f);
	//		if (args.Length >= 2)
	//		{
	//			Debug.LogError("设置转角: " + args[1]);
	//			_targetTailAngle = args[1];
	//		}
	//	}
	//	MyAccesstor._engineTailRigid.AddForce(addForce);
	//}

	// ----------------//
	// --- 类型
	// ----------------//
	readonly List<Color> _speedToColor = new List<Color>()
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
