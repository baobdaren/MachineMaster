using Sirenix.OdinInspector;
using UnityEngine;
/// <summary>
/// 抽象基类
/// 零件在物理模拟状态下的控制逻辑
/// </summary>
public abstract class PlayerPartBase:SerializedMonoBehaviour
{
    // ----------------- //
    // --  序列化
    // ----------------- //

    // ----------------- //
    // --  私有成员
    // ----------------- //
    public AbsBasePlayerPartAccessor Accessor { set; get; }

    // ----------------- //
    // --  公有成员
    // ----------------- //
    public PlayerPartCtrl MyCtrlData { set; get; } 

    public int GetLayer
    {
        get { return MyCtrlData.Layer; }
    }

    public Rigidbody2D[] AllActivedRigids 
    {
        get
        {
            return GetComponentsInChildren<Rigidbody2D>();
        }
    }

    public Rigidbody2D[] AllRigids
	{
		get
		{
			return GetComponentsInChildren<Rigidbody2D>(true);
		}
	}

	// ----------- //
	// -- Unity消息
	// ----------- //

	// ----------- //
	// -- 私有方法
	// ----------- //
	//private GameObject CreateScrewGameObject(Vector3 worldAnchor)
	//{
	//    GameObject screw = GameObject.Instantiate(PartConfig.Instance.PhysicsScrewrefab);
	//    if (screw)
	//    {
	//        screw.transform.SetParent(ObjParentsManager.Instance.ParentOfCreatedPart.transform);
	//        screw.transform.position = worldAnchor;
	//        screw.SetActive(false);
	//    }
	//    return screw;
	//}


	// ----------- //
	// -- 公有方法 
	// ----------- //
	public void SetAsPhysicsLayer()
	{
		foreach (var item in GetComponentsInChildren<Transform>(true))
		{
			item.gameObject.layer = GameLayerManager.GetPartGameObjectLayer(GetLayer);
            Debug.LogError("设置玩家物理零件层级" + item.gameObject.layer);
		}
	}

	// ----------- //
	// -- 类型
	// ----------- //
	//public struct PartConnectionObject
	//{
	//    public PartConnectionObject(GameObject screwObject, PartBase jointAttatch, PartBase jointConnect)
	//    {
	//        ScrewObject = screwObject;
	//        ConnectBaseJointAttacthed = jointAttatch;
	//        ConnectBaseJointConnectTo = jointConnect;
	//    }
	//    public GameObject ScrewObject;
	//    public PartBase ConnectBaseJointAttacthed;
	//    public PartBase ConnectBaseJointConnectTo;
	//}
}

public enum PartWorkState
{
    /// <summary>
    /// Main界面时正常显示状态
    /// </summary>
    Normal, // 基本状态
    /// <summary>
    /// 编辑状态
    /// </summary>
    TmpEditMaster,
    /// <summary>
    /// 链接时作为主物体状态
    /// </summary>
    TmpConnectingMaster,
    /// <summary>
    /// 铰接时可以作为铰接对象的状态
    /// </summary>
    TmpConnecting_SelectedTarget,
    /// <summary>
    /// 铰接时尚未作为铰接对象的状态
    /// </summary>
    TmpConnectingTarget_Connectable, 
    /// <summary>
    /// 铰接时被过滤的零件状态
    /// </summary>
    TmpConnectingTarget_UnConnectable,
    /// <summary>
    /// 弹出，即当前状态弹出
    /// </summary>
    Cancle,
    /// <summary>
    /// 空，特殊用途
    /// </summary>
    None // 用于不切换时传递
}   