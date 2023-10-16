using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeFactory : AbsPlayerPartFactory
{
	// ----------------//
	// --- 序列化
	// ----------------//


	// ----------------//
	// --- 公有成员
	// ----------------//


	// ----------------//
	// --- 私有成员
	// ----------------//


	// ----------------//
	// --- Unity消息
	// ----------------//

	// ----------------//
	// --- 公有方法
	// ----------------//


	// ----------------//
	// --- 私有方法
	// ----------------//
	protected override void OnCreatedAsEdit(PlayerPartCtrl partCtrlData, AbsBasePlayerPartAccessor accesstor)
	{

	}


	protected override void OnCreatedAsPhysics(PlayerPartCtrl partCtrlData, AbsBasePlayerPartAccessor accesstor)
	{

	}


	protected override void OnSetSize(PlayerPartCtrl partCtrlData, AbsBasePlayerPartAccessor accesstor)
	{

	}


	protected override void ApplyPositionAndRotation(PlayerPartCtrl partCtrl, AbsBasePlayerPartAccessor accesstor)
	{

	}
	//private GameObject CreateSetcion()
	//{
	//	var cloneSectionRigid = GameObject.Instantiate(_originNode, _originNode.transform.parent).GetComponent<Rigidbody2D>();
	//	cloneSectionRigid.name = _originNode.name;
	//	cloneSectionRigid.gameObject.SetActive(false);
	//	PartConfig.Instance.RopeConfig.Apply(cloneSectionRigid, 0.1f);
	//	return cloneSectionRigid.gameObject;
	//}

	private void ConnectAllSection(RopeAccessor rope, int useAmount)
	{

	}

	private void PlaceSection(RopeAccessor accesstor, int useAmount)
	{
		//Assert.IsTrue(_sectionList.Count >= placeAmount, "放置数量必须小于总数");

	}
	// ----------------//
	// --- 类型
	// ----------------//
}
