using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "ColorConfig", menuName = "���������ļ�/����ColorConfig����", order = 12)]
public class ColorConfig : UniqueConfigBase
{
	// ------------------ //
	// --- ���л�
	// ------------------ // 
	public Color BearingDisableColor;
	public Color BearingDeleteableColor;
	public Color BearingConnectableColor;

	// ------------------ //
	// --- ���г�Ա
	// ------------------ //
	public static ColorConfig Instance { private set; get; }

	// ------------------ //
	// --- ˽�г�Ա
	// ------------------ //


	// ------------------ //
	// --- Unity��Ϣ
	// ------------------ //

	//private void OnValidate()
	//{
	//	Debug.Log($"�����ļ� {typeof(GameConfig)} ˢ��");
	//}

	//private void OnDestroy()
	//{
	//	Debug.LogError($"{typeof(GameConfig)} {this.GetInstanceID()} ɾ��");
	//}
	// ------------------ //
	// --- ���з���
	// ------------------ //
	public override void InitInstance()
	{
		Instance = this;
	}


	// ------------------ //
	// --- ����
	// ------------------ //
}
