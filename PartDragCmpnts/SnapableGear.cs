using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SnapableGear : SnapableBase
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
	public override SnapBaseShapeTypes SnapBaseShapeType => SnapBaseShapeTypes.Gear;

	// ----------------//
	// --- Unity消息
	// ----------------//

	// ----------------//
	// --- 公有方法
	// ----------------//

	// ----------------//
	// --- 私有方法
	// ----------------//
	protected override SnapVector SnapToGears(List<SnapableBase> targets, List<SnapableBase> snapedTargets)
	{
		SnapVector snapResult = SnapVector.Empty;
		List<SnapableBase> sortedGears = targets.FindAll(
			(SnapableBase item) => {
				return Mathf.Abs(Vector2.Distance(item.MovePosition, MovePosition) - item.GearMiddleCircle.radius - GearMiddleCircle.radius) < 0.02f;
			}
		);
		sortedGears.Sort((a, b) => { return Vector2.SqrMagnitude(a.MovePosition - MovePosition) > Vector2.SqrMagnitude(b.MovePosition - MovePosition)?1:-1; });
		snapedTargets.Clear();
		if (sortedGears.Count >= 2)
		{
			snapResult = SnapCircleAndCircle(GearMiddleCircle, sortedGears[0].GearMiddleCircle, sortedGears[1].GearMiddleCircle);
			snapedTargets.Add(sortedGears[0]);
			snapedTargets.Add(sortedGears[1]);
		}
		else if (sortedGears.Count == 1)
		{
			snapResult = SnapCircleAndCircle(GearMiddleCircle, sortedGears[0].GearMiddleCircle, null);
			snapedTargets.Add(sortedGears[0]);
		}
		return snapResult;
	}

	// ----------------//
	// --- 类型
	// ----------------//
}
