using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : SerializedMonoBehaviour
{
	// ----------------//
	// --- 序列化
	// ----------------//

	// ----------------//
	// --- 公有成员
	// ----------------//

	// ----------------//
	// --- 私有成员
	// ----------------//
	private RenderTexture rt;

	// ----------------//
	// --- Unity消息
	// ----------------//
	private void Awake()
	{
		//GetComponentInChildren<Camera>(true).enabled = false;
		Camera renderCamera = GetComponentInChildren<Camera>(true);
		SpriteRenderer sp = GetComponent<SpriteRenderer>();
		rt = new RenderTexture(Mathf.Max((int)(sp.bounds.size.x), 1), Mathf.Max((int)(sp.bounds.size.y)*2, 1), 0);
		renderCamera.orthographicSize = rt.height / 2f;
		sp.bounds = new Bounds(transform.position, new Vector3(rt.width, rt.height/2f));
		renderCamera.targetTexture = rt;
		renderCamera.transform.position = transform.position + Vector3.up * rt.height / 4;
		sp.material.SetTexture("_target", rt);
		rt.name = "临时渲染目标纹理：" + rt.GetHashCode();
	}

	//private void OnBecameVisible()
	//{
	//	//Debug.Log(name + "可见");
	//	GetComponentInChildren<Camera>(true).enabled = true;
	//	GetComponentInChildren<SpriteRenderer>(true).enabled = true;
	//}

	//private void OnBecameInvisible()
	//{
	//	//Debug.Log(name + "不可见");
	//	GetComponentInChildren<Camera>(true).enabled = false;
	//	GetComponentInChildren<SpriteRenderer>(true).enabled = false;
	//}

	private void OnDestroy()
	{
		//if (rt)
		//{
		//	RenderTexture.ReleaseTemporary(rt);
		//}
	}
	// ----------------//
	// --- 公有方法
	// ----------------//


	// ----------------//
	// --- 私有方法
	// ----------------//
//#if UNITY_EDITOR
//	[Button("展示")]
//	private void SetRt()
//	{
//		Camera renderCamera = GetComponentInChildren<Camera>(true);
//		SpriteRenderer sp = GetComponent<SpriteRenderer>();
//		rt = renderCamera.targetTexture;
//		rt.width = Mathf.Max((int)(sp.bounds.size.x), 1);
//		rt.height = Mathf.Max((int)(sp.bounds.size.y) * 2, 1);
//		renderCamera.orthographicSize = rt.height / 2f;
//		sp.bounds = new Bounds(transform.position, new Vector3(rt.width, rt.height / 2f));
//		renderCamera.targetTexture = rt;
//		renderCamera.transform.position = transform.position + Vector3.up * rt.height / 4;
//		sp.sharedMaterial.SetTexture("_target", rt);
//		rt.name = "临时渲染目标纹理：" + rt.GetHashCode();
//	}
//#endif
}
