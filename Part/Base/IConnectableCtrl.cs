using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public interface IConnectableCtrl
{
	// ----------------//
	// --- 序列化
	// ----------------//

	// ----------------//
	// --- 公有成员
	// ----------------//
	public List<Collider2D> GetIgnorableColliders { get; }

	// ----------------//
	// --- 私有成员
	// ----------------//
	public List<Collider2D> GetEditConnectableColliders { get; }
	public List<Collider2D> GetPhysicsConnectableColliders { get; }

	// ----------------//
	// --- Unity消息
	// ----------------//

	// ----------------//
	// --- 公有方法
	// ----------------//
	public bool OverlapPoint(Vector2 pos);
	public bool TryGetOverlapPointEditColliderIndex(Vector2 point, out int index);
	public Collider2D GetPhysicsCollider(int editIndex);
	public Collider2D GetPhysicsCollider(Collider2D editCollider);
	// ----------------//
	// --- 私有方法
	// ----------------//

	// ----------------//
	// --- 类型
	// ----------------//
}
