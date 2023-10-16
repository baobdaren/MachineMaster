using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelAccessor : AbsBasePlayerPartAccessor
{
	// ----------------//
	// --- 序列化
	// ----------------//
	[SerializeField]
	private List<Sprite> _wheelSprites;

	// ----------------//
	// --- 公有成员
	// ----------------//

	// ----------------//
	// --- 私有成员
	// ----------------//
	private SpriteRenderer _render => GetComponent<SpriteRenderer>();

	// ----------------// 
	// --- Unity消息
	// ----------------//
	public void Start()
	{
		transform.name += $" hash ={gameObject.GetHashCode()}";
	}

	// ----------------//
	// --- 公有方法
	// ----------------//
	public void SetDisplay(float size)
	{
		int index = (int)size;
		Debug.Assert(index == size && _wheelSprites.Count > index);
		//Debug.Log("选择尺寸" + index);
		//size--;
		_render.sprite = _wheelSprites[index];
		// 以256为1个单位所包含的像素
		// -0.028 使碰撞器比轮胎略小一点
		GetComponent<CircleCollider2D>().radius = 0.5f + 0.125f * index - 0.028f;
	}

	// ----------------//
	// --- 私有方法
	// ----------------//

	// ----------------//
	// --- 类型
	// ----------------//
}
