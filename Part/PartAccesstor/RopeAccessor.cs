using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeAccessor : AbsSectionPartAccesor
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

	// ----------------//
	// --- Unity消息
	// ----------------//

	// ----------------//
	// --- 公有方法
	// ----------------//

	// ----------------//
	// --- 私有方法
	// ----------------//
	protected override void OnBeforeInit()
	{
		while (SectionList.Count < (int)PartConfig.Instance.RopeConfig.SpecLower)
		{
			SectionList.Add(GameObject.Instantiate(_originSection, _originSection.transform.parent));
		}
		GameObject.DestroyImmediate(_originSection);
	}

	// ----------------//
	// --- 类型
	// ----------------//

}
