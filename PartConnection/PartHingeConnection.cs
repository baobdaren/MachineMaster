using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 表示了一个轴承的连接存档数据
/// </summary>
public class PartHingeConnection : BasePartConnection
{
	//public PartConnection(IConnectableCtrl master, int masterPartColliderIndex, IConnectableCtrl target, int targetPartColliderIndex, bool fixedOrHinge, Vector2 anchorWorldPos)
	//{
	//	MasterPart = master;
	//	MasterPartConnectColliderIndex = masterPartColliderIndex;
	//	TargetPart = target;
	//	TargetPartConnectColliderIndex = targetPartColliderIndex;
	//	FixedOrHinge = fixedOrHinge;
	//	AnchorWorldPos = anchorWorldPos;
	//	EditBearing = GameObject.Instantiate<GameObject>(fixedOrHinge  ? PartConfig.Instance.EditBearing_Fixed : PartConfig.Instance.EditBearing_Hinge, EditingBearing.BearingParent_Edit.transform);
	//	EditBearing.transform.position = AnchorWorldPos;
	//}
	public PartHingeConnection(Vector3 bearingWorldPos) : base(bearingWorldPos)
	{
		EditBearing = GameObject.Instantiate<GameObject>(PartConfig.Instance.EditBearing_Hinge, ParentsManager.Instance.ParentOfEditBearing.transform).GetComponentInChildren<EditBearing>(true);
		EditBearing.transform.position = bearingWorldPos;
	}


	//public PartHingeConnection() { }

	//public IConnectableCtrl MasterPart;
	//public int MasterPartConnectColliderIndex;
	//public IConnectableCtrl TargetPart;
	//public int TargetPartConnectColliderIndex;
	//public Vector2 AnchorWorldPos;
	//public bool FixedOrHinge;

	// 此轴承的世界坐标
	//public Vector3 bearingWorldPosition;
}
