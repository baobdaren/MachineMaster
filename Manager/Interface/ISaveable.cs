using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using static ArchiveManager;

public interface ISaveable
{
	// ------------- //
	// -- 序列化
	// ------------- //

	// ------------- //
	// -- 私有成员
	// ------------- //

	// ------------- //
	// -- 公有成员
	// ------------- //

	// ------------- //
	// -- Unity 消息
	// ------------- //

	// ------------- //
	// -- 公有方法
	// ------------- //
	public abstract void LoadArchive(Archive archive);
	public abstract void SaveIntoArchive(Archive archive);
	//protected abstract void ResetData();
	// ------------- //
	// -- 私有方法
	// ------------- //

	// ------------- //
	// -- 类型
	// ------------- //
}

