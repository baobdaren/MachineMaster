using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

public class EnvPresser: MonoBehaviour
{
	// ------------- //
	// -- 序列化
	// ------------- //
	[SerializeField]
	private bool UpdateDirectorInEditor = false;
	[SerializeField]
	private Transform Body;
	[SerializeField]
	private Transform Prop;
	[ReadOnly]
	[SerializeField]
	private Transform PropSitter;

	[SerializeField]
	private GameObject PropSitterPrefab;

	[SerializeField]
	private bool IsHorizontal = false;
	// ------------- //
	// -- 私有成员
	// ------------- //

	// ------------- //
	// -- 公有成员
	// ------------- //

	// ------------- //
	// -- Unity 消息
	// ------------- //
	private void Update()
	{
		OnInspector_SetDirection();
	}

	// ------------- //
	// -- 公有方法
	// ------------- //
	/// <summary>
	/// 自动自身朝向
	/// </summary>
	public void SetDirectionSelf()
	{
		if (PropSitter == null)
		{
			//PropSitter = GameObject.Instantiate(PropSitterPrefab, transform).transform;
#if UNITY_EDITOR
			PropSitter = (UnityEditor.PrefabUtility.InstantiatePrefab(PropSitterPrefab, transform) as GameObject).transform;
#else
			PropSitter = (GameObject.Instantiate(PropSitterPrefab, transform) as GameObject).transform;
#endif
			PropSitter.GetComponent<EnvPresser_Helper>().EnvPresserBody = gameObject;
		}
		Prop.transform.position = PropSitter.transform.position;
		if(IsHorizontal)
		{
			Body.right = Prop.position - Body.position;
			Prop.right = Body.right;
		}
		else
		{
			Body.up = Prop.position - Body.position;
			Prop.up = Body.up;
		}
	}

	/// <summary>
	/// 外部调用设置全局自动朝向
	/// </summary>
	public void SetDirector()
	{
		var findResult = FindObjectsByType<EnvPresser>(FindObjectsSortMode.None);
		foreach (var item in findResult)
		{
			item.SetDirectionSelf();
		}
	}
	// ------------- //
	// -- 私有方法
	// ------------- //
	/// <summary>
	/// 编辑器设置全局自动朝向
	/// </summary>
	[Button("自动朝向")]
	private void OnInspector_SetDirection()
	{
		SetDirector();
	}

	[Button("完整删除")]
	private void Delete()
	{
#if UNITY_EDITOR
		UnityEditor.Undo.RecordObjects(new Object[] { gameObject, PropSitter }, "撤销完整删除" + typeof(EnvPresser));
#endif
		if (PropSitter == null)
		{
			Debug.LogWarning("基点似乎已删除");
		}
		else
		{
			GameObject.DestroyImmediate(PropSitter.gameObject);
		}
		GameObject.DestroyImmediate(gameObject);
	}
	private void OnValidate()
	{
		if (PropSitter == null) return;
		int BodySitterOrder = GetComponent<SpriteRenderer>().sortingOrder;
		int BodySitterLayer = GetComponent<SpriteRenderer>().sortingLayerID;
		PropSitter.GetComponent<SpriteRenderer>().sortingOrder = BodySitterOrder;
		PropSitter.GetComponent<SpriteRenderer>().sortingLayerID = BodySitterLayer;
		Body.GetComponent<SpriteRenderer>().sortingOrder = BodySitterOrder-1;
		Body.GetComponent<SpriteRenderer>().sortingLayerID = BodySitterLayer;
		Prop.GetComponent<SpriteRenderer>().sortingOrder = BodySitterOrder-2;
		Prop.GetComponent<SpriteRenderer>().sortingLayerID = BodySitterLayer;
	}

	private void Reset()
	{
		Body = transform.Find("Body");
		Prop = transform.Find("Prop");
		Debug.Assert(Body != null);
		Debug.Assert(Prop != null);
		OnValidate();
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.DrawLine(Body.position, Prop.position);
	}

	private void OnDrawGizmos()
	{
		if (UpdateDirectorInEditor)
		{
			SetDirectionSelf();
		}
	}
}
