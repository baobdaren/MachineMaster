using UnityEngine;

public class PlayerTaskDialog: AbsPlayerTask
{
	// ------------- //
	// -- 序列化
	// ------------- //

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
		StartTask();
	}

	// ------------- //
	// -- 公有方法
	// ------------- //
	public void DialogFinished(int input = 0)
	{ 
		FinishTask(input);
	}

	// ------------- //
	// -- 私有方法
	// ------------- //

}
