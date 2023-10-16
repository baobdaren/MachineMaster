using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 零件的创建采用抽象工厂模式（每种工厂都生产零件，但零件彼此不同却有共性；超级工厂提供了产品族的直接生产方式，无需直接调用工厂）
/// 这个超级工厂帮助直接调用各类工厂
/// </summary>
public class PlayerPartSuperFactory
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
	private PlayerPartSuperFactory() { }

	private static Dictionary<PartTypes, AbsPlayerPartFactory> _partTypeToFactory =
	new Dictionary<PartTypes, AbsPlayerPartFactory>()
	{
		[PartTypes.JETEngine] =  new JETEngineFactory(),
		[PartTypes.Engine] = new EngineFactory(),
		[PartTypes.Gear] = new GearFactory(),
		[PartTypes.Presser] = new PresserFactory(),
		[PartTypes.Rail] = new RailFactory(),
		[PartTypes.Rope] = new RopeFactory(),
		[PartTypes.Spring] = new SpringFactory(),
		[PartTypes.Steel] = new SteelFactory(),
		[PartTypes.Wheel] = new WheelFactory(),
		[PartTypes.AVSensor] = new AngleVelocitySensorFactory(),
		[PartTypes.DISSensor] = new DistanceSensorFactory(),
	};


	// ----------------//
	// --- Unity消息
	// ----------------//

	// ----------------//
	// --- 公有方法
	// ----------------//
	public static T GetFactor<T>() where T : AbsPlayerPartFactory
	{
		foreach (var item in _partTypeToFactory)
		{
			if (item.Value is T)
			{
				return item.Value as T;
			}
		}
		Debug.Assert(false, "没有找到工厂");
		return null;
	}

	/// <summary>
	/// 创建编辑状态零件
	/// </summary>
	/// <param name="partType">零件类型</param>
	/// <returns></returns>
	public static PlayerPartCtrl CreatePlayerEditPart(PartTypes partType)
	{
		return CreatePlayerEditPart(new PlayerPartCtrlData(partType));
	}

	/// <summary>
	/// 创建编辑状态零件实例
	/// </summary>
	/// <param name="partCtrl">零件数据</param>
	/// <returns></returns>
	public static PlayerPartCtrl CreatePlayerEditPart(PlayerPartCtrlData partCtrlData)
	{
		PlayerPartCtrl partCtrl = new PlayerPartCtrl(partCtrlData);
		_partTypeToFactory[partCtrl.MyPartType].CreateEditPart(partCtrl);
		ModifyPlayerEditPart(partCtrl);
		PlayerPartManager.Instance.AddPart(partCtrl);
		return partCtrl;
	}

	/// <summary>
	/// 创建编辑状态的零件
	/// 依据已有的数据来创建
	/// </summary>
	/// <param name="partCtrl"></param>
	/// <returns></returns>
	public static void ModifyPlayerEditPart(PlayerPartCtrl partCtrl)
	{
		_partTypeToFactory[partCtrl.MyPartType].ModifyPart(partCtrl);
	}

	public static void CreatePhysicsPart(PlayerPartCtrl partCtrl)
	{
		PlayerPartBase result = _partTypeToFactory[partCtrl.MyPartType].CreatePhysicsPart(partCtrl);
		result.SetAsPhysicsLayer();
		if (result == null)
		{
			Debug.LogError("创建物理零件失败 - " + partCtrl.MyPartType);
		}
		partCtrl.MyPhysicsPart = result;
	}

	// ----------------//
	// --- 私有方法
	// ----------------//
	private static GameObject CreateEditPartGameObject(PlayerPartCtrl partCtrl)
	{
		return _partTypeToFactory[partCtrl.MyPartType].CreateEditPart(partCtrl);
	}

	// ----------------//
	// --- 类型
	// ----------------//
}
