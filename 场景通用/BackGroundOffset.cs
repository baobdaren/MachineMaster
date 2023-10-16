using Sirenix.OdinInspector;
using UnityEngine;

public class BackGroundOffset: MonoBehaviour
{
	// ------------- //
	// -- 序列化
	// ------------- //
	[SerializeField]
	private float m_offsetRatio => Mathf.Abs(transform.position.z / 200f);

	// ------------- //
	// -- 私有成员
	// ------------- //
	private Vector3 m_originPos;
	/// <summary>
	/// 超过这个距离不再更新
	/// </summary>
	private float disUseEffect;
	// ------------- //
	// -- 公有成员
	// ------------- //

	// ------------- //
	// -- Unity 消息
	// ------------- //
	private void Start()
	{
		m_originPos = transform.position;
		Renderer sp = GetComponent<Renderer>();
		disUseEffect = sp.bounds.size.x / 2f;
		disUseEffect += CameraActor.OtherSizeLimit[1] * CameraActor.Instance.MainCamera.aspect;
		disUseEffect /= 1f - m_offsetRatio;
		sp.bounds = new Bounds(transform.position, new Vector3(sp.bounds.size.x + disUseEffect, sp.bounds.size.y, sp.bounds.size.z));
		//GetComponent<SpriteRenderer>().allowOcclusionWhenDynamic = false;
		//Debug.Log($"偏移检测距离为：{disUseEffect}");
	}


	//private void Update()
	//{
	//	float xDis = CameraActor.Instance.MainCamera.transform.position.x - m_originPos.x;
	//	if (Mathf.Abs(xDis) <= disUseEffect)
	//	{
	//		transform.position = new Vector3(m_originPos.x + xDis * m_offsetRatio, m_originPos.y, m_originPos.z);
	//	}
	//}

	// ------------- //
	// -- 公有方法
	// ------------- //

	// ------------- //
	// -- 私有方法
	// ------------- //
#if UNITY_EDITOR
	[Button("本物体自动材质")]
	private void AtuoSetMaterial()
	{
		GetComponent<Renderer>().material = Resources.Load("Render/Materials/MaterialBackgroundOffsetLit") as Material;
	}

	[Button("所有子物体自动材质")]
	private void AtuoSetMaterial_AllChild()
	{
		foreach (var item in GetComponentsInChildren<Renderer>())
		{
			item.material = Resources.Load("Render/Materials/MaterialBackgroundOffsetLit") as Material;
		}
	}
#endif

}
