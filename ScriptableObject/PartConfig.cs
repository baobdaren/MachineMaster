using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "PartConfig", menuName = "创建数据文件/创建零件配置数据", order = 12)]
public class PartConfig : UniqueConfigBase
{
    // 统一节点对象（齿轮，铰链，电机）的齿间距为2
    //public const float DISTANCE_SECTION = 0.1f;
    //public const float DISTANCE_CIRLESECTION = DISTANCE_SECTION * 0.455f;

    // 模拟世界的重力/质量比例
    // 由于模型大小一致，在不同场景下需要统一调整重力和质量
    // 只影响重力和质量
    [Header("模拟物理的整体比例")]
    public static float WorldScale = 1;

    private GameObject CreateParent;

    // ------------------ //    
    // --- 序列化    
    // ------------------ //
    [Header("物理模式刚体的基准参数")]
    public RigidConfig PartRigidConfig;

    [Space]
    [Header("发动机")]
    public JETEngineConfigData JETEngineConfig;

    [Header("电机")]
    [Space]
    public EngineConfigData EngineConfig;

    [Header("齿轮")]
    [Space]
    public GearConfigData GearConfig;

    [Header("液压器")]
    [Space]
    public PresserConfigData PresserConfig;

    [Header("铰链")]
    [Space]
    public RailConfigData RailConfig;

    [Header("绳子")]
    [Space]
    public RopeConfigData RopeConfig;

    [Header("减震器")]
    [Space]
    public SpringConfigData SpringConfig;

    [Header("钢材")]
    [Space]
    public SteelConfigData SteelConfig;

    [Header("车轮")]
    [Space]
    public WheelConfigData WheelConfig;

    [Header("距离传感器")]
    [Space]
    public DistanceSensorData DistanceSensor;

    [Header("角度/速度传感器")]
    [Space]
    public VelocitySensorData VelocitySensor;

    [Header("铰接轴承")]
    [Space]
    public GameObject EditBearing_Fixed;
    public GameObject EditBearing_Hinge;

    [Header("物理模拟交接轴承")]
    [Space]
    public GameObject PhysicsConnection;

    [Header("Part SortingLayer IDs")]
    //[Sirenix.OdinInspector.w]
    [ListDrawerSettings(NumberOfItemsPerPage = 20)]
    public List<(PartTypes, Vector2)> SortingLayerList = new List<(PartTypes, Vector2)>()
    {
        (PartTypes.JETEngine, new Vector2(0,1))
    };

    // ------------------ //    
    // --- 公有成员    
    // ------------------ //
    public static PartConfig Instance { private set; get; }
    [Header("默认的零件")]
    public PartPrefabConfig DefaultPartPrefabs;

    // ------------------ //   
    // --- 私有成员    
    // ------------------ //
   // private PartPrefabConfig LevelPartPrefab
   // {
   //     get
   //     {
			//if (CurrentChapterNameCache != GameManager.Instance.SelectedChapterName)
			//{
   //             CurrentChapterNameCache = GameManager.Instance.SelectedChapterName;
   //             _levelPartPrefabConfig = AssetsManager.Instance.LoadPartPrefab(CurrentChapterNameCache);
			//}
			//if (_levelPartPrefabConfig == null)
			//{
   //             _levelPartPrefabConfig = Instance.DefaultPartPrefabs;
			//}
   //         return _levelPartPrefabConfig;
   //     }
   // }
    private string CurrentChapterNameCache 
    {
        set { /*Debug.LogError("赋值");*/ _tets = value;  }
        get => _tets;
    }
    private string _tets;
    private PartPrefabConfig _levelPartPrefabConfig;

    // ------------------ //    
    // --- Unity消息    
    // ------------------ //

