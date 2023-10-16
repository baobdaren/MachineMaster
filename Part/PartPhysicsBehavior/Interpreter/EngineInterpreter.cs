using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineInterpreter : AbsInterpreter
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


	// ----------------//
	// --- Unity消息
	// ----------------//

	// ----------------//
	// --- 公有方法
	// ----------------//
	public JointMotor2D GetMotor(float currentAngularVelocity)
	{ 
		currentAngularVelocity /= 6; // 转换单位 r/min
		bool forward = RawData[0] > 0;
		RawData[0] = Mathf.Clamp(RawData[0], -1, 1);
		if (RawData[0] == 0)
		{
			return new JointMotor2D ();
		}
		//float powerAbs = 1000 + RawData[0] * PartConfig.Instance.EngineConfig.MaxMotorPower;
		//float targetTorqueAbs = 9550 * Mathf.Abs(powerAbs) / Mathf.Clamp(currentAngularVelocity, 1, float.MaxValue);
		//float targetAbsSpeed = PartConfig.Instance.EngineConfig.MaxMotorSpeed * (powerAbs / PartConfig.Instance.EngineConfig.MaxMotorPower * 0.3f + 0.7f);
		//return (targetTorqueAbs, (forward ? 6 : -6)* targetSpeed); // 不稳定
		// 调整速度 *3
		//return (5000f, (forward ? 6 : -6)* targetSpeed);
		float torque = 0.5f * PartConfig.Instance.EngineConfig.MaxMotorPower * (1f + Mathf.Abs(RawData[0]));
		return new JointMotor2D() { maxMotorTorque = torque, motorSpeed = RawData[0] * PartConfig.Instance.EngineConfig.MaxMotorSpeed };
	}

	// ----------------//
	// --- 私有方法
	// ----------------//

	// ----------------//
	// --- 类型
	// ----------------//

}
