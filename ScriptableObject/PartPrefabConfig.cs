using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PartPrefabConfig", menuName = "创建数据文件/创建零件预制体数据", order = 13)]
public class PartPrefabConfig : SerializedScriptableObject 
{
	// ------------------ //    
	// --- 序列化    
	// ------------------ //


	// ------------------ //    
	// --- 公有成员    
	// ------------------ //
	public GameObject AircraftEnginePrefab;
	public GameObject EnginePrefab;
	public GameObject GearPrefab;
	public GameObject PresserPrefab;
	public GameObject RailPrefab;
	public GameObject RopePrefab;
	public GameObject SpringPrefab;
	public GameObject SteelPrefab;
	public GameObject WheelPrefab;
	public GameObject DistanceSensorPrefab;
	public GameObject AVSensorPrefab;




	// ------------------ //   
	// --- 私有成员    
	// ------------------ //


	// ------------------ //    
	// --- Unity消息    
	// ------------------ //


	// ------------------ //    
	// --- 公有方法   
	// ------------------ //

	// ------------------ //   
	// --- 私有方法
	// ------------------ //


	// ------------------ //   
	// --- 类型
	// ------------------ //


}
