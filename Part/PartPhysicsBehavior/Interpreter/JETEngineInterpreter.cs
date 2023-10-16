using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JETEngineInterpreter : AbsInterpreter
{
	// ----------------//
	// --- 序列化
	// ----------------//


	// ----------------//
	// --- 公有成员
	// ----------------//
	// 输入的第一个参数是推理角度，输入限制为[-1,1]，这里转换为欧拉角[-90, 90]
	public float GetPushForce { get => Mathf.Abs(RawData[0]) * PartConfig.Instance.JETEngineConfig.PushForceMax; }
	public float GetPushDirection { get => Mathf.Clamp(RawData[1] * 90, -90, 90); }

	// ----------------//
	// --- 私有成员
	// ----------------//


	// ----------------//
	// --- Unity消息
	// ----------------//

	// ----------------//
	// --- 公有方法
	// ----------------//


	// ----------------//
	// --- 私有方法
	// ----------------//

	// ----------------//
	// --- 类型
	// ----------------//

}
