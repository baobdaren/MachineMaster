using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[ES3Serializable]
public class PartConnectionCoreData
{
	//public string part1ID;
	///// <summary>
	///// 铰接到该零件的刚体的索引
	///// </summary>
	//public int part1ColliderIndex;
	//public string part2ID;
	//public int part2ColliderIndex;
	//public bool isFixedJoint;
	//public Vector2 anchorPos;
	/// <summary>
	/// 在固定铰接中：只包含2个零件
	/// 在轴承铰接中：包含所有连接到该轴承的零件
	/// </summary>
	public string[] IDsConnectedPart;
	// 每一个零件 可能对应多个连接
	// x和零件ID对应，y则为连接到刚体的所有序号
	/// <summary>
	/// 这里默认一维对应连接零件数组的序号
	/// </summary>
	public int[] IndexConnectedPartCollider;
	public bool[] FlagConneectedPartIsFixed;
	public Vector2 AnchorPosition;
}
