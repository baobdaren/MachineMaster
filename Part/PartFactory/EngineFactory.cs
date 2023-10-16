using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineFactory : AbsPlayerPartFactory
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
		accesstor.PartDragCmpnt.EnableSnapBound = true;
	}

	protected override void OnCreatedAsPhysics(PlayerPartCtrl partCtrlData, AbsBasePlayerPartAccessor accesstor)
	{
		foreach (var item in accesstor.AllRigids)
		{
			PartConfig.Instance.PartRigidConfig.ApplySimulateConfig(item);
		}
	}

	protected override void OnSetSize(PlayerPartCtrl partCtrlData, AbsBasePlayerPartAccessor accesstor)
	{
		//const int wantAmount = 10;
		//if (!accesstor.GearHeads.Contains(accesstor.OriginGearHead))
		//{
		//	accesstor.GearHeads.Add(accesstor.OriginGearHead);
		//}
		//while (wantAmount > accesstor.GearHeads.Count)
		//{
		//	accesstor.GearHeads.Add(GameObject.Instantiate(accesstor.OriginGearHead, accesstor.OriginGearHead.transform.parent));
		//}
		//float placeRadius = (PartConfig.DISTANCE_CIRLESECTION / 2) / Mathf.Sin(360 / wantAmount / 2 * Mathf.Deg2Rad);
		//for (int i = 0; i < accesstor.GearHeads.Count; i++)
		//{
		//	if (i >= wantAmount) // 多余的节点禁用
		//	{
		//		accesstor.GearHeads[i].SetActive(false);
		//		continue;
		//	}
		//	accesstor.GearHeads[i].SetActive(true);
		//	float angle = i * (360f / wantAmount);
		//	Vector2 relativePos = new Vector2(placeRadius * Mathf.Cos(angle * Mathf.Deg2Rad), placeRadius * Mathf.Sin(angle * Mathf.Deg2Rad));
		//	accesstor.GearHeads[i].transform.localPosition = relativePos;
		//	accesstor.GearHeads[i].transform.localEulerAngles = new Vector3(0, 0, angle - 90f);
		//}
		//float n = accesstor.EngineBody.GetComponent<SpriteRenderer>().sprite.rect.width / accesstor.EngineBody.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit;
		//float scale = 2 * (placeRadius - PartConfig.DISTANCE_CIRLESECTION / 4) / n;
		//accesstor.EngineBody.transform.localScale = new Vector3(scale, scale, 1);
		////accesstor.GetComponent<CircleCollider2D>().radius = accesstor.EngineBody.GetComponent<CircleCollider2D>().radius * scale;
	}
	// ----------------//
	// --- 类型
	// ----------------//
}
