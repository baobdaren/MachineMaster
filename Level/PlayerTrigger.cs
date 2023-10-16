using System.Collections.Generic;
using UnityEngine;

public class PlayerTrigger: AbsTargetTrigger
{
	// ------------- //
	// -- 序列化
	// ------------- //
	[SerializeField]
	private bool EnableVehiclePlayer = true;
	[SerializeField]
	private bool EnableAircraftPlayer = false;

	// ------------- //
	// -- 私有成员
	// ------------- //
	protected override List<GameObject> GetTriggerTargets 
	{
		get
		{
			if(_players == null) SearchPlayer();
			return _players;
		}
	}
	private List<GameObject> _players;

	// ------------- //
	// -- 公有成员
	// ------------- //

	// ------------- //
	// -- Unity 消息
	// ------------- //

	// ------------- //
	// -- 公有方法
	// ------------- //

	// ------------- //
	// -- 私有方法
	// ------------- //
	private void SearchPlayer()
	{
		if (_players != null) return;
		_players = new List<GameObject>();
		if (EnableVehiclePlayer)
			_players.Add(FindObjectOfType<VehiclePlayer>(true).gameObject);
		if (EnableAircraftPlayer)
			_players.Add(FindObjectOfType<AircraftPlayer>(true).gameObject);
	}
}
