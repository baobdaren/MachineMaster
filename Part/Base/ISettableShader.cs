using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public interface ISettableShader
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
	public void SetOutLineActive(bool active);
	public void SetOutLineColor(Color outlineColor);
	public void SetTexActive(bool active);
	public void SetPureColor(Color texColor);
	public void SetErrorTex(bool active);
	public void SetAlpha(float alpha);


	// ----------------//
	// --- 私有方法
	// ----------------//

	// ----------------//
	// --- 类型
	// ----------------//
}
