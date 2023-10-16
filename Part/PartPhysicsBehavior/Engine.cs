using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;

public class Engine : AbsProgrammablePartBase
{
    // ------------------- //
    // --  序列化
    // ------------------- //

    // ------------------- //
    // --  公有成员
    // ------------------- //
	public EngineAccessor MyAccesstor { get => Accessor as EngineAccessor; }
    //public float[] GearPosition = new float[] {  } // 档为功能，减少扭矩，增加转速上限

    // ------------------- //
    // --  私有成员 
    // ------------------- //
    protected override ExpressionID GetRootExpression => ExpressionID.INPUT_ENGINE;
    protected override AbsInterpreter GetInterpreter => MyInterpreter;
    private EngineInterpreter MyInterpreter = new EngineInterpreter();


	// -------------- //
	// --  Unity消息
	// -------------- //


	// -------------- //
	// --  私有方法
	// -------------- //

	// -------------- //
	// --  公有方法
	// -------------- //

	public override void On_InterpreterUpdated()
	{
		/* P = T * n / 9550    T = 9550P/n    n = 9550P/T */
		//float P = 9000;
		float currentAngularVelocity = Mathf.Abs(MyAccesstor.TopRigid.angularVelocity - MyAccesstor.BottomRigid.angularVelocity);
		MyAccesstor.EngineFrameJoint.motor = MyInterpreter.GetMotor(currentAngularVelocity);
	}
}
