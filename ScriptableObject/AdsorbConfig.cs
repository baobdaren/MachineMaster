using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "AdsorbConfig", menuName = "创建数据文件/创建吸附配置数据", order = 12)]
public class AdsorbConfig : UniqueConfigBase
{
	// ----------------//
	// --- 序列化
	// ----------------//

	// ----------------//
	// --- 公有成员
	// ----------------//
	public static AdsorbConfig Instance { get; private set; }
	public AdsorbReleationship AdsorbReleationshipData { get; private set; }

	// ----------------//
	// --- 私有成员
	// ----------------//

	public override void InitInstance()
	{
		Instance = this;
		AdsorbReleationshipData = new AdsorbReleationship();
	}

	// ----------------//
	// --- 公有方法
	// ----------------//
	public bool IsAdsorbable(PartTypes aPart, PartTypes bPart)
	{ 
		return AdsorbReleationshipData.IsAdsorbable(aPart, bPart);
	}

	// ----------------//
	// --- 私有方法
	// ----------------//

	// ----------------//
	// --- 类型
	// ----------------//
	public class AdsorbReleationship
	{ 
		private List<(PartTypes, PartTypes)> _releations;

		public static AdsorbReleationship Instance 
		{
			get
			{
				if (_instance == null)
				{
					_instance = new AdsorbReleationship();
				}
				return _instance;
			}
		}
		public static AdsorbReleationship _instance =	new AdsorbReleationship();

		public void Add(PartTypes a, PartTypes b)
		{
			(PartTypes, PartTypes) key1 = (a, b);
			(PartTypes, PartTypes) key2 = (b, a);
			_releations.Remove(key1);
			_releations.Remove(key2);
			_releations.Add(key1);
			_releations.Add(key2);
		}

		public bool IsAdsorbable(PartTypes aPart, PartTypes bPart)
		{
			return _releations.Contains((aPart, bPart));
		}
	}
}
