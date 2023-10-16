using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class GearHeadFlag : SerializedMonoBehaviour
{
	// ----------------//
	// --- 序列化
	// ----------------//

	// ----------------//
	// --- 公有成员
	// ----------------//
	public List<Collider2D> AllHeadColliders => AllHeadColliders;
	// ----------------//
	// --- 私有成员
	// ----------------//
	[ReadOnly]
	[SerializeField]
	private List<Collider2D> allHeadColliders;

	// ----------------//
	// --- Unity消息
	// ----------------//

	// ----------------//
	// --- 公有方法
	// ----------------//
	private void Reset()
	{
		allHeadColliders = new List<Collider2D>(GetComponentsInChildren<Collider2D>());
		Vector2 capSize = new Vector2(0.05483254f, 0.1148894f);
		allHeadColliders.RemoveAll(
			(Collider2D col) =>
		{
			if (col is CapsuleCollider2D)
			{
				return (col as CapsuleCollider2D).size != capSize;
			}
			else if (col is CircleCollider2D)
			{
				return (col as CircleCollider2D).radius != 0.0184506f;
			}
			return true;
		});
		Debug.Log($"{name} + 有 {allHeadColliders.Count} 个");
	}

	// ----------------//
	// --- 私有方法
	// ----------------//

	// ----------------//
	// --- 类型
	// ----------------//
}
