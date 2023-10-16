using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SnapableCircle : SnapableBase
{
	// ----------------//
	// --- 序列化
	// ----------------//

	// ----------------//
	// --- 公有成员
	// ----------------//
	public override SnapBaseShapeTypes SnapBaseShapeType => SnapBaseShapeTypes.Circle;

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

	// ----------------//
	// --- 类型
	// ----------------//

	//protected override SnapVector SnapToBoxs(List<SnapableBase> targets, List<SnapableBase> snapedTargets)
	//{
	//throw new System.NotImplementedException();
	//}

	/// <summary>
	/// 圆之间吸附
	/// 设计问题,这段代码和齿轮相互吸附代码基本一致
	/// </summary>
	/// <param name="targets"></param>
	/// <param name="snapedTargets"></param>
	/// <returns></returns>
	protected override SnapVector SnapToCircles(List<SnapableBase> targets, List<SnapableBase> snapedTargets)
	{
		SnapVector snapResult = SnapVector.Empty;
		List<SnapableBase> sortedGears = new List<SnapableBase>(targets);
		sortedGears.Sort((a, b) => { return Vector2.SqrMagnitude(a.MovePosition - MovePosition) > Vector2.SqrMagnitude(b.MovePosition - MovePosition) ? 1 : -1; });
		snapedTargets.Clear();
		if (sortedGears.Count >= 2)
		{
			SnapCircleAndCircle(CircleBound, sortedGears[0].CircleBound, sortedGears[1].CircleBound);
			snapedTargets.Add(sortedGears[0]);
			snapedTargets.Add(sortedGears[1]);
		}
		else if (sortedGears.Count == 1)
		{
			SnapCircleAndCircle(CircleBound, sortedGears[0].CircleBound, null);
			snapedTargets.Add(sortedGears[0]);
		}
		return snapResult;
	}

	//protected override SnapVector SnapToPoints(List<SnapableBase> targets, List<SnapableBase> snapedTargets)
	//{
	//	throw new System.NotImplementedException();
	//}
}
