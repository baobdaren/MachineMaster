using System.Collections.Generic;
using UnityEngine;

public class Gear : PlayerPartBase
{
	// ---------------- //
	// --- 序列化
	// ---------------- //

	// ---------------- //
	// --- 私有成员
	// ---------------- //

	// ---------------- //
	// --- 公有成员
	// ---------------- //
	public GearAccessor MyAccesstor { get => Accessor as GearAccessor; }

	///// <summary>
	///// 吃头数量，齿轮依赖齿头数量
	///// 注意：履带依赖齿头数为10的选项，此时齿中圆半径为2.546479f，设履带齿头铰接点距离齿中圆为r
	///// 则履带铰接点偏移为 = 4*0.4/2.546*r/2
	///// </summary>
	//public override int SizeCount => PartConfig.Instance.GearConfg.Length;
	//public override PartType GetPartType => PartType.Gear;


	// ---------------- //
	// --- Unity消息
	// ---------------- //
	//protected override void Start_Edit()
	//{
	//	base.Start_Edit();
	//	originGearHead.gameObject.SetActive(false);
	//	ConnectableColliders = new List<Collider2D>(gearBody.GetComponents<Collider2D>());
	//	GetComponent<MouseDragAbsort>().EnableAbsort = true;
	//}


	// ---------------- //
	// --- 私有方法
	// ---------------- //


	// ---------------- //
	// -- 公有方法
	// ---------------- //
}
