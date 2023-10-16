using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.U2D;

public abstract class BasePartCtrl : IConnectableCtrl, ICollisionCtrl, ISettableShader
{
	// ----------------//
	// --- 序列化
	// ----------------//

	// ----------------//
	// --- 公有成员
	// ----------------//
	public abstract PartTypes MyPartType { get; }
	public abstract bool IsPlayerPart { get; }

	public abstract List<Collider2D> GetIgnorableColliders { get; }
	public abstract List<Collider2D> GetEditConnectableColliders { get; }
	public abstract List<Collider2D> GetPhysicsConnectableColliders { get; }
	public abstract IEnumerable<Collider2D> GetPhysicsColliders { get; }

	public abstract SnapableBase PartDragCmpnt { get; }
	public abstract MouseDoubleClick PartDoubleClickCmpnt { get; }

	/// <summary>
	/// 从设计角度 是否可移动
	/// 不保证连接着不可移动的物体
	/// </summary>
	public bool Removeable { get => PartDragCmpnt != null && PartDragCmpnt.Dragable; }
	public Vector2 Position 
	{
		get { return PartCtrlData.Position; }
		set { PartCtrlData.Position = value; OnSettedCtrlData(); }
	}
	public Quaternion Rotation
	{
		get { return PartCtrlData.Rotation; }
		set { PartCtrlData.Rotation = value; OnSettedCtrlData(); }
	}
	public int Layer
	{
		get { return PartCtrlData.Layer; }
		set { PartCtrlData.Layer = value; OnSettedCtrlData(); }
	}
	public float Size
	{
		get { return PartCtrlData.Spec; }
		set { PartCtrlData.Spec = value; OnSettedCtrlData(); }
	}


	// ----------------//
	// --- 私有成员
	// ----------------//
	protected abstract List<Renderer> EditRenders { get; }
	protected abstract List<Renderer> PhysicsRenders { get; }
	protected abstract PartCtrlCoreData PartCtrlData { get; }

	// ----------------//
	// --- 公有方法
	// ----------------//
	public abstract IEnumerable<Collider2D> GetColliders_CollisionTest(PartTypes partType);

	public bool OverlapPoint(Vector2 pos)
	{
		if (GetEditConnectableColliders != null)
		{
			foreach (var item in GetEditConnectableColliders)
			{
				if (item.OverlapPoint(pos))
				{
					return true;
				}
			}
		}
		return false;
	}

	public bool TryGetOverlapPointEditColliderIndex(Vector2 point, out int index)
	{
		for (int i = 0; i < GetEditConnectableColliders.Count; i++)
		{
			if (GetEditConnectableColliders[i].OverlapPoint(point))
			{
				index = i;
				return true;
			}
		}
		index = -1;
		return false;
	}

	public Collider2D GetPhysicsCollider(int editIndex)
	{
		return GetPhysicsConnectableColliders[editIndex];
	}

	public Collider2D GetPhysicsCollider(Collider2D editCollider)
	{
		return GetPhysicsCollider(GetEditConnectableColliders.IndexOf(editCollider));
	}

	public void SetOutLineColor(Color outlineColor)
	{
		SetOutLineActive(true);
		foreach (Renderer itemRenders in EditRenders)
		{
			foreach (Material item in itemRenders.materials)
			{
				item.SetColor("OutlineColor", outlineColor);
				Debug.Assert(item.HasProperty("OutlineColor"), "OutlineColor 材质错误" + item.name, itemRenders.gameObject);
			}
		}
	}
	public void SetOutLineActive(bool active)
	{
		foreach (Renderer itemRenders in EditRenders)
		{
			foreach (Material item in itemRenders.materials)
			{
				item.SetInt("UseOutline", active ? 1 : 0);
				Debug.Assert(item.HasProperty("UseOutline"), "UseOutline 材质错误" + item.name, itemRenders.gameObject);
			}
		}
	}
	public void SetTexActive(bool active)
	{
		foreach (Renderer itemRenders in EditRenders)
		{
			foreach (Material item in itemRenders.materials)
			{
				item.SetInt("UseTex", active ? 1 : 0);
				Debug.Assert(item.HasProperty("UseTex"), "UseTex 材质错误" + item.name, itemRenders.gameObject);
			}
		}
	}
	public void SetPureColor(Color texColor)
	{
		foreach (Renderer itemRenders in EditRenders)
		{
			foreach (Material item in itemRenders.materials)
			{
				item.SetColor("PureColor", texColor);
				Debug.Assert(item.HasProperty("PureColor"), "PureColor 材质错误" + item.name, itemRenders.gameObject);
			}
		}
	}

	public void SetErrorTex(bool active)
	{
		foreach (Renderer itemRenders in EditRenders)
		{
			foreach (Material item in itemRenders.materials)
			{
				item.SetInt("UseErrorTex", active ? 1 : 0);
				Debug.Assert(item.HasProperty("UseErrorTex"), "UseErrorTex 材质错误" + item.name, itemRenders.gameObject);
			}
		}
	}

	public void SetAlpha(float alpha)
	{
		//foreach (Renderer item in EditRenders)
		//{
		//	//LineRenderer lineRenderer = item.GetComponent<LineRenderer>();
		//	//item ("PureColor", texColor);
		//}
	}

	public bool OverlapOther(ICollisionCtrl other)
	{
		if(Layer != other.Layer) return false;
		foreach (var itemOtherCollider in other.GetColliders_CollisionTest(MyPartType))
		{
			if (!itemOtherCollider.enabled || !itemOtherCollider.gameObject.activeSelf) continue;
			foreach (var itemSelfCollider in GetColliders_CollisionTest(other.MyPartType))
			{
				if (!itemSelfCollider.enabled || !itemSelfCollider.gameObject.activeSelf) continue;
				if (itemSelfCollider == itemOtherCollider)
				{
					Debug.LogError("注意 你正在求两个相同的Collider2D的 Distance");
					return true;
				}
				var dis = itemOtherCollider.Distance(itemSelfCollider);
				if (dis.isOverlapped && dis.distance < -0.002f)
				{
					//Debug.Log($"{itemSelfCollider.gameObject.name} - {itemOtherCollider.gameObject.name} = {dis.distance}");
					return true;
				}
			}
		}
		return false;
	}

	public bool OverlapOthers(List<BasePartCtrl> other)
	{
		foreach (var item in other)
		{
			if(item == this || OverlapOther(item)) return true;
		}
		return false;
	}

	public void SwitchEditMaterials(Material m)
	{
		SwitchMaterials(m, EditRenders);
	}

	public void SwitchPhysicsMaterials(Material m)
	{
		SwitchMaterials(m, PhysicsRenders);
	}

	//public void IgnoreCollision(ICollisionCtrl other)
	//{
	//	foreach (var item in GetPhysicsColliders)
	//	{
	//		foreach (var itemOther in other.GetPhysicsColliders)
	//		{
	//			Physics2D.IgnoreCollision(item, itemOther);
	//		}
	//	}
	//}

	// ----------------//
	// --- 私有方法
	// ----------------//
	protected abstract void OnSettedCtrlData();

	private void SwitchMaterials(Material m, List<Renderer> renders)
	{
		foreach (var item in renders)
		{
			if (item is SpriteRenderer)
			{
				(item as SpriteRenderer).material = m;
			}
			else if (item is SpriteShapeRenderer)
			{
				(item as SpriteShapeRenderer).SetMaterials(new List<Material>() { m, m });
			}
			else
			{
				item.material = m;
			}
		}
	}
	// ----------------//
	// --- 类型
	// ----------------//
}
