using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[ES3Serializable]
public class PartConnectionCoreData
{
	//public string part1ID;
	///// <summary>
	///// �½ӵ�������ĸ��������
	///// </summary>
	//public int part1ColliderIndex;
	//public string part2ID;
	//public int part2ColliderIndex;
	//public bool isFixedJoint;
	//public Vector2 anchorPos;
	/// <summary>
	/// �ڹ̶��½��У�ֻ����2�����
	/// ����н½��У������������ӵ�����е����
	/// </summary>
	public string[] IDsConnectedPart;
	// ÿһ����� ���ܶ�Ӧ�������
	// x�����ID��Ӧ��y��Ϊ���ӵ�������������
	/// <summary>
	/// ����Ĭ��һά��Ӧ���������������
	/// </summary>
	public int[] IndexConnectedPartCollider;
	public bool[] FlagConneectedPartIsFixed;
	public Vector2 AnchorPosition;
}
