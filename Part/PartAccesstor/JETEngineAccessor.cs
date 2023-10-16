using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.VFX;
using Sirenix.OdinInspector;

public class JETEngineAccessor : AbsBasePlayerPartAccessor
{

	// ------------- //
	// --  序列化
	// ------------- //
	//[SerializeField]
	//public VisualEffect FlameVFX;
	[ChildGameObjectsOnly]
	public ParticleSystem FlameVFX;
	public Vector2 AddForcePos 
	{
		get
		{
			if (_forcePos == null)
				_forcePos = FlameVFX.transform.localPosition;
			return _forcePos.Value;
		}
	}
	private Vector2? _forcePos;
	// ------------- //
	// --  私有方法
	// ------------- //
	protected override List<Renderer> InitGetAllRenders()
	{
		List<Renderer> renders = new List<Renderer>()
		{
			FlameVFX.GetComponent<Renderer>(),
			GetComponent<Renderer>()
		};
		return renders;
	}
}
