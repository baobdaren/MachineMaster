using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 零件的编辑视图
/// </summary>
public class MainViewPlayerPartEditor : BaseView
{
    [Header("删除和创建")]
    [SerializeField]
    private Button _buttonDesignSucced;
    [SerializeField]
    private Button _buttonDelete;
    [Header("显示底部详细设置面板")]
    [SerializeField]
    private Button _buttonDisplaySizeColorPanle;
    [SerializeField]
    private GameObject _panleSizeColor;
    [Header("左侧铰接面板")]
    [SerializeField]
    private Button _buttonDisplayConnectPanle;
	[SerializeField]
    private GameObject _panleConnect;
    [Header("颜色选择按钮-原始")]
    [SerializeField]
    private Button _colorSelectOriginBtn;
    [Header("建模按钮")]
    [SerializeField]
    private Button _buttonModeling;
    // --------------------
    // --- 公有成员
    // --------------------
    public ChildViewEdit_Connect ConnectView => GetComponentInChildren<ChildViewEdit_Connect>();

    // --------------------
    // --- 私有成员
    // --------------------
    private ControllerPlayerPartEditor Ctrl { get => ControllerPlayerPartEditor.Instance; }
    private CircleSlider RotateCicle
    {
        get
        {
            if (_rotateCicle == null)
            {
                _rotateCicle = Instantiate(GameConfig.Instance.PartRoateCircle).GetComponent<CircleSlider>();
                _rotateCicle.gameObject.SetActive(false);
            }
            return _rotateCicle;
        }
    }
    private CircleSlider _rotateCicle;
    private ModelPlayerPartEditor Model => ModelPlayerPartEditor.Instance;

    protected override CameraActor.CameraWorkStates? GetCameraWorkStateInThisView => CameraActor.CameraWorkStates.FreeMove;

	// 拖拽结束的标识，没有拖拽时NULL，拖拽时Fasle，当前帧没有拖拽但是Flag有值且False则位
	private bool? finishedDragFlag = null;
    // --------------------
    // --- Unity消息
    // --------------------

    protected override void Awake()
    {
        base.Awake();
        _buttonDelete.onClick.AddListener(OnClick_Delete);
        _buttonDesignSucced.onClick.AddListener(OnClick_EditSucced);
        _buttonDisplaySizeColorPanle.onClick.AddListener(OnClicked_DisplayBottomPanel);
        _buttonDisplayConnectPanle.onClick.AddListener(OnClicked_DisplayConnectPanel);
        RotateCicle.OnValueChanged += OnValueChanged_Rotation;
		_buttonModeling.onClick.AddListener(OnClicked_Modeling);
        //CreateColorSelectBtns();
        //_xInput.onEndEdit.AddListener(OnInputFiled_PositionX);
        //      _yInput.onEndEdit.AddListener(OnInputFiled_PositionY);

        //PartDragManager.Instance.OnStartDragPlayerPart.AddListener(delegate { Model.SetDirty(); });
        //PartDragManager.Instance.OnEndDragPlayerPart.AddListener(delegate { Model.SetDirty(); });
    }

	protected override void Start()
	{
        base.Start();
        ConnectCursor.Instance.Hide();
    }

    private void Update()
	{
		//if (Time.frameCount % 3 == 0)
		//{
		//	if (ModelEdit.Instance.GetConnectMain != null)
		//	{
  //              UpdateEditingUI(ModelEdit.Instance.EditingPlayerPartCtrl as PlayerPartCtrl);
		//	}
		//}
        //UpdatePartMaterials();
        //UpdateBearingColor();
	}

	//private void LateUpdate()
	//{
 //   }

	private void OnDrawGizmos()
	{
        Vector3 screenSize = GetComponent<RectTransform>().rect.size;
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(screenSize/2, screenSize);
	}

	// --------------------
	// --- 私有方法
	// --------------------
	private void OnValueChanged_Rotation(float value)
    {
        ModelPlayerPartEditor.Instance.EditingPlayerPartCtrl.Rotation = Quaternion.Euler(0, 0, value);
	}

