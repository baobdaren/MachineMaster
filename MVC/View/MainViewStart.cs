using Sirenix.OdinInspector;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 关卡主界面，默认视图
/// </summary>
public class MainViewStart : BaseView
{
    // --------------------
    // --- 序列化
    // --------------------
    [ChildGameObjectsOnly][SerializeField]
    Button _buttonExit;
    [ChildGameObjectsOnly]
    [SerializeField]
    Button _buttonDesign;
    [SerializeField]
    [ChildGameObjectsOnly]
    Button _buttonSimulate;
    [SerializeField]
    [ChildGameObjectsOnly]
    Button _buttonReplay;

    protected override CameraActor.CameraWorkStates? GetCameraWorkStateInThisView => CameraActor.CameraWorkStates.Follow;

	// --------------------
	// --- Unity消息
	// --------------------

	protected override void Awake()
    {
        base.Awake();
        _buttonDesign.onClick.AddListener(OnClick_BeginEdit);
        _buttonSimulate.onClick.AddListener(OnClick_Simulate);
        _buttonReplay.onClick.AddListener(OnClick_RetryLevel);
        //SetButtonsDisplay(false);
    }

	protected override void Start()
	{
        base.Start();
        //LevelProgressBase.Instance.OnEnableEdit.AddListener(()=>SetButtonsDisplay(true));
        //LevelProgressBase.Instance.OnDisableEdit.AddListener(()=> SetButtonsDisplay(false));
	}

	// --------------------
	// --- 私有方法
	// --------------------
	protected override void OnEnteringView(BaseOpenViewData datas)
	{
        //LayerWorker.Instance.Wrok(true); // 第一次载入场景时零件的实例初始化时机在此之后
        TimeManager.Instance.IsEditing = true;
		ControllerStart.Instance.SetCameraFreeMove();
        PartMaterialSetter.Instance.UpdatePartMaterials();
	}

	protected override void OnExitingView(BaseOpenViewData datas)
	{
    }

    //private void SetButtonsDisplay(bool designable)
    //{
    //    _buttonDesign.gameObject.SetActive(designable);
    //    _buttonSimulate.gameObject.SetActive(designable);
    //}

    private void OnClick_BeginEdit()
    {
        UIManager.Instance.OpenView<MainViewCreate>(BaseOpenViewData.Empty);
    }

    private void OnClick_Simulate()
    {
        int conflictCount = PartManager.Instance.GetConflictData().Count;
        if (conflictCount == 0) 
        {
			UIManager.Instance.OpenView<MainViewSimulate>(BaseOpenViewData.Empty);
		}
        else 
        {
            GameMessage.Instance.PrintMessageAtScreenCenter("零件存在冲突，无法开始模拟");
        }
    }

    private void OnClick_RetryLevel()
    {
        GameManager.Instance.ReplayCurrentLevel(true);
	}
    // --------------------
    // --- 公共方法
    // --------------------
}
