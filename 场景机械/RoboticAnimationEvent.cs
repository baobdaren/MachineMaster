using UnityEngine;

public class RoboticAnimationEvent: StateMachineBehaviour
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
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		Debug.Log(stateInfo);
		Debug.Log(layerIndex);
	}

	public delegate void AnimationEvent(string eventName);
	public event AnimationEvent onAnimationEvent;

	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		onAnimationEvent?.Invoke("Flash");
		Debug.Log("state exit");
	}

	public void Flash(string s,int i)
	{
		Debug.Log("animator 事件 flash" + s);
	}
	// ------------- //
	// -- 公有方法
	// ------------- //

	// ------------- //
	// -- 私有方法
	// ------------- //

}