    /// <summary>
    /// 设计完成
    /// </summary>
    private void OnClick_EditSucced()
    {
        //Ctrl.EditFinish_Succeed();
        Ctrl.EditFinish(true);
        UIManager.Instance.OpenView<MainViewCreate>(BaseOpenViewData.Empty);
    }
    /// <summary>
    /// 删除
    /// </summary>
    private void OnClick_Delete()
    {
        Ctrl.EditFinish(false);
        UIManager.Instance.OpenView<MainViewCreate>(BaseOpenViewData.Empty);
		if (PlayerPartManager.Instance.AllColliders.Find((a) => { return a == null; }))
		{
            Debug.LogError("删除零件导致空的碰撞器存在于列表");
        }
    }

    /// <summary>
    /// 设置尺寸等的底部界面
    /// </summary>
    private void OnClicked_DisplayBottomPanel()
    {
        _panleSizeColor.transform.DOLocalMove(-_panleSizeColor.transform.localPosition, 0.3f);
        _buttonDisplaySizeColorPanle.interactable = false;
        _buttonDisplaySizeColorPanle.transform.DORotate(-_buttonDisplaySizeColorPanle.transform.eulerAngles, 0.2f, RotateMode.Fast).onComplete +=
        () =>
        {
            _buttonDisplaySizeColorPanle.interactable = true;
		    if (_panleSizeColor.transform.localPosition.y > 0)
		    {
                RotateCicle.Display(ModelPlayerPartEditor.Instance.EditingPlayerPartCtrl);
		    }
			else
			{
                RotateCicle.Hide();
			}
        };


        //     RectTransform rt = btn.GetComponent<RectTransform>();
        //     RectTransform movePanel = btn.Obj.GetComponent<RectTransform>();
        //     btn.ReverseState();
        //     btn.enabled = false;
        //     bool isOpening = btn.State == ButtonState.ON;
        //     movePanel.DOAnchorPosY(movePanel.anchoredPosition.y * -1, .1f).onComplete += () => 
        //     {
        //         btn.enabled = true;
        //         btn.GetComponent<RectTransform>().DOLocalRotate(btn.GetComponent<RectTransform>().localRotation.eulerAngles * -1, .8f);
        //if (isOpening)
        //{
        //             RotateCicle.Display(ModelEdit.Instance.EditingPlayerPartCtrl);
        //}
        //else
        //{
        //             RotateCicle.Hide();
        //}
        //     };
    }

    /// <summary>
    /// 左侧交接面板显现
    /// </summary>
    /// <param name=""></param>
    private void OnClicked_DisplayConnectPanel()
    {
        _panleConnect.transform.DOLocalMove(-_panleConnect.transform.localPosition, 0.3f);
        _buttonDisplayConnectPanle.interactable = false;
        Vector3 rotateToAbgle = _buttonDisplayConnectPanle.transform.eulerAngles.z == 0 ? new Vector3(0, 0, 180) : Vector3.zero;
        _buttonDisplayConnectPanle.transform.DORotate(rotateToAbgle, 0.2f, RotateMode.FastBeyond360).onComplete +=
        () =>
        {
            _buttonDisplayConnectPanle.interactable = true;
        };
    }

    private void OnClicked_Modeling()
    {
        UIManager.Instance.OpenView<MainViewModeling>(new ViewModelingOpenData(Model.EditingPlayerPartCtrl), true);
    }

    /// <summary>
    /// 创建颜色选择列表
    /// 已删除的功能
    /// </summary>
    //private void CreateColorSelectBtns()
    //{
    //    int step = 2;
    //    Button[] cloneColorItems = new Button[360/step];
    //    cloneColorItems[0] = _colorSelectOriginBtn;
    //    for (int i = 1; i < cloneColorItems.Length; i++)
    //    {
    //        cloneColorItems[i] = Instantiate(_colorSelectOriginBtn, _colorSelectOriginBtn.transform.parent);
    //        cloneColorItems[i].name = i.ToString();
    //    }
    //    for (int i = 0; i < cloneColorItems.Length; i++)
    //    {
    //        int input = i*step;
    //        cloneColorItems[i].transform.GetChild(0).GetComponent<Graphic>().color = Color.HSVToRGB(input/360f, 1f, 1f);
    //        cloneColorItems[i].GetComponentInChildren<NiceButton>().onClick.AddListener(() => { Ctrl.SetMasterPartHueOffset(input); });
    //    }
    //}

