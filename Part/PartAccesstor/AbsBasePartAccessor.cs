using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbsBasePartAccessor : SerializedMonoBehaviour, ILayerSettable
{
	// ------------- //
	// -- 序列化
	// ------------- //
	// 可连接刚体不可动态添加，这一位置，多节物体只能指定初始的值，或者不可连接
	public List<Collider2D> AllConectableColliders { get => _allConectableColliders; }
	/// <summary>
	/// 包含的渲染组件
	/// 渲染排序从前往后增加，后序显示在前面
	/// </summary>
	private List<Renderer> _allRenders;
	private List<Collider2D> _allColliders;
	private List<Rigidbody2D> _allRigids;
	[SerializeField]
	[ChildGameObjectsOnly]
	[Header("可焊接/铰接的碰撞器")]
	private List<Collider2D> _allConectableColliders;

	// ------------- //
	// -- 私有成员
	// ------------- //
	private bool Initted = false;
	// ------------- //
	// -- 公有成员
	// ------------- //
	//public BasePartCtrl PartCtrl { get; }
	public List<Renderer> AllRenders { get { InitAccessor(); return _allRenders; } }
	public List<Collider2D> AllColliders { get { InitAccessor(); return _allColliders; } }
	public List<Rigidbody2D> AllRigids { get { InitAccessor(); return _allRigids; } }
	public int SetOrder
	{
		set
		{
			int startIndex = value;
			foreach (var item in AllRenders)
			{
				item.sortingLayerID = SortingLayer.NameToID("Physics");
				item.sortingOrder = startIndex++;
			}
			Debug.Assert(startIndex - value < 100);
		}
	}
	// ------------- //
	// -- Unity 消息
	// ------------- //

	// ------------- //
	// -- 公有方法
	// ------------- //
	public bool GetConnectableCollider(Vector2 worldPos, out Collider2D result)
	{
		for (int i = 0; i < AllConectableColliders.Count; i++)
		{
			if (AllConectableColliders[i].OverlapPoint(worldPos))
			{
				result = AllConectableColliders[i];
				return true;
			}
		}
		result = null;
		return false;
		//throw new System.Exception($"{transform.parent.name} - {name} 没有覆盖这个连接点 位置={worldPos}");
	}



	public List<Collider2D> GetCollidersForConflictTest(PartTypes targetPartType)
	{
		InitAccessor();
		return GetCollidersForConflict(targetPartType);
	}
	// ------------- //
	// -- 私有方法
	// ------------- //
	/// <summary>
	/// 创建可能用到的所有子物体，以创建快速访问的属性
	/// </summary>
	protected virtual void OnBeforeInit() { }
	protected virtual List<Collider2D> GetCollidersForConflict(PartTypes targetPartType) { return _allColliders; }
	protected virtual List<Renderer> InitGetAllRenders()
	{
		return new List<Renderer>(GetComponentsInChildren<Renderer>(true));
	}
	/// <summary>
	/// 在从预制体复制后，立即调用以初始化一些快速访问的属性
	/// 时机必须尽早，否则可能导致后期添加的子物体被错误添加
	/// </summary>
	public virtual void InitAccessor()
	{
		if (Initted) return;
		OnBeforeInit();
		_allColliders = new List<Collider2D>(GetComponentsInChildren<Collider2D>(true));
		_allRenders = InitGetAllRenders();
		_allRigids = new List<Rigidbody2D>(GetComponentsInChildren<Rigidbody2D>(true));
		//Debug.Log($"初始化所{name} Accesstor 碰撞器{_allColliders.Count}个 渲染器{_allRenders.Count}个 刚体2D{_allRigids.Count}");

		//{
		//	//PartColorManager.Instance.SetMaterial_MainPart(partCtrl);
		//	//ControllerEdit.Instance.SetEditMainPart(partCtrl);
		//	UIManager.Instance.OpenView<MainViewEdit>(new ViewEditOpenData(partCtrl));
		//});
		//if (PartType.Steel == this.MyPartCtrl.MyPartType)
		//{
		//	Debug.Log("Steel 初始化后 有碰撞器" + _allColliders.Count);
		//}
		Initted = true;
	}
}
