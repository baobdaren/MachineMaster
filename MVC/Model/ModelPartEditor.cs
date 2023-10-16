using System;
using UnityEngine;

public class ModelPartEditor: ResetAbleInstance<ModelPartEditor>
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
	public BasePartCtrl EditingPart 
	{ 
		get
		{
			return ModelPlayerPartEditor.Instance.EditingPlayerPartCtrl ?? (BasePartCtrl)ModelScenePart.Instance.EditingScenePart;
		}
	}

	public bool IsEditing = false;
	// ------------- //
	// -- Unity 消息
	// ------------- //

	// ------------- //
	// -- 公有方法
	// ------------- //

	// ------------- //
	// -- 私有方法
	// ------------- //

}
