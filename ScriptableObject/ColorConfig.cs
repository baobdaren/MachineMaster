using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "ColorConfig", menuName = "创建数据文件/创建ColorConfig数据", order = 12)]
public class ColorConfig : UniqueConfigBase
{
	// ------------------ //
	// --- 序列化
	// ------------------ // 
	public Color BearingDisableColor;
	public Color BearingDeleteableColor;
	public Color BearingConnectableColor;

	// ------------------ //
	// --- 公有成员
	// ------------------ //
	public static ColorConfig Instance { private set; get; }

	// ------------------ //
	// --- 私有成员
	// ------------------ //


	// ------------------ //
	// --- Unity消息
	// ------------------ //

	//private void OnValidate()
	//{
	//	Debug.Log($"配置文件 {typeof(GameConfig)} 刷新");
	//}

	//private void OnDestroy()
	//{
	//	Debug.LogError($"{typeof(GameConfig)} {this.GetInstanceID()} 删除");
	//}
	// ------------------ //
	// --- 公有方法
	// ------------------ //
	public override void InitInstance()
	{
		Instance = this;
	}


	// ------------------ //
	// --- 类型
	// ------------------ //
}
