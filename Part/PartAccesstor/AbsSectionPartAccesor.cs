using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 多段物体
/// 多段物体需要存储每一段的数据
/// </summary>
public class AbsSectionPartAccesor : AbsBasePlayerPartAccessor
{
	// ----------------//
	// --- 序列化
	// ----------------//


	// ----------------//
	// --- 公有成员
	// ----------------//
	[HideInInspector]
	public List<GameObject> SectionList;
	// ----------------//
	// --- 私有成员
	// ----------------//
	[SerializeField]
	protected GameObject _originSection;



	// ----------------//
	// --- Unity消息
	// ----------------//

	// ----------------//
	// --- 公有方法
	// ----------------//

	// ----------------//
	// --- 私有方法
	// ----------------//

	// ----------------//
	// --- 类型
	// ----------------//

}
