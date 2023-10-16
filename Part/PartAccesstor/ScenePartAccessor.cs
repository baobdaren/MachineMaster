using UnityEngine;

public class ScenePartAccessor : AbsBasePartAccessor
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
	public MouseDoubleClick DoubleClickCmpnt { private get; set; }

	// ------------- //
	// -- Unity 消息
	// ------------- //
	//protected virtual void Start()
	//{
	//	if (DoubleClickCmpnt != null)
	//	{
	//		DoubleClickCmpnt.OnDoubleClick.AddListener(() => PartSelector.Instance.SelectClickedPart(PartCtrl));
	//	}
	//}
	// ------------- //
	// -- 公有方法
	// ------------- //

	// ------------- //
	// -- 私有方法
	// ------------- //
}
