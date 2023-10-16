using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.U2D;

public class VisualSteel : SerializedMonoBehaviour
{
	// ----------------//
	// --- 序列化
	// ----------------//

	// ----------------//
	// --- 公有成员
	// ----------------//
	public void SetCorner (List<Vector3> pos)
	{
		if(_spCtrler != null) _spCtrler = GetComponent<SpriteShapeController>();
		_spCtrler.spline.Clear();
		for (int i = 0; i < pos.Count; i++)
		{
			_spCtrler.spline.InsertPointAt(i, pos[i]);
		}
	}

	public void SetCorner(params Vector3[] corners)
	{
		_spCtrler.spline.Clear();
		for (int i = 0; i < corners.Length; i++)
		{
			_spCtrler.spline.InsertPointAt(0, corners[corners.Length - 1 - i]);
		}
	}

	public bool Display 
	{
		set
		{
			if(gameObject.activeSelf != value)
				gameObject.SetActive(value);
		}
	
	}

	public int VirsualCorner
	{
		set
		{
			_spCtrler.spline.SetSpriteIndex(value, 1);
		}
	}

	// ----------------//
	// --- 私有成员
	// ----------------//
	private SpriteShapeController _spCtrler;

	// ----------------//
	// --- Unity消息
	// ----------------//
	private void Awake()
	{
		_spCtrler = GetComponent<SpriteShapeController> ();
	}

	// ----------------//
	// --- 公有方法
	// ----------------//

	// ----------------//
	// --- 私有方法
	// ----------------//

	// ----------------//
	// --- 类型
	// ----------------//
}
