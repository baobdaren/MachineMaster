using UnityEngine;

public class ModelScenePart: ResetAbleInstance<ModelScenePart>
{
	// ------------- //
	// -- 序列化
	// ------------- //

	// ------------- //
	// -- 私有成员
	// ------------- //
	private ScenePartCtrl _editingScenePart;
	// ------------- //
	// -- 公有成员
	// ------------- //
	public ScenePartCtrl EditingScenePart
	{
		set
		{ 
			_editingScenePart = value;
		}
		get { return _editingScenePart; }
	}

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
