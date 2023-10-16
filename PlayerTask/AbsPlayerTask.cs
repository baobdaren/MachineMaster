using System;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// 玩家任务的基础类
/// </summary>
public abstract class AbsPlayerTask:MonoBehaviour
{
	// ------------- //
	// -- 序列化
	// ------------- //
	/// <summary>
	/// 该任务的前置任务
	/// </summary>
	[SerializeField]
	protected List<AbsPlayerTask> PreviousTasks;

	// ------------- //
	// -- 私有成员
	// ------------- //

	// ------------- //
	// -- 公有成员
	// ------------- //
	public event Action<int> OnTaskFinish;
	public event Action OnTaskStart;

	public bool PreviousAllFinished { get => PreviousTasks == null || PreviousTasks.FindIndex(t => t.TaskFinished == false) == -1; }
	public bool TaskFinished { get; protected set; }
	protected bool TaskStarted { get; set; }

	// ------------- //
	// -- Unity 消息
	// ------------- //

	// ------------- //
	// -- 公有方法
	// ------------- //

	// ------------- //
	// -- 私有方法
	// ------------- //
	protected virtual void FinishTask(int finishResult = 0)
	{
		if (TaskFinished)
		{
			return;
		}
		TaskFinished = true;
		OnTaskFinish?.Invoke(finishResult);
	}

	protected virtual void StartTask()
	{
		if (TaskStarted || TaskFinished)
		{
			return;
		}
		if (PreviousAllFinished == false)
		{
			return;
		}
		TaskStarted = true;
		OnTaskStart?.Invoke();
	}
}
