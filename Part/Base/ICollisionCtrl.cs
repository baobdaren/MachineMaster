using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public interface ICollisionCtrl
{
	// ----------------//
	// --- 序列化
	// ----------------//

	// ----------------//
	// --- 公有成员
	// ----------------//
	public abstract int Layer { get; }

	// ----------------//
	// --- 私有成员
	// ----------------//
	public PartTypes MyPartType { get; }
	public IEnumerable<Collider2D> GetPhysicsColliders { get; }

	// ----------------//
	// --- Unity消息
	// ----------------//

	// ----------------//
	// --- 公有方法
	// ----------------//
	public IEnumerable<Collider2D> GetColliders_CollisionTest(PartTypes partType);

	public bool OverlapOther(ICollisionCtrl other);
	//public void IgnoreCollision(ICollisionCtrl other);

	// ----------------//
	// --- 私有方法
	// ----------------//

	// ----------------//
	// --- 类型
	// ----------------//
}
