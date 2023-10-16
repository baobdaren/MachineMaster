using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;

/// <summary>
/// 可编辑区域
/// 包含了编辑条件处罚和编辑区域
/// </summary>
public class EditZone : SerializedMonoBehaviour
{
	// ----------------//
	// --- 序列化
	// ----------------//
	[SerializeField]
	private AbsTargetTrigger PlayerTrigger;
	[SerializeField]
	public PolygonCollider2D EditArea;

	// ----------------//
	// --- 公有成员
	// ----------------//
	public Collider2D SpwanCollider
	{
		get
		{
			var triggerBox = PlayerTrigger.GetComponentInChildren<Collider2D>();
			Debug.Assert(triggerBox);
			return triggerBox;
		}
	}

	public bool IsTargetStayIn
	{
		get => PlayerTrigger.TargetStayIn;
	}

	[HideInInspector]
	public UnityEvent OnPlayerEnter = new UnityEvent();
	[HideInInspector]
	public UnityEvent OnPlayerExit = new UnityEvent();

	/// <summary>
	/// 可连接的场景对象
	/// </summary>
	public List<ScenePartCtrl> AllSceneParts
	{
		get
		{
			if (_sceneParts == null)
			{
				var allParts = FindObjectsOfType<ScenePart>(true);
				_sceneParts = new List<ScenePartCtrl>(allParts.Length);
				foreach (ScenePart item in allParts)
				{
					_sceneParts.Add(item.MyCtrl); ;
				}
			}
			return _sceneParts;
		}
	}

	public List<Collider2D> OtherColliders { private set; get; }

	// ----------------//
	// --- 私有成员
	// ----------------//
	private List<ScenePartCtrl> _sceneParts;

	// ----------------//
	// --- Unity消息
	// ----------------//
	private void Awake()
	{
		Debug.Assert(PlayerTrigger != null, this.name);
		//Debug.Assert(AllSceneParts != null);
		OtherColliders = new List<Collider2D>();

		PlayerTrigger.OnPlayerEnter.AddListener(OpenDesignUI);
		PlayerTrigger.OnPlayerExit.AddListener(CloseDesignUI);

		CreateBoundLineRender();
	}

	private void Start()
	{
		
	}
	// ----------------//
	// --- 公有方法
	// ----------------//
	public bool HasOverBoundPoints(out Vector2[] result)
	{
		var castResult = new RaycastHit2D[2]; // 最多检测两个
		int castAmount = 0;
		for (int i = 1; i < EditArea.points.Length; i++)
		{
			castAmount = Physics2D.LinecastNonAlloc(EditArea.points[i - 1], EditArea.points[i - 1], castResult, ~GameLayerManager.EditPart);
			if (castAmount == 0)
			{
				continue;
			}
		}
		if (castAmount == 0)
		{ 
			result = null;
			return false;
		}
		result = new Vector2[castResult.Length];
		for (int i = 0; i < castResult.Length; i++)
		{
			result[i] = castResult[i].point;
		}
		return true;
	}

	public bool HasOverBoundPoints()
	{
		var castResult = new RaycastHit2D[2]; // 最多检测两个
		for (int i = 1; i < EditArea.points.Length; i++)
		{
			if (Physics2D.LinecastNonAlloc(EditArea.points[i - 1], EditArea.points[i - 1], castResult, ~GameLayerManager.EditPart) != 0)
			{
				return true;
			}
		}
		return false;
	}

	public IConnectableCtrl GetScenepart(Hash128 id)
	{
		foreach (ScenePartCtrl item in AllSceneParts)
		{
			if (item.GetID == id)
			{
				return item;
			}
		}
		Debug.Assert(false);
		return null;
	}
	// ----------------//
	// --- 私有方法
	// ----------------//
	private void OpenDesignUI(GameObject target)
	{
		OnPlayerEnter?.Invoke();
		//UIManager.Instance.Display();
		// 暂时定为：场景对象和零件不铰接
		//PartManager.Instance.AddExtraColliders(ConnectableSceneObjects);
	}

	private void CloseDesignUI(GameObject target)
	{
		OnPlayerExit?.Invoke();
		//UIManager.Instance.Hide();
		//PartManager.Instance.ClearExtraColliders();
	}

	private void CreateBoundLineRender()
	{
		Vector2 extents = EditArea.bounds.extents;
		Vector3[] corner =
		{
			EditArea.bounds.center + new Vector3(extents.x, extents.y, 0),
			EditArea.bounds.center + new Vector3(-extents.x, extents.y, 0),
			EditArea.bounds.center + new Vector3(-extents.x, -extents.y, 0),
			EditArea.bounds.center + new Vector3(extents.x, -extents.y, 0),
		};
		var boundLine = gameObject.AddComponent<LineRenderer>();
		boundLine.startWidth = .02f;
		boundLine.positionCount = 4;
		boundLine.textureMode = LineTextureMode.Tile;
		boundLine.loop = true;
		boundLine.material = MaterialConfig.Instance.EditAreaBox;
		boundLine.numCornerVertices = 6;
		boundLine.SetPositions(corner);
		//boundLine.sortingLayerID = RenderLayerManager.Instance.GetEditAreaIndex;
	}
	// ----------------//
	// --- 类型
	// ----------------//
}
