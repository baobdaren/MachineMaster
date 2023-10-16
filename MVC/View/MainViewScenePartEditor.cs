using UnityEngine;
using UnityEngine.Events;

public class MainViewScenePartEditor: BaseView
{
	// ------------- //
	// -- 序列化
	// ------------- //
	[SerializeField]
	private NiceButton ExitButton;

	// ------------- //
	// -- 私有成员
	// ------------- //
	private ControllerScenePartEditor Ctrl => ControllerScenePartEditor.Instance;
	protected override CameraActor.CameraWorkStates? GetCameraWorkStateInThisView => CameraActor.CameraWorkStates.FreeMove;

	// ------------- //
	// -- 公有成员
	// ------------- //

	// ------------- //
	// -- Unity 消息
	// ------------- //
	protected override void Awake()
	{
		base.Awake();
		ExitButton.OnLeftClick.AddListener(OnClicked_ExitView);
	}

	//private void Update()
	//{
	//	if (DisplayingState != DisplayingStates.Closed && Model.IsDirty)
	//	{
	//		PartConnectionManager.Instance.UpdateEditBearings(Model.EditingScenePart);
	//		Model.IsDirty = false;
	//	}
	//}
	// ------------- //
	// -- 公有方法
	// ------------- //
	protected override void OnEnteringView(BaseOpenViewData datas)
	{
		ScenePartCtrl myEditPart = (datas as ViewScenePartOpenData).ScenePartCtrl;
		Ctrl.SetEditPart(myEditPart);
		//CameraActor.Instance.SetCameraDisplay(CameraActor.CameraCullingTypes.ToEditPart);
	}

	protected override void OnExitingView(BaseOpenViewData datas)
	{
		Ctrl.SetFinishEdit();
		//CameraActor.Instance.SetCameraDisplay(CameraActor.CameraCullingTypes.All);
	}

	// ------------- //
	// -- 私有方法
	// ------------- //
	private void OnClicked_ExitView()
	{
		UIManager.Instance.OpenView<MainViewStart>(BaseOpenViewData.Empty);
	}


}
