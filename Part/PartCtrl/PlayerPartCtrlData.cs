using UnityEngine;

[ES3Serializable]
public class PlayerPartCtrlData : PartCtrlCoreData
{
	/// <summary>
	/// ES3需要
	/// </summary>
	public PlayerPartCtrlData() { }
	/// <summary>
	/// 深拷贝
	/// </summary>
	/// <param name="other"></param>
	public PlayerPartCtrlData(PlayerPartCtrlData other) : base(other)
	{

	}
	/// <summary>
	/// 创建
	/// </summary>
	/// <param name="partType"></param>
	public PlayerPartCtrlData(PartTypes partType) : base(partType)
	{
		Debug.Assert(partType != PartTypes.Custom, "玩家零件不该是场景零件类型");
	}

	public override bool IsPlayerPart => true;
}
