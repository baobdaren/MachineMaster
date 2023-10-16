using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ChildViewEdit_Connect : BaseChildView
{
    [SerializeField]
    private Button _btnClearConnects;

	// 新的连接方案
	[Header("新的连接方案")]
    [SerializeField]
    private Button _btnConnect_Fixed; // 焊接
    [SerializeField]
    private Button _btnConnect_CreateBearing; // 创建轴承

    // -------------- //
    // -- 公有成员
    // -------------- //

    // -------------- //
    // -- 私有成员
    // -------------- //
    private ChildViewEdit_PartFilter _partSelectView;
    private ModelPlayerPartEditor Model => ModelPlayerPartEditor.Instance;
    private ControllerPlayerPartEditor Ctrl => ControllerPlayerPartEditor.Instance;

	// -------------- //
	// -- Unity消息
	// -------------- //
	private void Awake ( )
    {
        //_btnConnect_Fixed.onClick.AddListener(() => { OnClick_StartConnect(ConnectCursor.ConnectCursorStates.Fixed); });
        _btnConnect_CreateBearing.onClick.AddListener(OnClick_StartConnect);
        _btnClearConnects.onClick.AddListener(OnClick_ClearConnects);
        ConnectCursor.Instance.OnMoving.AddListener(OnDragUpdate_ConnectCursor);
        ConnectCursor.Instance.OnClick_NotClickedUI.AddListener(OnCursorClicked_PlaceConnection);
        _partSelectView = GetComponent<ChildViewEdit_PartFilter>(); // 保留零件筛选
    }

    // -------------- //
    // -- 公有方法
    // -------------- //
    public void ForceUpdate_ConnectCursor()
    {
        OnDragUpdate_ConnectCursor();
    }

	public override void OnParentViewEnter()
	{
		base.OnParentViewEnter();
        ConnectCursor.Instance.Hide();
	}
	// -------------- //
	// -- 私有方法
	// -------------- //
	/// <summary>
	/// 拖动铰接位置
	/// 区分焊接和轴承铰接
	/// </summary>
	private void OnDragUpdate_ConnectCursor ()
    {
        ConnectCursor.Instance.SetColor(-1);
        Vector2 anchor = ConnectCursor.Instance.transform.position;
        bool overlapedMainPart = Model.GetConnectMain.OverlapPoint(anchor);
        ConnectCursor.Instance.SetColor(overlapedMainPart ? 1 : -1);
        Model.SetConnectData(anchor); // 删除问题可能存在
        return;
    }
    /// <summary>
    /// 点击清除连接
    /// </summary>
    private void OnClick_ClearConnects ( )
    {
        Ctrl.DeleteMasterConnection();
    }

    private void OnClick_StartConnect()
    {
		if (Model.IsEditingSteel)
		{
            GameMessage.Instance.PrintMessageAtMousePos("正在编辑钢材");
            return;
		}
        Model.IsConnecting = true;
        ConnectCursor.Instance.Display();
        _partSelectView.SetDisplay(true);
        PartConnectionManager.Instance.SetActiveEditBearings(false);
    }

    /// <summary>
    /// 交接光标拖拽时鼠标点击
    /// </summary>
    /// <param name="fixedOrHinge"></param>
    private void OnCursorClicked_PlaceConnection ()
    {
        _partSelectView.SetDisplay(false);
        StringBuilder sb = new StringBuilder("铰接失败");
        bool createResult = false;
        IConnectableCtrl part1 = Model.EditingPlayerPartCtrl;
        //IConnectableCtrl part2 = Model.WillConnectTarget;
  //      if (isFixed)
  //      {
  //          createResult = PartConnectionManager.Instance.TryCreateFixedToBearing(part1, part2, Model.CurrentConnectCursorPos.Value, sb);
  //      }
		//else
		//{
		if (!Model.IsCursorOverlapedMainNow)
		{
            sb.Append("必须对准主零件");
            createResult = false;
		}
		else
		{
            //createResult = PartConnectionManager.Instance.TryCreateHingeConnection(Model.GetConnectMain, Model.CurrentConnectCursorPos.Value, sb);
            PartConnectionManager.Instance.CreateConection(Model.CurrentConnectCursorPos.Value);
            createResult = true;
		}
		//}

        if (createResult)
        {
            GameMessage.Instance.PrintMessageAtScreenCenter("设置成功");
        }
        else
		{
            GameMessage.Instance.PrintMessageAtScreenCenter(sb.ToString());
		}
        PartConnectionManager.Instance.SetActiveEditBearings(true);
        Model.ResetConnectState();
        Model.IsConnecting = false;
        PartConnectionManager.Instance.UpdateEditBearings(Model.EditingPlayerPartCtrl);
    }

    private bool TryGetConnectableTarget_PlayerPart(out IConnectableCtrl targetPlayerPartCtrl)
    {
        // 尝试连接玩家创建的零件-----------
        // 获取可以连接的所有碰撞器（该函数自动剔除自身的碰撞器，这意味着必然不能自己连接自己）
        PartConnectionManager.Instance.FindConnectableColliders(ConnectCursor.Instance.transform.position, ModelPlayerPartEditor.Instance.EditingPlayerPartCtrl, out List<Collider2D> castColliders);
        if (castColliders.Count == 0)
        {
            //Debug.Log($"没有对准其他 连接流程中断");
            targetPlayerPartCtrl = null;
            return false;
        }
        targetPlayerPartCtrl = null;
        // 从连接类型选择界面选定一种类型，选择第一个作为连接目标
        foreach (var item in castColliders)
        {
            if (!Model.ConnectablePartType.HasValue || Model.ConnectablePartType == PlayerPartManager.Instance.ColliderToPartCtrl[item].MyPartType)
            {
                targetPlayerPartCtrl = PlayerPartManager.Instance.ColliderToPartCtrl[item];
                break;
            }
        }
        return targetPlayerPartCtrl != null;
    }

	private bool TryGetConnectableTarget_ScenePart(out IConnectableCtrl scenePart)
	{
		List<ScenePartCtrl> sceneConnectableParts = LevelProgressBase.Instance.CurrentEditZone.AllSceneParts;
		Vector2 connectPos = ConnectCursor.Instance.transform.position;
        scenePart = null;
		foreach (IConnectableCtrl itemSceneParts in sceneConnectableParts)
		{
			if (itemSceneParts.OverlapPoint(connectPos))
			{
                scenePart = itemSceneParts;
                return true;
			}
		}
        return false;
	}
}

