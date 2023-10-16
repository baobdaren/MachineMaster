using UnityEngine;
using UnityEngine.Events;

public class ControllerScenePartEditor
{
	// ------------- //
	// -- 序列化
	// ------------- //
	public	static ControllerScenePartEditor Instance { get; private set; } = new ControllerScenePartEditor();

	// ------------- //
	// -- 私有成员
	// ------------- //
	private ModelScenePart Model => ModelScenePart.Instance;

	// ------------- //
	// -- 公有成员
	// ------------- //
	public UnityEvent<ScenePartCtrl> OnStartEditScenePart = new UnityEvent<ScenePartCtrl>();
	public UnityEvent OnFinishEditScenePart = new UnityEvent();

	// ------------- //
	// -- Unity 消息
	// ------------- //

	// ------------- //
	// -- 公有方法
	// ------------- //
	public void SetEditPart(ScenePartCtrl scenePartCtrl)
	{
		//if (Model.EditingScenePart != null)
		//{
		//	PartMaterialSetter.Instance.SetMaterial_AsNormal(Model.EditingScenePart);
		//}
		Model.EditingScenePart = scenePartCtrl;
		OnStartEditScenePart?.Invoke(Model.EditingScenePart);
		//PartMaterialSetter.Instance.SetMaterial_EditMainPart(Model.EditingScenePart);
		PartMaterialSetter.Instance.UpdatePartMaterials();
	}

	public	void SetFinishEdit()
	{
		//if(Model.EditingScenePart != null) 
		//{
		//	PartMaterialSetter.Instance.SetMaterial_AsNormal(Model.EditingScenePart);
		//}
		PartMaterialSetter.Instance.UpdatePartMaterials();

		Model.EditingScenePart = null;
		OnFinishEditScenePart?.Invoke();
	}

	// ------------- //
	// -- 私有方法
	// ------------- //

}