    // ------------------ //    
    // --- 公有方法   
    // ------------------ //
    public GameObject InstantiatePart(PartTypes partType, bool isEditPart)
    {
        //Debug.Log($"{(LevelPartPrefab == null ? "没有" : "有")}关卡零件预制配置");
		GameObject prefab = partType switch
		{
			PartTypes.JETEngine => JETEngineConfig.AircraftEnginePrefab,
			PartTypes.Engine => EngineConfig.EnginePrefab,
			PartTypes.Gear => GearConfig.GearPrefab,
			PartTypes.Presser => PresserConfig.PresserPrefab,
			PartTypes.Rail => RailConfig.RailPrefab,
			PartTypes.Rope => RopeConfig.RopePrefab,
			PartTypes.Spring => SpringConfig.SpringPrefab,
			PartTypes.Steel => SteelConfig.SteelPrefab,
			PartTypes.Wheel => WheelConfig.WheelPrefab,
			PartTypes.AVSensor => VelocitySensor.AVSensorPrefab,
			PartTypes.DISSensor => DistanceSensor.DistanceSensorPrefab,
			_ => null,
		};
        Assert.IsNotNull(prefab);
        GameObject parent = isEditPart ? ParentsManager.Instance.ParentOfEditParts : ParentsManager.Instance.ParentOfPhysicsParts;
        GameObject result = Instantiate(prefab, parent.transform);
        result.GetComponentInChildren<AbsBasePlayerPartAccessor>().InitAccessor();
        //result.transform.SetParent(parent, true);
		foreach (var item in result.GetComponentsInChildren<Transform>(true))
		{
			if (item.GetComponent<KeepLayer>())
			{
                //keepLayer.SetSlefLayer();
			}
			else
			{
                item.gameObject.layer = isEditPart ? GameLayerManager.EditPart : GameLayerManager.DefaultPartObjectLayer;
			}
            //Debug.Log($"设置层级 {item.gameObject.name} 为 {item.gameObject.layer}");
		}
        return result;
	}

    /// <summary>
    /// 获取设置尺寸修改界面的参数
    /// </summary>
    /// <param name="partType"></param>
    /// <returns>最小值，最大值，是否使用整数</returns>
    public (float, float, bool) GetPartSizeSliderSetting(PartTypes partType)
    {
        IPartConfig config = partType switch
        {
            PartTypes.JETEngine => JETEngineConfig,
            PartTypes.Engine => EngineConfig,
            PartTypes.Gear => GearConfig,
            PartTypes.Presser => PresserConfig,
            PartTypes.Rail => RailConfig,
            PartTypes.Rope => RopeConfig,
            PartTypes.Spring => SpringConfig,
            PartTypes.Steel => SteelConfig,
            PartTypes.Wheel => WheelConfig,
            PartTypes.AVSensor => VelocitySensor,
            PartTypes.DISSensor => DistanceSensor,
            _ => null,
        };
        return (config.SpecUpper, config.SpecLower, config.SpecIsWholeNum);
    }

    public PlayerPartBase AddPartBehaviorCmpnt(PartTypes partType, GameObject part, PlayerPartCtrl partCtrl)
    {
		PlayerPartBase pb = partType switch
		{
			PartTypes.JETEngine => part.AddComponent<JETEngine>(),
			PartTypes.Engine => part.AddComponent<Engine>(),
			PartTypes.Gear => part.AddComponent<Gear>(),
			PartTypes.Presser => part.AddComponent<Presser>(),
			PartTypes.Rail => part.AddComponent<Chain>(),
			PartTypes.Rope => part.AddComponent<Rope>(),
			PartTypes.Spring => part.AddComponent<Spring>(),
			PartTypes.Steel => part.AddComponent<Steel>(),
			PartTypes.Wheel => part.AddComponent<Wheel>(),
			PartTypes.AVSensor => part.AddComponent<AngleVelocitySensor>(),
			PartTypes.DISSensor => part.AddComponent<DistanceSensor>(),
			_ => part.AddComponent<Wheel>(),//Debug.LogError("添加错误的零件行为PB脚本");
		};
        pb.MyCtrlData = partCtrl;
        return pb;
	}

    public override void InitInstance()
	{
        Instance = this;
	}

    public static void ApplyEdit(Rigidbody2D rbody)  
    {
        rbody.bodyType = RigidbodyType2D.Kinematic;
    }
	

    public static void ApplyHighDrag(Rigidbody2D rbody)
    {
        rbody.bodyType = RigidbodyType2D.Dynamic;
		//rbody.useFullKinematicContacts = true;
		rbody.mass = float.MaxValue;
		rbody.gravityScale = 0;
	}

