using UnityEngine;


/// <summary>
/// 零件的创建视图
/// </summary>
public class MainViewCreate : BaseView
{
    // ----------------- //
    // -- 序列化
    // ----------------- //
    [SerializeField]
    private NiceButton CancleButton;

	protected override CameraActor.CameraWorkStates? GetCameraWorkStateInThisView => CameraActor.CameraWorkStates.FreeMove;

	// ----------------- //
	// -- Unity 消息
	// ----------------- //
	protected override void Awake()
	{
		base.Awake();
		CancleButton.OnLeftClick.AddListener(OnClicked_Cancle);
	}
	// ----------------- //
	// -- 公有成员
	// ----------------- //

	// ----------------- //
	// -- 公有方法
	// ----------------- //
	protected override void OnEnteringView(BaseOpenViewData openData)
	{
		
	}

	protected override void OnExitingView(BaseOpenViewData openData)
	{
	}

	/// <summary>
	/// 创建零件名称，在Inspector调用
	/// </summary>
	/// <param name="prefabName"></param>
	public void OnClick_CreatePart(PartTypes partType)
    {
        PlayerPartCtrl ctrlData = PlayerPartSuperFactory.CreatePlayerEditPart(partType);
        ctrlData.MyEditPartAccesstor.PartDragCmpnt.ForceDrag(true, false);
        //ControllerEdit.Instance.SetEditMainPart(ctrlData);
		UIManager.Instance.OpenView<MainViewPlayerPartEditor>(new ViewEditOpenData(ctrlData), false) ;
		PartMaterialSetter.Instance.UpdatePartMaterials();
    }
    // ----------------- //
    // -- 私有方法
    // ----------------- //
    private void OnClicked_Cancle()
    {
        UIManager.Instance.OpenView<MainViewStart>(BaseOpenViewData.Empty);
    }
}