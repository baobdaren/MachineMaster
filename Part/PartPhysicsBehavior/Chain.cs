using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Assertions;

public class Chain : AbsSectionPartBase
{
	// ---------- //
	// --- 私有属性
	// ---------- //

	// ---------- //
	// --- 公有属性
	// ---------- //
	public ChainAccessor MyAccesstor { get => Accessor as ChainAccessor; }


	//-------------- //
	// Unity消息
	//-------------- //
	//protected override void Start_Edit()
	//{
	//	base.Start_Edit();
	//	if (_originNode.GetComponent<Rigidbody2D>() == null)
	//	{
	//		_originNode.gameObject.AddComponent<Rigidbody2D>();
	//	}
	//	ConnectableColliders = new List<Collider2D>();
	//}



	// ---------- //
	// --- 私有方法
	// ---------- //


	// ---------- //
	// --- 公有方法
	// ---------- //

	//public override GameObject CreateSetcion()
	//{
	//	_originNode.gameObject.SetActive(false);
	//	var cloneSectionRigid = GameObject.Instantiate(_originNode, _originNode.transform.parent).GetComponent<Rigidbody2D>();
	//	cloneSectionRigid.name = _originNode.name;
	//	if (!cloneSectionRigid.TryGetComponent<MouseDragAbsort>(out MouseDragAbsort dragCmpnt))
	//	{
	//		dragCmpnt = cloneSectionRigid.gameObject.AddComponent<MouseDragAbsort>();
	//	}
	//	PartConfig.Instance.RailConfig.Apply(cloneSectionRigid);
	//	cloneSectionRigid.gameObject.SetActive(true);
	//	dragCmpnt.EnableAbsort = false;
	//	return cloneSectionRigid.gameObject;
	//}


	//public override void ConnectSection()
	//{
	//	int placeAmount = MyCtrlData.MainSizeIndex;
	//	Assert.IsTrue(_sectionList.Count > 1, $"多节零件节数至少为2 当前为 {_sectionList.Count}");
	//	for (int i = 1; i <= placeAmount; i++)
	//	{
	//		GameObject sectionA = _sectionList[i - 1].gameObject;
	//		GameObject sectionConnectTo = _sectionList[i == placeAmount ? 0 : i].gameObject;
	//		if (!sectionA.TryGetComponent<HingeJoint2D>(out HingeJoint2D disJoint))
	//		{
	//			disJoint = sectionA.AddComponent<HingeJoint2D>();
	//		}
	//		GameObject connectBody = sectionA.transform.GetChild(0).gameObject;
	//		connectBody.transform.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.right, (sectionConnectTo.transform.position - sectionA.transform.position)));
	//		// 线连接到自身的铰接体
	//		disJoint.connectedBody = connectBody.GetComponent<Rigidbody2D>();
	//		disJoint.autoConfigureConnectedAnchor = false;
	//		disJoint.anchor = Vector2.zero;
	//		//disJoint.autoConfigureConnectedAnchor = false;
	//		// 铰接体链接到目标
	//		if (!connectBody.TryGetComponent<HingeJoint2D>(out HingeJoint2D joint))
	//		{
	//			joint = connectBody.AddComponent<HingeJoint2D>();
	//		}
	//		joint.autoConfigureConnectedAnchor = true;
	//		joint.connectedBody = sectionConnectTo.GetComponent<Rigidbody2D>();
	//		joint.anchor = new Vector2(DISTANCE_SECTION, 0);
	//		joint.autoConfigureConnectedAnchor = false;
	//		joint.useMotor = false;
	//	}
	//}
	//public override void StartToPhysicsSumilate (PartCtrlData ctrlData)
	//{
 //       Destroy(GetComponent<SpriteRenderer>());
	//	base.StartToPhysicsSumilate(ctrlData);

	//	for (int i = 0; i < transform.childCount; i++)
	//	{
	//		PartConfig.Instance.RailConfig.Apply(transform.GetChild(i).GetComponent<Rigidbody2D>());
	//		PartConfig.Instance.RailConfig.Apply(transform.GetChild(i).transform.GetChild(0).GetComponent<Rigidbody2D>());
	//	}
 //   }

	//public override void PlaceSection()
	//{
	//	int placeAmount = MyCtrlData.MainSizeIndex;
	//	float placeRadius = (DISTANCE_SECTION / 2) / Mathf.Sin(360f / placeAmount / 2 * Mathf.Deg2Rad);
	//	for (int i = 0; i < _sectionList.Count; i++)
	//	{
	//		if (i >= placeAmount) // 多余的节点禁用
	//		{
	//			_sectionList[i].gameObject.SetActive(false);
	//			continue;
	//		}
	//		_sectionList[i].gameObject.SetActive(true);
	//		Vector2 relativePos = new Vector2(placeRadius * Mathf.Cos(i * (360f / placeAmount) * Mathf.Deg2Rad), placeRadius * Mathf.Sin(i * (360f / placeAmount) * Mathf.Deg2Rad));
	//		_sectionList[i].transform.position = (Vector2)transform.position + relativePos;
	//	}
	//}

}
