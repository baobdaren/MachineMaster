using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerManager: Loadable<PlayerManager>
{

	// ------------- //
	// -- 序列化
	// ------------- //

	// ------------- //
	// -- 私有成员
	// ------------- //
	public VehiclePlayer Player
	{
		get
		{ 
			if (_player == null)
				_player = GameObject.FindFirstObjectByType<VehiclePlayer>(FindObjectsInactive.Include);
			return _player;
		}
	}
	private VehiclePlayer _player;
	// ------------- //
	// -- 公有成员
	// ------------- //

	// ------------- //
	// -- Unity 消息
	// ------------- //

	// ------------- //
	// -- 公有方法
	// ------------- //
	public bool PlayerInTheArea(Collider2D col)
	{
		return col.OverlapPoint(Player.transform.position);
	}

	// ------------- //
	// -- 私有方法
	// ------------- //
	protected override void OnResetData()
	{
		var _ = Player;
	}

	protected override void OnLoadedArchive(ArchiveManager.Archive archive)
	{
		//if (archive.playerPos == null)
		//{
		//	return;
		//}
		//Player.SetSavedPostionAndRotation(new Vector2(archive.playerPos[0], archive.playerPos[1]), archive.playerPos[2]);
	}

	protected override void OnSaveingArchive(ArchiveManager.Archive archive)
	{
		//if (Player.SaveablePosition.HasValue)
		//{
		//	archive.playerPos = new float[3];
		//	archive.playerPos[0] = Player.SaveablePosition.Value.x;
		//	archive.playerPos[1] = Player.SaveablePosition.Value.y;
		//	archive.playerPos[2] = Player.SaveableAngle;
		//}
		//else
		//{
		//	archive.playerPos = null; // 一般以空代表没有存储位置
		//}
	}

	// ------------- //
	// -- 类型
	// ------------- //
}

