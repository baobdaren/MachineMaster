using System;
using UnityEngine;
using Sirenix;
using Sirenix.OdinInspector;

[RequireComponent(typeof(Collider2D))]
public class PlayerTaskTriggle: AbsPlayerTask
{
	// ------------- //
	// -- 序列化
	// ------------- //
	[SerializeField]
	Collider2D TriggleFinishTask;

	// ------------- //
	// -- 私有成员
	// ------------- //

	// ------------- //
	// -- 公有成员
	// ------------- //

	// ------------- //
	// -- Unity 消息
	// ------------- //
	private void Awake()
	{
		Debug.Assert(TriggleFinishTask && TriggleFinishTask.isTrigger);
	}

	private void Update()
	{
		if (TaskStarted)
		{
			if (TriggleFinishTask.OverlapPoint(PlayerManager.Instance.Player.transform.position))
			{
				FinishTask(0);
			}
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (PlayerManager.Instance.Player.GetComponent<Rigidbody2D>() == collision.attachedRigidbody)
		{
			StartTask();
		}
	}
	// ------------- //
	// -- 公有方法
	// ------------- //

	// ------------- //
	// -- 私有方法
	// ------------- //

}
