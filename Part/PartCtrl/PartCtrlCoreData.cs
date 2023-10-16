using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

/// <summary>
/// 零件的核心数据
/// </summary>
[ES3Serializable]
public abstract class PartCtrlCoreData
{
	public PartCtrlCoreData(PartTypes partType) 
	{
		MyPartType = partType; 
		Spec = PartConfig.Instance.GetPartSizeSliderSetting(MyPartType).Item1;
		SectionDataList = new List<(Vector3, Quaternion)>();
	}

	/// <summary>
	/// 深拷贝一个副本
	/// </summary>
	/// <param name="other"></param>
	public PartCtrlCoreData(PartCtrlCoreData other)
	{
		MyPartType = other.MyPartType;
		Layer = other.Layer;
		Spec = other.Spec;
		Position = other.Position;
		Rotation = other.Rotation;
		if(other.SectionDataList != null)
		{
			SectionDataList = new List<(Vector3, Quaternion)>(other.SectionDataList);
		}
	}


	/// <summary>
	/// ES3持久化使用的无参构造函数
	/// </summary>
	public PartCtrlCoreData() { }

	public int Layer;
	public float Spec; // 规格
	public PartTypes MyPartType;
	public Vector3 Position;
	public Quaternion Rotation;
	public List<(Vector3, Quaternion)> SectionDataList;
	public List<float> Sizes;
	public abstract bool IsPlayerPart { get; }
}


[Serializable]
public enum PartTypes
{
	JETEngine, Engine, Gear, Presser, Rail, Rope, Spring, Steel, Wheel, AVSensor, DISSensor, Custom
}