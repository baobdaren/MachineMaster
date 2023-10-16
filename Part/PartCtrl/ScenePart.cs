using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(MouseDoubleClick))]
public class ScenePart : SerializedMonoBehaviour
{
	// ----------------//
	// --- 序列化
	// ----------------//
	[Header("场景对象不一定可连接")]
	[SerializeField]
	private bool isConnectable;

	[SerializeField]
	[HideIf("@!isConnectable")]
	[Header("可交接对象")]
	[ListDrawerSettings(NumberOfItemsPerPage = 8)]
	private List<Collider2D> _connectableColliders;

	[ReadOnly]
	[SerializeField]
	[GUIColor("SelfIDIsNull")]
	private Hash128 _selfID;

	[SerializeField]
	private bool _isGear = false;
	[SerializeField]
	[HideIf("@!IsGear")]
	private CircleCollider2D _gearMiddleCircle;

	[SerializeField]
	[Range(0, 2)]
	[OnValueChanged("InitLayer")]
	public int _partLayer = 0;

	[SerializeField]
	private bool _enableDrag = false;
#if UNITY_EDITOR
	[Button("设置所有子物体材质")]
	[HideIf("@!isConnectable")]
	private void SetMAll()
	{
		foreach (var item in GetComponentsInChildren<SpriteRenderer>())
		{
			item.material = AssetDatabase.LoadAssetAtPath<Material>("Assets/Render/Part/PartMaterial_Lit.mat");
			Debug.Log(item.name + "被设置");
		}
	}
#endif
	// ----------------//
	// --- 公有成员
	// ----------------//
	// 和场景零件的冲突测试代码
	public bool IsConnectable => isConnectable;
	public bool IsGear => _isGear;
	public bool EnableDrag => _enableDrag;
	/// <summary>
	/// 用于确定存档中的场景零件是否这个 1000 0000 中不重复
	/// </summary>
	public Hash128 GetPartHashID { get => _selfID; }
	public List<Collider2D> ConnectableColliders => IsConnectable ? _connectableColliders : null;
	public CircleCollider2D GearMiddleCircle => _gearMiddleCircle;
	public int PartLayer { get => _partLayer; }

	public ScenePartCtrl MyCtrl
	{
		get
		{
			if (_myCtrl == null)
			{
				_myCtrl = new ScenePartCtrl(this);
			}
			return _myCtrl;
		}
		set { _myCtrl = value; }
	}
	public List<Collider2D> AllColliders 
	{
		get
		{
			if (_allColliders == null) _allColliders = new List<Collider2D>(GetComponentsInChildren<Collider2D>());
			return _allColliders;
		}
	}
	public List<Renderer> AllRenders
	{
		get
		{
			if (_allRenders == null)
			{
				_allRenders = new List<Renderer>(GetComponentsInChildren<SpriteRenderer>());
			}
			return _allRenders;
		}
	}
	public virtual ScenePartCtrlData GetInitCoreData
	{
		get
		{
			ScenePartCtrlData r = new ScenePartCtrlData
			{
				Layer = _partLayer,
				Position = transform.position,
				Rotation = transform.rotation,
				MyPartType = PartTypes.Custom
			};
			return r;
		}
	}
	public MouseDoubleClick DoubleClickCmpnt { get; private set; }
	public SnapableBase DragCmpnt { get; private set; }
	// ----------------//
	// --- 私有成员
	// ----------------//
	private ScenePartCtrl _myCtrl;
	private List<Collider2D> _allColliders;
	private List<Renderer> _allRenders;

	// ----------------//
	// --- Unity消息
	// ----------------//
	private void Awake()
	{
		DoubleClickCmpnt = GetComponentInChildren<MouseDoubleClick>(true);
		//CmpntDoubleClick.OnDoubleClick.AddListener(() =>
		//{
		//	//UIManager.Instance.OpenView<MainViewScenePart>(new ViewScenePartOpenData(MyCtrl));
		//	PartSelector.Instance.SelectClickedPart(MyCtrl);
		//});

		if (TryGetComponent(out SnapableBase snapCmpnt))
		{
			PartSnapManager.Instance.RegistSnapableTarget(snapCmpnt, MyCtrl);
		}

		DragCmpnt = GetComponentInChildren<SnapableBase>(true);
	}

	// ----------------//
	// --- 公有方法
	// ----------------//
	public void SetAsPhysics()
	{
		foreach (var item in GetComponentsInChildren<Transform>(true))
		{
			item.gameObject.layer = GameLayerManager.GetPartGameObjectLayer(_partLayer);
		}
	}

	// ----------------//
	// --- 私有方法
	// ----------------//
	private void InitLayer()
	{
		foreach (var item in GetComponentsInChildren<Transform>(true))
		{
			item.gameObject.layer = _partLayer;
		}
	}

#if UNITY_EDITOR
	[EnableIf("HashIsNull")]
	[Button("设置随机哈希值")]
	private void EditorSetHashID()
	{
		_selfID = GetRandomHash();
	}

	[Button("添加所有碰撞器")]
	private void AddCollidersToConnectableList()
	{
		_connectableColliders = new List<Collider2D>(GetComponentsInChildren<Collider2D>(true));
	}

	private Color SelfIDIsNull()
	{ return _selfID.isValid == true ? Color.green : Color.red; }

	[Button("重置")]
	private void ResetHashID()
	{
		_selfID = new Hash128();
	}

	[Button("查看String格式的值")]
	private void DisplayStringValue()
	{
		Debug.Log(_selfID.ToString());
	}

	private Hash128 GetRandomHash()
	{
		Hash128 hash = new Hash128();
		for (int i = 0; i < 10; i++)
		{
			hash.Append(UnityEngine.Random.Range(float.MinValue, float.MaxValue));
		}
		return hash;
	}

	private bool HashIsNull()
	{ return !_selfID.isValid; }

	private bool IsConnectablePart()
	{ return isConnectable; }

	[Button("修改层级")]
	private void SetScenePartLayer()
	{
		foreach (var item in GetComponentsInChildren<Transform>())
		{
			if (item.gameObject.layer != LayerMask.NameToLayer("ScenePart"))
			{
				item.gameObject.layer = LayerMask.NameToLayer("ScenePart");
				Debug.Log("修改为ScenePart Layer", item.gameObject);
			}
		}
	}
#endif

	// ----------------//
	// --- 类型
	// ----------------//
	[Serializable]
	public struct PartTypeConflictData
	{
		public PartTypes PartType;
		public List<Collider2D> colliders;
	}
}
