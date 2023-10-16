using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class TestGearMaterial : SerializedMonoBehaviour
{
	// ----------------//
	// --- 序列化
	// ----------------//
	public SpriteRenderer BodySpriteRender;
	public SpriteRenderer GearHeadSpriteRender;

	// ----------------//
	// --- 公有成员
	// ----------------//

	// ----------------//
	// --- 私有成员
	// ----------------//

	// ----------------//
	// --- Unity消息
	// ----------------//
	private void Update()
	{
		Vector2 scale = BodySpriteRender.transform.lossyScale;
		GearHeadSpriteRender.material.SetVector("Scale", scale);
		GearHeadSpriteRender.material.SetFloat("Distance", Vector2.Distance(GearHeadSpriteRender.transform.localPosition, Vector2.zero));
	}

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
