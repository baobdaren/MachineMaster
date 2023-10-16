using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SnapablePoint : SnapableBase
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
	public override SnapBaseShapeTypes SnapBaseShapeType => SnapBaseShapeTypes.Point;

	// ----------------//
	// --- Unity消息
	// ----------------//

	// ----------------//
	// --- 公有方法
	// ----------------//

	// ----------------//
	// --- 私有方法
	// ----------------//

	// ----------------//
	// --- 类型
	// ----------------//

	//protected override SnapVector SnapToBoxs(List<SnapableBase> targets, List<SnapableBase> snaped)
	//{
	//	throw new System.NotImplementedException();
	//}

	//protected override SnapVector SnapToCircles(List<SnapableBase> targets, List<SnapableBase> snaped)
	//{
	//	throw new System.NotImplementedException();
	//	Debug.Log
	//	//return SnapPointAndCircle(this.Point.transform.position, target.CircleBound);
	//}

	//protected override SnapVector SnapToPoints(List<SnapableBase> targets, List<SnapableBase> snaped)
	//{
	//	float? minSqrDis = null;
	//	SnapableBase minDisSnapOBjects = null;
	//	foreach (var item in targets)
	//	{
	//		if (item.EnableSnapBound)
	//		{
	//			continue;
	//		}
	//		float itemSqrDis = Vector2.SqrMagnitude(item.MovePosition - MovePosition);
	//		if (minSqrDis == null || itemSqrDis < minSqrDis)
	//		{
	//			minSqrDis = itemSqrDis;
	//			minDisSnapOBjects = item;
	//		}
	//	}
	//	snaped.Add(minDisSnapOBjects);
	//	return new SnapVector(minDisSnapOBjects.MovePosition - MovePosition);
	//}
}
