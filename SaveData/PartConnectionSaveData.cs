using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[ES3Serializable]
public class PartConnectionSaveData
{
	string part1ID;
	int part1ConnectColliderIndex;
	string part2ID;
	int part2ConnectColliderIndex;
	bool isFixedJoint;
	Vector2 anchorPos;
}
