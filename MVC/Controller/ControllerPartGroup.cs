using UnityEngine;
using UnityEngine.Events;

public class ControllerPartGroup
{
	// ------------- //
	// -- 序列化
	// ------------- //

	// ------------- //
	// -- 私有成员
	// ------------- //
	private ModelPartEditor Model => ModelPartEditor.Instance;
	private GlobalViewPartEditor View => GlobalViewPartEditor.Instance;

	public UnityEvent<int> OnSelectLayer = new UnityEvent<int>();
	//public UnityEvent<BasePartCtrl> OnStartEditPart = new UnityEvent<BasePartCtrl>();
	//public UnityEvent OnFinishiEditPart = new UnityEvent();

	// ------------- //
	// -- 公有成员
	// ------------- //
	public static readonly ControllerPartGroup Instance = new ControllerPartGroup();
	//public BasePartCtrl EditingPart
	//{
	//	get { return _editingPart; }
	//	set
	//	{
	//		_editingPart = value;
	//		PartConnectionManager.Instance.UpdateEditBearings(_editingPart);
	//		if (value != null)
	//		{
	//			CurrentLayer = _editingPart.Layer;
	//		}
	//	}
	//}
	// ------------- //
	// -- Unity 消息
	// ------------- //

	// ------------- //
	// -- 公有方法
	// ------------- //
	public void SetEditingPartLayer(int layer) 
	{
		if (PartManager.Instance.EditingPart == null) return;
		PartManager.Instance.EditingPart.Layer = layer;
	}
	// ------------- //
	// -- 私有方法
	// ------------- //
	private ControllerPartGroup() { }
}
