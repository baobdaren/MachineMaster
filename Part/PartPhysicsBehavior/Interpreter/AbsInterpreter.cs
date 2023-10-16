using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbsInterpreter
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
	protected float[] RawData = new float[16];

	// ----------------//
	// --- Unity消息
	// ----------------//

	// ----------------//
	// --- 公有方法
	// ----------------//
	public void SetInputData(float[] args)
	{
		// 没有任何输入连接时参数是null
		if (args == null)
		{
			return;
		}
		//Debug.LogError(args);
		for (int i = 0; i < args.Length && i < RawData.Length; i++)
		{
			RawData[i] = args[i];
		}
	}

	// ----------------//
	// --- 私有方法
	// ----------------//

	// ----------------//
	// --- 类型
	// ----------------//

}
