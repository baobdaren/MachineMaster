using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceSensor : BaseSensor
{
	// ---------------- //
	// -- 序列化
	// ---------------- //
	private Transform directTransform { get => (Accessor as DistanceSensorAccessor)._directGo; }


	// ---------------- //
	// -- 私有成员
	// ---------------- //
	private LineRenderer CastLine { get => (Accessor as DistanceSensorAccessor)._lineR; }
	private RaycastHit2D[] castData;
	private bool _needUpdateCastData = true;
	private float _dectedDistance = float.MaxValue;
	// ---------------- //
	// --- 共有成员
	// ---------------- //
	/// <summary>
	/// 获取检测距离
	/// </summary>
	public float DectedDistance
	{
		get
		{
			if (_needUpdateCastData)
			{
				#region 旧版
				//Debug.LogError("刷新数据距离");
				//int dectedCount;
				//do
				//{ // 使用不重新申请内存的方式检测，如果检测结果等于数组长度，则长度*2再检测。
				//	dectedCount = Physics2D.RaycastNonAlloc(directTransform.position, directTransform.TransformDirection(Vector2.right), castData, float.MaxValue);
				//	if (dectedCount == castData.Length)
				//	{
				//		castData = new RaycastHit2D[castData.Length * 2];
				//		continue;
				//	}
				//} while (false);
				//_needUpdateCastData = false;
				//if (dectedCount == 0)
				//{
				//	_dectedDistance = float.MaxValue;
				//}
				//RaycastHit2D? nearestCastHit = null;
				//for (int i = 1; i < dectedCount; i++)
				//{
				//	if (!_selfColliders.Contains(castData[i].collider))
				//	{

				//		if (!nearestCastHit.HasValue || castData[i].distance < nearestCastHit.Value.distance)
				//		{
				//			nearestCastHit = castData[i];
				//		}
				//	}
				//}

				//_dectedDistance = (nearestCastHit.HasValue) ? (nearestCastHit.Value.distance) : (float.MaxValue);
				#endregion
				_dectedDistance = 5;
				castData = Physics2D.RaycastAll(directTransform.position, directTransform.TransformDirection(Vector2.right), 5f);
				for (int i = 0; i < castData.Length; i++)
				{
					if (Accessor.AllColliders.Contains(castData[i].collider) )
					{
						continue;
					}
					if (_dectedDistance > castData[i].distance)
					{
						_dectedDistance = castData[i].distance;
					}
				}
				_needUpdateCastData = false;
			}
			//Debug.LogError("获取距离为：" + _dectedDistance);
			return _dectedDistance;
		}
	}

	// ---------------- //
	// -- 私有方法
	// ---------------- //


	// ---------------- //
	///// Unity消息
	// ---------------- //
	private void FixedUpdate()
	{
		_needUpdateCastData = true;
	}
	private void Update()
	{
		CastLine.positionCount = 2;
		(Accessor as DistanceSensorAccessor)._tex.SetText(DectedDistance.ToString("F4"));
		CastLine.SetPositions(new Vector3[] { directTransform.position, directTransform.TransformPoint(Vector3.right * Mathf.Clamp(DectedDistance, 0, 1)) });
		//CastLine.SetPositions(new Vector3[] { directTransform.position, directTransform.TransformPoint(Vector3.right * 6.66f) });
	}




	// ---------------- //
	// -- 公有方法
	// ---------------- //
}