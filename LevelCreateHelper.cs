using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LevelCreateHelper : MonoBehaviour
{
	// ------------- //
	// -- 序列化
	// ------------- //
	[ReadOnly]
	[SerializeField]
	[GUIColor("HasEditArea?Color.Green:Color.Red")]
	private bool HasEditArea;


	// ------------- //
	// -- 私有成员
	// ------------- //

	// ------------- //
	// -- 公有成员
	// ------------- //

	// ------------- //
	// -- Unity 消息
	// ------------- //
#if UNITY_EDITOR

	[Button("检测 重复的场景全局光")]
	private void Check()
	{
		var lights = new List<Light2D>(FindObjectsOfType<Light2D>());
		lights.RemoveAll(a => a.lightType != Light2D.LightType.Global);
		foreach (var item in lights)
		{
			Debug.Log("全局光组件", item.gameObject);
		}
	}

	//[Button("查找所有非光照材质物体")]
	//private void FindRenderWitohutLight()
	//{
	//	var renders = new List<Renderer>(FindObjectsOfType<Renderer>());
	//	renders.RemoveAll(rd => rd.material.li)
	//}
	[Button("加载SO")]
	private void LoadSOFile()
	{
		var loadedObject = Resources.LoadAll("DataFile/SingleConfig/");
		foreach (var item in loadedObject)
		{
			(item as UniqueConfigBase).InitInstance();
		}
	}
#endif
	// ------------- //
	// -- 公有方法
	// ------------- //

	// ------------- //
	// -- 私有方法
	// ------------- //

}
