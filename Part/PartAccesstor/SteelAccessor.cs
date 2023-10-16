using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class SteelAccessor : AbsBasePlayerPartAccessor
{
	// ----------------//
	// --- 序列化
	// ----------------//
	[SerializeField]
	private CapsuleCollider2D[] _sections; 

	// ----------------//
	// --- 公有成员
	// ----------------//
	public CapsuleCollider2D[] ColliderSections { get => _sections; }

	/// <summary>
	/// 根据数据修改表现
	/// </summary>
	/// <param name="corners"></param>
	public void UpdateCornersLineRender(List<(Vector3, Quaternion)> corners)
	{
		// 设计拐角！！！
		// X 方案1：小幅度拐角直接连接，拐弯无过渡；大幅度拐角采用“大”圆形碰撞作为拐角碰撞体
		// X 方案2：不论拐角幅度多大，都采用圆形碰撞体；当拐角过大（此时圆形碰撞体无法覆盖拐角）时逐渐增加半径
		// X 方案3：幅度小时采用其他方案；幅度大时使用肋板来掩饰半径较大的圆形碰撞器。
		//  方案4：拐角幅度大时添加段（条件就是显示效果）
		// 最终方案：在拐角处使用一个胶囊碰撞器
		if (ValidateCorners(corners) == false) return;
		_spCtrlerSpline.Clear();
		for (int i = corners.Count - 1; i >= 0; i--)
		{
			Vector3 curPos = corners[i].Item1;
			if (i != corners.Count - 1 && i != 0)
			{
				// 中间拐点
				// 顺序上的上一个点
				Vector3 leftPos = corners[i - 1].Item1;
				// 顺序上的下一个点
				Vector3 rightPos = corners[i + 1].Item1;
				Vector3 tangentDir = Quaternion.Euler(0,0,90) * ((leftPos - curPos).normalized + (rightPos - curPos).normalized).normalized;

				//Vector3 rightCornerPoint = curPos + tangentDir * _capsuleRadius;
				//_spCtrlerSpline.InsertPointAt(0, rightCornerPoint);
				//_spCtrlerSpline.SetTangentMode(0, ShapeTangentMode.Continuous);
				//_spCtrlerSpline.SetRightTangent(0, rightPos - rightCornerPoint);
				//_spCtrlerSpline.SetLeftTangent(0, (rightCornerPoint - rightPos).normalized *_capsuleRadius);

				_spCtrlerSpline.InsertPointAt(0, curPos);
				_spCtrlerSpline.SetTangentMode(0, ShapeTangentMode.Continuous);
				_spCtrlerSpline.SetRightTangent(0, tangentDir * _capsuleRadius * 0.1f);
				_spCtrlerSpline.SetLeftTangent(0, -tangentDir * _capsuleRadius * 0.1f);


				//Vector2 leftCornerPoint = curPos - tangentDir.normalized * _capsuleRadius;
				//_spCtrlerSpline.InsertPointAt(0, leftCornerPoint);
				//_spCtrlerSpline.SetTangentMode(0, ShapeTangentMode.Continuous);
				//_spCtrlerSpline.SetRightTangent(0, (leftCornerPoint - leftPos).normalized * _capsuleRadius);
				//_spCtrlerSpline.SetLeftTangent(0, leftPos - leftCornerPoint);

				//_spCtrler.spriteShape
			}
			else
			{
				// 首尾
				_spCtrlerSpline.InsertPointAt(0, curPos);
				_spCtrlerSpline.SetTangentMode(0, ShapeTangentMode.Broken);
			}
		}
	}

	// ----------------//
	// --- 私有成员
	// ----------------//
	private Spline _spCtrlerSpline { get => _spCtrler.spline; }
	private SpriteShapeController _spCtrler { get { if (__spCtrler == null) __spCtrler = GetComponent<SpriteShapeController>(); return __spCtrler; } }
	private SpriteShapeController __spCtrler;
	private float _capsuleRadius 
	{ 
		get 
		{
			if (__capsuleRadius.HasValue == false) __capsuleRadius = ColliderSections[0].GetComponent<CapsuleCollider2D>().size.y; 
			return __capsuleRadius.Value;
		} 
	}
	private float? __capsuleRadius;
	//private List<(Vector3, Quaternion)> _corners = new List<(Vector3, Quaternion)> (10);

	// ----------------//
	// --- Unity消息
	// ----------------//

	// ----------------//
	// --- 公有方法
	// ----------------//
	public void SetHighLightIndex(int? posIndex = null)
	{
		for (int i = 0; i < _spCtrlerSpline.GetPointCount(); i++)
		{
			_spCtrlerSpline.SetSpriteIndex(0, (posIndex != null && posIndex == i) ? 1 : 0);
		}
	}

	// ---------------- //
	// --- 私有方法
	// ---------------- //
	protected override void OnBeforeInit()
	{
		//_sections = new GameObject[10];
		//for (int i = 0; i < _sections.Length; i++)
		//{
		//	if (i == 0)
		//	{
		//		_sections[i] = _sectionOrigin;
		//	}
		//	else
		//	{ 
		//		_sections[i] = GameObject.Instantiate(_sectionOrigin, _sectionOrigin.transform.parent);
		//	}
		//	_sections[i].gameObject.SetActive(false);
		//}
	}

	private static bool ValidateCorners(List<(Vector3, Quaternion)> arr)
	{
		int pointCount = arr.Count;
		if (pointCount < 2)
		{
			return false;
		}

		for (int i = 0; i < pointCount - 1; i++)
		{
			if ((arr[i].Item1 - arr[i + 1].Item1).sqrMagnitude < 0.001f)
			{
				return false;
			}
		}

		return true;
	}

	// ----------------//
	// --- 类型
	// ----------------//

}
