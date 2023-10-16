using System.Collections.Generic;
using UnityEngine;

public class NiceUIHelper : MonoBehaviour
{
	// ------------- //
	// -- 序列化
	// ------------- //
	[SerializeField]
	private List<Vector3> PosList;

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
	private void OnDrawGizmos()
	{
		if(PosList == null || PosList.Count == 0) return;
		foreach (var item in PosList)
		{
			Gizmos.DrawLine(GetComponent<RectTransform>().position, item);
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
