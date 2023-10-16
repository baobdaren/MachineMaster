using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 零件的组件访问器
/// 基类简单实现了所有的访问属性，并给定了必须给出几个字段
/// 子类通过虚属性实现动态添加组件。
/// </summary>
public abstract class AbsBasePlayerPartAccessor : AbsBasePartAccessor
{
	// ----------------//
	// --- 序列化
	// ----------------//
	//[HideInInspector]
	//public PlayerPartCtrl MyPartCtrl { get =>  base.PartCtrl as PlayerPartCtrl;}



	[HideInInspector]
	public SnapableBase PartDragCmpnt 
	{ 
		get 
		{ 
			if (_dragCmpnt == null) _dragCmpnt = GetComponentInChildren<SnapableBase>(true); 
			return _dragCmpnt; 
		} 
	}
	private SnapableBase _dragCmpnt;

	[HideInInspector]
	public MouseDoubleClick DoubleClickCmpnt
	{
		get
		{
			if (_doubleClickCmpnt == null) _doubleClickCmpnt = GetComponentInChildren<MouseDoubleClick>(true);
			if(_doubleClickCmpnt == null) _doubleClickCmpnt = gameObject.AddComponent<MouseDoubleClick>();
			return _doubleClickCmpnt;
		}
	}
	private MouseDoubleClick _doubleClickCmpnt;
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


	// ----------------//
	// --- 类型
	// ----------------//
	public enum InitType
	{
		Edit, Simulate
	}
}
