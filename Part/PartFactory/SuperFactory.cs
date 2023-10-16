using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����Ĵ������ó��󹤳�ģʽ��ÿ�ֹ��������������������˴˲�ͬȴ�й��ԣ����������ṩ�˲�Ʒ���ֱ��������ʽ������ֱ�ӵ��ù�����
/// ���������������ֱ�ӵ��ø��๤��
/// </summary>
public class PlayerPartSuperFactory
{
	// ----------------//
	// --- ���л�
	// ----------------//

	// ----------------//
	// --- ���г�Ա
	// ----------------//

	// ----------------//
	// --- ˽�г�Ա
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
	// --- Unity��Ϣ
	// ----------------//

	// ----------------//
	// --- ���з���
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
		Debug.Assert(false, "û���ҵ�����");
		return null;
	}

	/// <summary>
	/// �����༭״̬���
	/// </summary>
	/// <param name="partType">�������</param>
	/// <returns></returns>
	public static PlayerPartCtrl CreatePlayerEditPart(PartTypes partType)
	{
		return CreatePlayerEditPart(new PlayerPartCtrlData(partType));
	}

	/// <summary>
	/// �����༭״̬���ʵ��
	/// </summary>
	/// <param name="partCtrl">�������</param>
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
	/// �����༭״̬�����
	/// �������е�����������
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
			Debug.LogError("�����������ʧ�� - " + partCtrl.MyPartType);
		}
		partCtrl.MyPhysicsPart = result;
	}

	// ----------------//
	// --- ˽�з���
	// ----------------//
	private static GameObject CreateEditPartGameObject(PlayerPartCtrl partCtrl)
	{
		return _partTypeToFactory[partCtrl.MyPartType].CreateEditPart(partCtrl);
	}

	// ----------------//
	// --- ����
	// ----------------//
}
