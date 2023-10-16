using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailFactory : AbsPlayerPartFactory
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
		foreach (var item in accesstor.AllRigids)
		{
			PartConfig.Instance.PartRigidConfig.AppliyEditConfig(item);
		}
	}

	protected override void OnCreatedAsPhysics(PlayerPartCtrl partCtrlData, AbsBasePlayerPartAccessor accesstor)
	{
		foreach (var item in accesstor.AllRigids)
		{
			item.simulated = true;
			PartConfig.Instance.PartRigidConfig.ApplySimulateConfig(item);
		}
	}

	protected override void OnSetSize(PlayerPartCtrl partCtrlData, AbsBasePlayerPartAccessor accessor)
	{
		ChainAccessor railAccesstor = accessor as ChainAccessor;
		PlaceSection(railAccesstor, (int)partCtrlData.Size);
		ConnectAllSections(railAccesstor, (int)partCtrlData.Size);
	}

	private void ConnectAllSections(ChainAccessor accessor, int useAmount)
	{
		for (int i = 1; i <= useAmount; i++)
		{
			GameObject previousSection = accessor.SectionList[i - 1];
			GameObject currentSection = accessor.SectionList[i == useAmount ? 0 : i];
			Vector2 dir = currentSection.transform.position - previousSection.transform.position;
			previousSection.transform.rotation = Quaternion.Euler(0,0, Vector2.SignedAngle(Vector2.right, dir));
			if (!previousSection.TryGetComponent<HingeJoint2D>(out HingeJoint2D previousJoint))
			{
				previousJoint = previousSection.AddComponent<HingeJoint2D>(); 
			}
			previousJoint.connectedBody = currentSection.GetComponentInChildren<Rigidbody2D>(true);
			previousJoint.useMotor = false;
			previousJoint.anchor = 4 * accessor.ToothRadius * Vector2.right;

			//GameObject connectBody = previousSection.transform.GetChild(0).gameObject;
			////connectBody.transform.right = sectionConnectTo.transform.position - previewSection.transform.position;
			//previousSection.transform.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.right, (nextSection.transform.position - previousSection.transform.position)));
			//// 线连接到自身的铰接体
			//previousJoint.connectedBody = connectBody.GetComponentInChildren<Rigidbody2D>(true);
			//previousJoint.autoConfigureConnectedAnchor = true;
			//previousJoint.anchor = Vector2.zero;
			////disJoint.autoConfigureConnectedAnchor = false;
			//// 铰接体链接到目标
			//if (!connectBody.TryGetComponent<HingeJoint2D>(out HingeJoint2D joint))
			//{
			//	joint = connectBody.AddComponent<HingeJoint2D>();
			//}	
			//joint.autoConfigureConnectedAnchor = true;
			//joint.connectedBody = nextSection.GetComponent<Rigidbody2D>();
			//joint.anchor = new Vector2(accessor.RailRadius*4, 0);
			////joint.autoConfigureConnectedAnchor = false;
			//joint.useMotor = false;
		}
	}

	private void PlaceSection(ChainAccessor accessor, int useAmount)
	{
		float placeRadius = accessor.ToothRadius*2 / Mathf.Sin(360f / useAmount / 2 * Mathf.Deg2Rad);
		for (int i = 0; i < accessor.SectionList.Count; i++)
		{
			if (i >= useAmount) // 多余的节点禁用
			{
				accessor.SectionList[i].SetActive(false);
				continue;
			}
			accessor.SectionList[i].SetActive(true);
			Vector2 relativePos = new Vector2(placeRadius * Mathf.Cos(i * (360f / useAmount) * Mathf.Deg2Rad), placeRadius * Mathf.Sin(i * (360f / useAmount) * Mathf.Deg2Rad));
			accessor.SectionList[i].transform.position = (Vector2)accessor.transform.position + relativePos;
		}
	}
	// ----------------//
	// --- 类型
	// ----------------//

}
