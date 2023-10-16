using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringFactory : AbsPlayerPartFactory
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

	// ----------------//
	// --- 类型
	// ----------------//

	protected override void OnCreatedAsEdit(PlayerPartCtrl partCtrlData, AbsBasePlayerPartAccessor acc)
	{
		SpringAccessor springAccessor = acc as SpringAccessor;
		springAccessor.SliderJoint.enabled = false;
		springAccessor.SpringJoint.enabled = false;
		foreach (var item in springAccessor.AllRigids)
		{
			PartConfig.Instance.PartRigidConfig.AppliyEditConfig(item);
		}
	}

	protected override void OnCreatedAsPhysics(PlayerPartCtrl partCtrlData, AbsBasePlayerPartAccessor accessor)
	{
		SpringAccessor springAccessor = accessor as SpringAccessor;
		springAccessor.SliderJoint.enabled = true;
		springAccessor.SpringJoint.enabled = true;
		springAccessor.SliderJoint.useLimits = true;
		springAccessor.SpringJoint.distance = PartConfig.Instance.SpringConfig.SpringDistanceMaxValue;
		springAccessor.SpringJoint.frequency = PartConfig.Instance.SpringConfig.Frequency;
		springAccessor.SpringJoint.dampingRatio = PartConfig.Instance.SpringConfig.DampingRatio;
		springAccessor.SliderJoint.limits = new JointTranslationLimits2D()
		{
			max = PartConfig.Instance.SpringConfig.SpringDistanceMaxValue,
			min = PartConfig.Instance.SpringConfig.SpringDistanceMinValue
		};
		foreach (var item in springAccessor.AllRigids)
		{
			PartConfig.Instance.PartRigidConfig.ApplySimulateConfig(item);
		}
		springAccessor.SpringJoint.autoConfigureDistance = false;
	}

	protected override void OnSetSize(PlayerPartCtrl partCtrlData, AbsBasePlayerPartAccessor accessor)
	{
		SpringAccessor springAccessor = accessor as SpringAccessor;
		Debug.LogWarning("我们需要修改尺寸修改的配置，如这里的100");
		springAccessor.Bottom.transform.localPosition = Vector3.down * partCtrlData.Size;
	}
}
