using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class PhysicsBearing : MonoBehaviour, ILayerSettable
{
	// ----------------//
	// --- 序列化
	// ----------------//
	[SerializeField]
	private GameObject Outer;
	[SerializeField]
	private GameObject Inner;
	[SerializeField]
	private GameObject FixedRivet;

	[SerializeField]
	private GameObject FixedGameObject;
	[SerializeField]
	private GameObject HingeGameObject;


	// ----------------//
	// --- 公有成员
	// ----------------//
	public int SetOrder 
	{
		set
		{
			if (_hingeOrFixed.HasValue && _hingeOrFixed.Value == true)
			{
				SetDisplayHinge(value);
			}
			else
			{
				SetDisplayFixed(value);
			}
		}
	}


	// ----------------//
	// --- 私有成员
	// ----------------//
	private bool? _hingeOrFixed;
	//private Joint2D ScrewJoint;
	//private bool working = false;
	//private int innerBrokenLayer;
	//private int outerBrokenLayer;

	// ----------------//
	// --- Unity消息
	// ----------------//
	private void Awake()
	{
		_hingeOrFixed = null;
		FixedRivet.GetComponent<SpriteRenderer>().sortingLayerID = SortingLayer.NameToID("Physics");
		Outer.GetComponent<SpriteRenderer>().sortingLayerID = SortingLayer.NameToID("Physics");
		Inner.GetComponent<SpriteRenderer>().sortingLayerID = SortingLayer.NameToID("Physics");
	}
	//private void FixedUpdate()
	//{
	//	if (ScrewJoint == null && working)
	//	{
	//		working = false;
	//		Outer.GetComponent<SpriteRenderer>().sortingLayerID = outerBrokenLayer;
	//		Inner.GetComponent<SpriteRenderer>().sortingLayerID = innerBrokenLayer;
	//		GameObject.DestroyImmediate(gameObject);
	//	}
	//}


	// ----------------//
	// --- 公有方法
	// ----------------//
	public void SetDisplay(bool hingeOrFixed)
	{
		_hingeOrFixed = hingeOrFixed;
	}

	//public void SetDisplayHinge(Collider2D innerCollider, Collider2D outerCollider, Joint2D joint, Vector2 pos)
	//{
	//	Assert.IsTrue(ScrewJoint == null, "不能再赋值，joint已经存在了"); 
	//	// 整体都放在铰接点上（joint2D没有anchor属性？）
	//	transform.position = pos;
	//	// 修改所属父物体
	//	Outer.transform.SetParent(outerCollider.transform);
	//	Inner.transform.SetParent(innerCollider.transform);
	//	ScrewJoint = joint;
	//	// 保存层级，铰接断开后使用此层及
	//	//innerBrokenLayer = innerSpriteRenderLayer;
	//	//outerBrokenLayer = outerSpriteRenderLayer;
	//	// 轴承铰接断开前，内圈和外圈均使用最大的sortingLayer层级
	//	//int frontLayer = SortingLayer.GetLayerValueFromID(innerSpriteRenderLayer)>SortingLayer.GetLayerValueFromID(outerSpriteRenderLayer)?innerSpriteRenderLayer:outerSpriteRenderLayer;
	//	Outer.GetComponent<SpriteRenderer>().sortingLayerID = frontLayer;
	//	Inner.GetComponent<SpriteRenderer>().sortingLayerID = frontLayer;
	//	// 轴承orderLayer高于0，显示在零件上层
	//	Outer.GetComponent<SpriteRenderer>().sortingOrder = 1;
	//	Inner.GetComponent<SpriteRenderer>().sortingOrder = 1;
	//	working = true;
	//}
	// ----------------//
	// --- 私有方法
	// ----------------//
	private void SetDisplayHinge(int startLayer)
	{
		Inner.GetComponent<SpriteRenderer>().sortingOrder = startLayer;
		Outer.GetComponent<SpriteRenderer>().sortingOrder = startLayer+1;
		Inner.gameObject.SetActive(true);
		Outer.gameObject.SetActive(true);
		FixedRivet.gameObject.SetActive(false);
	}

	private void SetDisplayFixed(int startLayer)
	{
		FixedRivet.GetComponent<SpriteRenderer>().sortingOrder = startLayer;
		Inner.gameObject.SetActive(false);
		Outer.gameObject.SetActive(false);
		FixedRivet.gameObject.SetActive(true);
	}

	// ----------------//
	// --- 类型
	// ----------------//
}