    // ------------------ //   
    // --- 类型
    // ------------------ //
    /// <summary>
	/// 基本的简单零件，这类零件只有一个刚体组件，具有多节的零件质量可以依据自然数推导
	/// 电机/航空发动机
	/// 钢材      -- 依据节的数量推导总质量
	/// 铰链/绳子  -- 代表每一节的质量
	/// </summary>
	[Serializable]
    public class RigidConfig
    {
        const float Drag = 0.1f;
        const float AngularDrag = 0.05f;
        public float Mass;
        public float GravityScale;
        public PhysicMaterial PhysicMaterial;
        public virtual void ApplySimulateConfig(Rigidbody2D rigid, float randomDrag = Drag, float angularDrag = AngularDrag)
        {
            //rigid.useAutoMass = true;
            rigid.simulated = true;
            rigid.bodyType = RigidbodyType2D.Dynamic;
            rigid.drag = Drag;
            rigid.angularDrag = AngularDrag;
            rigid.gravityScale = GravityScale * WorldScale;
            if (rigid.gravityScale == 0)
            {
                Debug.LogError("这个引力系数为 0 =" + rigid.name);
            }
        }
        public virtual void ApplyHighDragConfig(Rigidbody2D rigid)
        {
            rigid.useAutoMass = false;
            rigid.simulated = true;
            rigid.bodyType = RigidbodyType2D.Dynamic;
            rigid.mass = 0;
            rigid.drag = 100;
            rigid.angularDrag = 10;
            rigid.gravityScale = 0;
        }
        public void AppliyEditConfig(Rigidbody2D rigid)
        {
            rigid.useAutoMass = false;
            rigid.simulated = true;
            rigid.bodyType = RigidbodyType2D.Static;
            rigid.mass = 0;
            rigid.drag = 0;
            rigid.angularDrag = 0;
            rigid.gravityScale = 0;
        }
    }

    /// <summary>
    /// 带有特殊尺寸的零件，这类零件必须给出一个尺寸，而不是自然数的递增
    /// 车轮，齿轮
    /// </summary>
    [Serializable]
    public class DiffSizeRigidConfigData : RigidConfig
    {
        public float Size;
    }

    /// <summary>
    /// 铰接组成，这类零件自带铰接属性，必须给出两个刚体组件的属性
    /// 减震器/液压器
    /// </summary>
    [Serializable]
    public class SlidingRigidConfig
    {
        public RigidConfig Top;
        public RigidConfig Bottom;
    }

    /// <summary>
    /// 铰接组件的属性
    /// </summary>
    [Serializable]
    public class ConnectSlidingRigidConfig : SlidingRigidConfig
    {
        [SerializeField]
        public float LimitMin;
        [SerializeField]
        public float LimitMax;
        [SerializeField]
        [Range(0, 1)]
        protected float DampingRatio;
        [SerializeField]
        [Range(0, 200)]
        protected float Frequency;
        public void ApplyJoint(SpringJoint2D joint)
        {
            if (Frequency == 0)
            {
                Debug.LogError("可能 错误的频率值 0");
            }
            joint.dampingRatio = DampingRatio;
            joint.frequency = Frequency;
        }
    }
    public enum Enum_PhysicsMaterial
	{
        IRON,WOOD,
	}


    public abstract class IPartConfig
    {
        public abstract float SpecUpper { get; }
        public abstract float SpecLower { get; }
        public abstract bool SpecIsWholeNum { get; }
    }

    [Serializable]
    public class WheelConfigData:IPartConfig
    {
        [SerializeField]
        [Tooltip("齿轮是依据面积大小确定质量的")]
        public Vector2 WheelOrigiSize => WheelPrefab.GetComponent<SpriteRenderer>().size;

        public GameObject WheelPrefab => Instance.DefaultPartPrefabs.WheelPrefab;

        public override float SpecUpper => 0;
        public override float SpecLower => 5;
		public override bool SpecIsWholeNum => true;
	}

