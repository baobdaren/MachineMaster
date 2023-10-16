using UnityEngine;

public class RoboticArm: MonoBehaviour
{
	// ------------- //
	// -- 序列化
	// ------------- //
	private ParticleSystem sparksParticleSystem;

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
		sparksParticleSystem = GetComponentInChildren<ParticleSystem>();
	}

	private void AniEvent_Flash(string eventName)
	{
		//Debug.Log("Flash" + eventName);
		sparksParticleSystem.Stop();
		sparksParticleSystem.Play();
	}

	// ------------- //
	// -- 公有方法
	// ------------- //

	// ------------- //
	// -- 私有方法
	// ------------- //

}