    private void DisplayRotateCircle(bool display = true)
    {
		if (display)
		{
            _rotateCicle.transform.position = ModelPlayerPartEditor.Instance.EditingPlayerPartCtrl.MyEditPartAccesstor.transform.position;
		}
        _rotateCicle.gameObject.SetActive(display);
    }


 //   private void UpdateBearingColor(bool forceUpdate = false)
 //   {
 //       //if (DisplayingState == DisplayingStates.Closed) return;
 //       //if (forceUpdate == false && Model.DirtyFlag_BearingColor == false) return;
 //       //// 查找轴承
 //       ////if (Model.IsFindingBearing == false) Model.DirtyFlag_BearingColor = false;
 //       //PartConnectionManager.Instance.UpdateEditBearings(Model.GetConnectMain);
 //       //Model.DirtyFlag_BearingColor = false;
	//}

    //   private void UpdateBearingColor(bool forceUpdate = false)
    //   {
    //       if (forceUpdate == false && Model.DirtyFlag_BearingColor == false) return;
    //       // 查找轴承
    //       if(Model.IsFindingBearing == false) Model.DirtyFlag_BearingColor = false;

    //	foreach (var itemConn in PartConnectionManager.Instance.AllEditConnection)
    //	{
    //		if (IsDisplaying == false)// 处于编辑页面
    //		{
    //			itemConn.Bearing.SetDisplay(false, false);
    //		}
    //		else if (Model.GetConnectMain.OverlapPoint(itemConn.AnchorPosition)
    //			&& !itemConn.ConnectedParts.ContainsKey(Model.GetConnectMain)) //该轴承覆盖当前零件，而且没有连接到该零件
    //		{
    //			itemConn.Bearing.SetDisplay(true, true);
    //		}
    //		else
    //		{
    //			itemConn.Bearing.SetDisplay(true, false);
    //		}
    //	}
    //}
    // --------------------
    // --- 公有方法
    // --------------------
    /// <summary>
    /// 点击清除连接
    /// </summary>
    public void OnClick_ClearConnects()
    {
        Ctrl.DeleteMasterConnection();
    }

	protected override void OnAwakeView(BaseOpenViewData datas)
	{
        //UpdateBearingColor(true);
	}

	protected override void OnSleepView(BaseOpenViewData datas)
	{
		base.OnSleepView(datas);
	}

	protected override void OnEnteringView(BaseOpenViewData datas)
    {
        ViewEditOpenData selfOpenData = (ViewEditOpenData)datas;

        DisplayRotateCircle(false);
        ConnectCursor.Instance.Hide();
        PartConnectionManager.Instance.SetActiveEditBearings(true);
        UpdateRotationUI(selfOpenData.PlayerPartCtrl);
        _buttonModeling.interactable = /*Model.IsEditingPlayerPart && */selfOpenData.PlayerPartCtrl.IsProgrammablePart;

        TimeManager.Instance.IsEditing = true;
        Ctrl.SetEditPart(selfOpenData.PlayerPartCtrl);
	}

	protected override void OnExitingView(BaseOpenViewData datas)
	{
        ConnectCursor.Instance.Hide();
        DisplayRotateCircle(false);
        ConnectCursor.Instance.Hide();
        Debug.Assert(PlayerPartManager.Instance.AllPlayerPartCtrls.Contains(null) == false, "已创建列表中有一个对象为空！");
        PartConnectionManager.Instance.SetActiveEditBearings(true);
        Ctrl.EditFinish(true);
        Model.DirtyFlag_BearingColor = true;
        Model.IsFindingBearing = false;
    }
    /// <summary>
    /// 设置旋转信息的滑块和文本
    /// </summary>
    public void UpdateRotationUI(PlayerPartCtrl data)
    {
		if (data == null)
		{
            return;
		}

        RotateCicle.SetValue(data.Rotation.eulerAngles.z, false);
    }
}