    [Serializable]
    public class SteelConfigData : IPartConfig
    {
        [SerializeField]
        private int _maxSectionAmount;
        public int MaxSectionAmount => _maxSectionAmount;

        public Vector2 SteelOrigiSize => SteelPrefab.GetComponent<SpriteRenderer>().size;

        public GameObject SteelPrefab => Instance.DefaultPartPrefabs.SteelPrefab;
        public override float SpecUpper => 0;
        public override float SpecLower => 0;
		public override bool SpecIsWholeNum => true;
	}

    [Serializable]
    public class SpringConfigData : IPartConfig
    {
        public GameObject SpringPrefab => Instance.DefaultPartPrefabs.SpringPrefab;
        public float SpringDistanceMinValue = 1.7f;
        public float SpringDistanceMaxValue = 10.8f;
        public override float SpecUpper => SpringDistanceMinValue;
		public override float SpecLower => SpringDistanceMaxValue;
        [HideInInspector]
		public override bool SpecIsWholeNum => false;
        [SerializeField]
        [Range(0, 1)]
        public float DampingRatio;
        public float Frequency;
    }

    [Serializable]
    public class RopeConfigData : IPartConfig
    {
        public float Rope_Drag = 0.4f;
        public float Rope_AngularDrag = 0.6f;
        public GameObject RopePrefab => Instance.DefaultPartPrefabs.RopePrefab;

        public override float SpecUpper => 4;
		public override float SpecLower => 32;
		public override bool SpecIsWholeNum => true;
	}

    [Serializable]
    public class RailConfigData : IPartConfig
    {
        public int Rail_SectionAmount = 64;
        //public int ConnSectionLayer { get => GameLayerManager }
        public GameObject RailPrefab => Instance.DefaultPartPrefabs.RailPrefab;
        public override float SpecUpper =>  4;
		public override float SpecLower => 64;
		public override bool SpecIsWholeNum => true;
	}

    [Serializable]
	public class PresserConfigData : IPartConfig
	{
        public GameObject PresserPrefab => Instance.DefaultPartPrefabs.PresserPrefab;
        public float SpringDistanceMinValue = 1.7f;
        public float SpringDistanceMaxValue = 10.8f;
        public float Frequency = 100f;
        public override float SpecUpper => SpringDistanceMinValue;
		public override float SpecLower => SpringDistanceMaxValue;
		public override bool SpecIsWholeNum => false;
	}

    [Serializable]
    public class GearConfigData : IPartConfig
    {
        public GameObject GearPrefab => Instance.DefaultPartPrefabs.GearPrefab;
        public override float SpecUpper => 0;
		public override float SpecLower => 7;
		public override bool SpecIsWholeNum => true;
	}

    [Serializable]
	public class JETEngineConfigData : IPartConfig
	{
        public GameObject AircraftEnginePrefab => Instance.DefaultPartPrefabs.AircraftEnginePrefab;
        public float PushForceMax = 10000;
        public override float SpecUpper => 0;
		public override float SpecLower => PushForceMax;
		public override bool SpecIsWholeNum => true;
        public float AnglePerSecond = 90 / 3;
	}

    [Serializable]
	public class EngineConfigData : IPartConfig
	{
        public GameObject EnginePrefab => Instance.DefaultPartPrefabs.EnginePrefab;

        [Min(0.001f)]
        public float MaxMotorPower;
        [Min(1)]
        public float MaxMotorSpeed;
        public override float SpecUpper => 0;
		public override float SpecLower => 0;
		public override bool SpecIsWholeNum => true;
	}

    [Serializable]
    public class DistanceSensorData : IPartConfig
    {
        public GameObject DistanceSensorPrefab => Instance.DefaultPartPrefabs.DistanceSensorPrefab;

        public override float SpecUpper => 0;
		public override float SpecLower => 0;
		public override bool SpecIsWholeNum => true;
	}

    [Serializable]
    public class VelocitySensorData : IPartConfig
    {
        public GameObject AVSensorPrefab => Instance.DefaultPartPrefabs.AVSensorPrefab;
        public override float SpecUpper => 0;
		public override float SpecLower => 0;
		public override bool SpecIsWholeNum => true;
	}
}
