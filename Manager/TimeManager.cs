using UnityEngine;

public class TimeManager
{
	// ------------- //
	// -- 序列化
	// ------------- //

	// ------------- //
	// -- 私有成员
	// ------------- //
	private float _standardValue;
	private bool _editing = false;
	private bool _pausing = false;

	// ------------- //
	// -- 公有成员
	// ------------- //
	public static TimeManager Instance = new TimeManager();

	/// <summary>
	/// 是否在编辑模式下
	/// 编辑模式会暂停时间
	/// </summary>
	public bool IsEditing
	{ 
		get { return _editing; }
		set 
		{ 
			_editing = value;
			AutoTimeScale();
		}
	}

	public bool IsPausing
	{ 
		get { return _pausing; }
		set 
		{ 
			_pausing = value;
			AutoTimeScale();
		}
	}
	// ------------- //
	// -- Unity 消息
	// ------------- //

	// ------------- //
	// -- 公有方法
	// ------------- //

	// ------------- //
	// -- 私有方法
	// ------------- //
	private TimeManager()
	{
		_standardValue = Time.fixedDeltaTime;
	}

	private void AutoTimeScale()
	{ 
		if(IsEditing || IsPausing) 
		{
			Time.timeScale = 0f; 
		}
		else
		{
			Time.timeScale = 1f;
		}
	}
}
