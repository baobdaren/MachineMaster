using System;
using UnityEngine;

[ES3Serializable]
public class ScenePartCtrlData : PartCtrlCoreData
{
	public Hash128 PartID;

	public ScenePartCtrlData() { }

	/// <summary>
	/// Éî¿½±´
	/// </summary>
	/// <param name="other"></param>
	public ScenePartCtrlData(ScenePartCtrlData other):base(other) 
	{
		PartID = other.PartID;
	}

	public override bool IsPlayerPart => false;

	//public ScenePartCtrlData() { }
}