using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using System;
using System.Linq;

/// <summary>
/// 吸附
/// </summary>
public class SnapableBase : SerializedMonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
	// ----------------//
	// --- 序列化
	// ----------------//

	// ----------------//
	// --- 公有成员
	// ----------------//
	public bool IsSnapableTarget { get; set; } = true;
	/// <summary>
	/// 指定实际需要移动的目标，如为NULL，则使用当前Transform
	/// </summary>
	[SerializeField]
	[EnableIf("Dragable")]
	protected Transform _moveTarget;
	public Vector2 MovePosition { get => MoveTarget.transform.position; }
	// 吸附
	public virtual SnapBaseShapeTypes SnapBaseShapeType { get => SnapBaseShapeTypes.Point; }
	[HideIf("@!IsSnapableTarget || SnapBaseShapeType != SnapBaseShapeTypes.Gear")]
	public CircleCollider2D GearMiddleCircle;
	[HideIf("@!IsSnapableTarget || (SnapBaseShapeType != SnapBaseShapeTypes.Circle)")]
	public CircleCollider2D CircleBound;
	[HideIf("@!IsSnapableTarget || SnapBaseShapeType != SnapBaseShapeTypes.Box")]
	public BoxCollider2D BoxBound;
	[HideIf("@!IsSnapableTarget || SnapBaseShapeType != SnapBaseShapeTypes.Point")]
	public Transform Point;
	// 拖拽相关
	public bool IsDraging { private set; get; } = false;
	public Vector2 ConsoleReadOffset { get => dragScreenOffset; }
	public bool Dragable = true;
	public bool EnableSnapCenter;
	public bool EnableSnapBound;
	public class OnDragEvent : UnityEvent { }
	public class OnDragPosEvent : UnityEvent<Vector2> { }
	[HideInInspector]
	public OnDragPosEvent OnDraging = new OnDragPosEvent();
	[HideInInspector]
	public OnDragEvent OnDragStart = new OnDragEvent();
	[HideInInspector]
	public OnDragEvent OnDragEnd = new OnDragEvent();
	// ----------------//
	// --- 私有成员
	// ----------------//
	private Vector2 dragScreenOffset = Vector2.zero;
	private Transform MoveTarget
	{
		get
		{
			if (_moveTarget != null)
				return _moveTarget;
			return transform;
		}
	}
	private Vector2 GetOffsetThisFrame 
	{
		get => dragScreenOffset = Mouse.current.position.ReadValue() - (Vector2)CameraActor.Instance.MainCamera.WorldToScreenPoint(MoveTarget.position);
	}
	// ----------------//
	// --- Unity消息
	// ----------------//
	public void OnBeginDrag(PointerEventData eventData = null)
	{
		//Debug.LogError("Unity 拖拽消息 - " + name);
		//if (!enabled || !Dragable)
		//{
		//	Debug.LogWarning("Unity 拖拽消息 不能使用而结束 - " + name);
		//	return;
		//}
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			DragAction_Start(true);
		}
	}

	/// <summary>
	/// 拖拽
	/// </summary>
	/// <param name="eventData"></param>
	public void OnDrag(PointerEventData eventData)
	{
		DragAction_Draging();
	}

	public void OnEndDrag(PointerEventData eventData = null)
	{
		DragAction_Finish();
	}

	private void OnDisable()
	{
		//SnapManager.Instance.UnRegistSnapTarget(this);
	}

	private void Reset()
	{
		if (_moveTarget == null)
		{
			_moveTarget = transform;
		}
	}

	private void OnDrawGizmosSelected()
	{
		if (IsSnapableTarget == false) return;
		Color oldColor = Gizmos.color;
		Gizmos.color = new Color(0.3f, 1f, 1f, (Time.time % 1));
		switch (SnapBaseShapeType)
		{
			case SnapBaseShapeTypes.Circle:
				Gizmos.DrawWireSphere((Vector2)transform.position + CircleBound.offset, CircleBound.radius);
				break;
			case SnapBaseShapeTypes.Box:
				Gizmos.DrawWireCube((Vector2)transform.position + BoxBound.offset, BoxBound.size);
				break;
			case SnapBaseShapeTypes.Point:
				Gizmos.DrawLine((Vector2)Point.position, (Vector2)Point.position + new Vector2(0.08f, -0.08f));
				Gizmos.DrawLine((Vector2)Point.position, (Vector2)Point.position + new Vector2(-0.08f, -0.08f));
				Gizmos.DrawLine((Vector2)Point.position, (Vector2)Point.position + new Vector2(0, 0.08f));
				break;
			case SnapBaseShapeTypes.Gear:
				Gizmos.DrawWireSphere((Vector2)transform.position + GearMiddleCircle.offset, GearMiddleCircle.radius);
				break;
		}
		Gizmos.color = oldColor;
	}

	private void OnDestroy()
	{
		PartSnapManager.Instance.UnRegistSnapableTarget(this);
		OnDragEnd.RemoveAllListeners();
		OnDraging.RemoveAllListeners();
		OnDragStart.RemoveAllListeners();
	}
	// ----------------//
	// --- 公有方法
	// ----------------//
	/// <summary>
	/// 强制拖拽对象，忽略按下触发过程，直接使对象跟随已经按下的鼠标
	/// </summary>
	public void ForceDrag(bool musetKeepPress, bool keepOffset)
	{
		if (musetKeepPress && !Mouse.current.leftButton.isPressed) return;
			//if(keepOffset == false)
			//{
			//	dragScreenOffset = GetOffsetThisFrame;
			//}
			//else
			//{
			//	dragScreenOffset = Vector2.zero;
			//}
		StartCoroutine(ProgressForceDrag(musetKeepPress, keepOffset));
	}

	public void CancleDrag()
	{
		IsDraging = false;
	}
	// ----------------//
	// --- 私有方法
	// ----------------//
	protected virtual SnapVector SnapToCircles(List<SnapableBase> target, List<SnapableBase> snaped) { return SnapVector.Empty; }
	protected virtual SnapVector SnapToBoxs(List<SnapableBase> target, List<SnapableBase> snaped) { return SnapVector.Empty; }
	protected virtual SnapVector SnapToPoints(List<SnapableBase> target, List<SnapableBase> snaped) { return SnapVector.Empty; }
	protected virtual SnapVector SnapToGears(List<SnapableBase> targets, List<SnapableBase> snaped) { return SnapVector.Empty; }


	/// <summary>
	/// 开始拖拽处理
	/// 对于手动拖拽游戏对象，需要偏移记录抓取的点
	/// 对于强制拖拽
	/// </summary>
	/// <param name="useOffset"></param>
	private void DragAction_Start(bool useOffset)
	{
		//Debug.LogError("False Drag");
		IsDraging = true;
		OnDragStart?.Invoke();
		//Debug.LogError("Drag 进程 Start");
		if (useOffset)
		{
			dragScreenOffset = GetOffsetThisFrame;
		}
		else
		{
			dragScreenOffset = Vector2.zero;
		}
		//Debug.LogError("物体的屏幕坐标" + (Vector2)CameraActor.Instance.MainCamera.WorldToScreenPoint(MoveTransform.position));
	}

	/// <summary>
	/// 拖拽
	/// </summary>
	private void DragAction_Draging()
	{
		if (!enabled || !Dragable)
		{
			return;
		}
		if (IsDraging == false)
		{
			return;
		}
		Vector3 mouseScreenPos = Mouse.current.position.ReadValue() - dragScreenOffset;
		mouseScreenPos += (Vector3.forward * 10);
		MoveTarget.transform.position = CameraActor.Instance.MainCamera.ScreenToWorldPoint(mouseScreenPos);
		// 中心吸附和边界吸附的结果必须合并未一个向量
		// 最佳效果是：先吸附边界，如果可以中心吸附则进行完吸附中心后需再次吸附边界。？？？
		SnapVector minVecSnapCenter = SnapVector.Empty;
		foreach (KeyValuePair<SnapableBase, ISettableShader> itemSnapTo in PartSnapManager.Instance.AllSnapableObjects)
		{
			if (itemSnapTo.Key == this) continue;
			if(itemSnapTo.Key.IsSnapableTarget == false) continue;
			SnapVector resultSnapCenter;
			if (EnableSnapCenter && itemSnapTo.Key.EnableSnapCenter)
			{
				resultSnapCenter = new SnapVector(itemSnapTo.Key.MovePosition - MovePosition);
			}
			else
			{
				resultSnapCenter =  SnapVector.Empty;
			}
			minVecSnapCenter.TrySetMinValue(resultSnapCenter);
		}
		minVecSnapCenter.ClampCenterSnapVec(out bool snapX, out bool snapY);
		if (snapX || snapY)
		{
			MoveTarget.position += minVecSnapCenter.GetVector3;
			Vector2 snapLinePos = MoveTarget.position;
			if (snapX) PartSnapManager.Instance.DisplayX(snapLinePos); else PartSnapManager.Instance.HideX();
			if (snapY) PartSnapManager.Instance.DisplayY(snapLinePos); else PartSnapManager.Instance.HideY();
			//Debug.Log("中心可吸附");
		}
		else
		{
			//Debug.Log("中心可吸附 失败");
			PartSnapManager.Instance.HideX();
			PartSnapManager.Instance.HideY();
		}

		// 边界吸附
		//SnapVector minVecSnapBound = SnapVector.Empty;
		SnapableBase[] SnapFinalResultPart = new SnapableBase[2];
		var snapVec = SnapBound(PartSnapManager.Instance.AllSnapableObjects.Keys.ToList(), SnapFinalResultPart);
		PartSnapManager.Instance.SnapedParts.Clear();
		foreach (SnapableBase item in SnapFinalResultPart)
		{
			if (item == null) break;
			PartSnapManager.Instance.SnapedParts.Add(PartSnapManager.Instance.AllSnapableObjects[item]);
		}
		//foreach (var itemSnapTo in SnapManager.Instance.AllSnapableObjects)
		//{
		//	if (itemSnapTo.Key == this) continue;
		//	SnapVector resultSnapBound = SnapBound(itemSnapTo.Key);
		//	resultSnapBound.ClampValue(0.02f, 0.02f);
		//	if (minVecSnapBound.TrySetMinDisValue_Bind(resultSnapBound) || bestSnapTouchedTarget == null)
		//	{
		//		bestSnapTouchedTarget = itemSnapTo.Value;
		//	}
		//}
		if (snapVec.HasAnyValue)
		{
			//Debug.LogError($"边界吸附有值{snapVec.GetVector3}, 吸附对象" + PartSnapManager.Instance.SnapedParts.Count);
			MoveTarget.position += snapVec.GetVector3;
			//SnapedParts = bestSnapTouchedTarget;
		}
		else
		{
			//Debug.LogError($"边界吸附有清除");
			PartSnapManager.Instance.SnapedParts.Clear();
			PartSnapManager.Instance.HideCircle();
		}
		OnDraging?.Invoke(MoveTarget.position);
	}

	private void DragAction_Finish()
	{
		OnDragEnd?.Invoke();
		Cursor.visible = true;
		PartSnapManager.Instance.HideX();
		PartSnapManager.Instance.HideY();
		PartSnapManager.Instance.HideCircle();
		PartSnapManager.Instance.SnapedParts.Clear();
		//Debug.LogError("finish Drag");
		IsDraging = false;
	}


	private IEnumerator ProgressForceDrag(bool mustKeepPress, bool useOffset)
	{
		// 修改Offset的设置
		DragAction_Start(useOffset);
		//transform.position = CameraActor.Instance.MouseWorldPos;
		//Debug.LogError("开始 强制拖拽 必须保持按下？" + mustKeepPress);
		//while (Mouse.current.leftButton.isPressed)
		while (IsDraging)
		{
			if (mustKeepPress && !Mouse.current.leftButton.isPressed) break;
			if (Keyboard.current.escapeKey.isPressed) break;
			// 在不安下鼠标左键就可以拖拽的情况下，再按下鼠标左键是否结束拖拽？
			if (!mustKeepPress && Mouse.current.leftButton.isPressed) break;
			DragAction_Draging();
			yield return 0;
		}
		//Debug.LogError("跳出强制拖拽");
		DragAction_Finish();
	}

	protected SnapVector SnapBound(List<SnapableBase> snapableObjects, SnapableBase[] snaped)
	{
		if (EnableSnapBound == false) return SnapVector.Empty;
		snapableObjects.RemoveAll((SnapableBase itemSnap) => { return itemSnap.IsSnapableTarget == false; });
		//Debug.LogError("预吸附边界对象数量" + snapableObjects.Count);
		SnapVector snapBound = SnapVector.Empty;
		Dictionary<SnapableBase, float> snapedTargets = new Dictionary<SnapableBase, float>(2);

		//SnapableBase[] snapFinalResult = new SnapableBase[2];
		List<SnapableBase> snapedResult = new List<SnapableBase>(2);
		List<SnapableBase> snapTargets = snapableObjects.FindAll(a => a.EnableSnapBound && a is SnapableCircle && a != this);
		if (snapBound.TrySetMinDisValue_Bind(SnapToCircles(snapTargets, snapedResult)))
		{
			snapedResult.CopyTo(snaped);
			snapedResult.Clear();
		}
		snapTargets = snapableObjects.FindAll(a => a.EnableSnapBound && a is SnapableBox && a != this);
		if (snapBound.TrySetMinDisValue_Bind(SnapToBoxs(snapTargets, snapedResult))) 
		{ 
			snapedResult.CopyTo(snaped); 
			snapedResult.Clear(); 
		}
 		snapTargets = snapableObjects.FindAll(a => a.EnableSnapBound && a is SnapableGear && a != this);
		if (snapBound.TrySetMinDisValue_Bind(SnapToGears(snapTargets, snapedResult))) 
		{ 
			snapedResult.CopyTo(snaped); 
			snapedResult.Clear();
		}
		snapTargets = snapableObjects.FindAll(a => a.EnableSnapBound && a is SnapablePoint && a != this);
		if (snapBound.TrySetMinDisValue_Bind(SnapToPoints(snapTargets, snapedResult))) 
		{ 
			snapedResult.CopyTo(snaped);
			snapedResult.Clear(); 
		}
		return snapBound;
	}

	//protected static Vector3 SnapCircleAndCircle(Vector2 masterPos, float masterRadius, Vector2 targetPos, float targetRadius)
	//{
	//	Vector2 adsorbVec = targetPos - masterPos;
	//	return adsorbVec - adsorbVec.normalized * (masterRadius + targetRadius);
	//}

	protected static SnapVector SnapBoxAndBox(BoxCollider2D masterBox, BoxCollider2D targetBox)
	{
		throw new Exception("没有实现盒子之间的吸附");
		//return SnapVector.Empty;
		//Vector2 moveDir; float moveDis;
		//float angle = Vector2.SignedAngle(masterBox.transform.right, masterBox.transform.position - targetBox.transform.position);
		//if (angle % 90 != 0)
		//{
		//	return null;
		//}
		//moveDis = Mathf.Abs(Vector2.Distance(targetBox.transform.position, masterBox.transform.position) * Mathf.Sin(Mathf.Abs(angle) * Mathf.Deg2Rad));
		//if (angle % 180 == 0)
		//{

		//}


		////moveDis -= masterBox.radius + targetBox.radius;
		//if (moveDis == 0)
		//{
		//	moveDir = Vector2.zero;
		//}
		//else
		//{
		//	Matrix4x4 rotateMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 0, (angle > 0 ? 1 : -1) * (90 - Mathf.Abs(angle))));
		//	moveDir = rotateMatrix.MultiplyVector(targetBox.transform.position - masterBox.transform.position).normalized;
		//}
		//if (!masterAdsorbTarget)
		//{
		//	moveDir *= -1;
		//}
		//return moveDir * moveDis;
	}

	protected static SnapVector SnapCircleAndBox(CircleCollider2D circle, BoxCollider2D box)
	{
		Vector2 moveDir; float moveDis;
		float angle = Vector2.SignedAngle(box.transform.right, box.transform.position - circle.transform.position);
		moveDis = Mathf.Abs(Vector2.Distance(box.transform.position, circle.transform.position) * Mathf.Sin(Mathf.Abs(angle) * Mathf.Deg2Rad));
		moveDis -= box.size.y / 2 + circle.radius;
		if (moveDis == 0)
		{
			moveDir = Vector2.zero;
		}
		else
		{
			Matrix4x4 rotateMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 0, (angle > 0 ? 1 : -1) * (90 - Mathf.Abs(angle))));
			moveDir = rotateMatrix.MultiplyVector(box.transform.position - circle.transform.position).normalized;
		}
		return new SnapVector(moveDir);
	}

	//	protected static SnapVector SnapCircleAndCircle(CircleCollider2D masterCircle, CircleCollider2D targetCircle)
	//	{
	//		Vector2 masterCenter = (Vector2)masterCircle.transform.position + masterCircle.offset;
	//		Vector2 targetCenter = (Vector2)targetCircle.transform.position + targetCircle.offset;
	//#if UNITY_EDITOR
	//		Debug.Assert(masterCircle.transform.lossyScale.x == masterCircle.transform.lossyScale.y, "圆的吸附必须保证xy缩放相同");
	//		Debug.Assert(targetCircle.transform.lossyScale.x == targetCircle.transform.lossyScale.y, "圆的吸附必须保证xy缩放相同");
	//#endif
	//		float masterRadius = masterCircle.radius * masterCircle.transform.lossyScale.x;
	//		float targetRadius = targetCircle.radius * targetCircle.transform.lossyScale.x;
	//		const float keepDis = 0.000f; // 吸附不能没有间距，导致齿轮无法转动
	//		SnapVector snapResult = new SnapVector((targetCenter - masterCenter).normalized * ((targetCenter - masterCenter).magnitude - masterRadius - targetRadius - keepDis));
	//		//snapResult - new SnapVector()
	//		return snapResult;
	//	}

	protected static SnapVector SnapCircleAndCircle(CircleCollider2D mainCir, CircleCollider2D circleA, CircleCollider2D circleB = null)
	{
		if (circleB == null)
		{
			Vector3 snapResultVec = circleA.transform.position - mainCir.transform.position;
			return new SnapVector(snapResultVec.normalized * (snapResultVec.magnitude - mainCir.radius - circleA.radius));
		}
		float r0 = mainCir.radius + circleB.radius;
		float r1 = mainCir.radius + circleA.radius;
		float rc = Vector3.Distance(circleA.transform.position, circleB.transform.position);
		float Degree0 = Mathf.Acos((rc * rc + r1 * r1 - r0 * r0) / 2 / rc / r1) * 180 / Mathf.PI;
		if (float.IsNaN(Degree0))
		{
			Debug.Assert(false);
			return SnapCircleAndCircle(mainCir, circleA);
		}
		Vector3 vec0To1 = (circleB.transform.position - circleA.transform.position).normalized;
		Quaternion qRoatet = Quaternion.AngleAxis(Degree0, Vector3.forward);
		Quaternion qRoatet2 = Quaternion.AngleAxis(-Degree0, Vector3.forward);
		Vector3 rotateResult = (qRoatet * vec0To1).normalized;
		Vector3 rotateResult2 = (qRoatet2 * vec0To1).normalized;
		Vector3 rPos1 = circleA.transform.position + r1 * rotateResult - mainCir.transform.position;
		Vector3 rPos2 = circleA.transform.position + r1 * rotateResult2 - -mainCir.transform.position;
		return new SnapVector(Vector3.SqrMagnitude(rPos1) < Vector3.SqrMagnitude(rPos2) ? rPos1 : rPos2);
	}

	protected static SnapVector SnapPointAndCircle(Vector2 point, CircleCollider2D targetCircle)
	{
		Vector2 vecDis = (Vector2)targetCircle.transform.position + targetCircle.offset - point;
		return new SnapVector((vecDis.magnitude - targetCircle.radius) * vecDis.normalized);
	}

	protected static SnapVector SnapPointAndPoint(Vector2 main, Vector2 target, out Vector2 snapTouchPoint)
	{
		snapTouchPoint = target;
		return new SnapVector(target - main);
	}
	// ----------------//
	// --- 类型
	// ----------------//
	public enum SnapBaseShapeTypes
	{
		Circle, Box, Point, Gear
	}

	public struct SnapVector
	{
		public static readonly SnapVector Empty = new SnapVector(null, null);

		public float? x;
		public float? y;

		public bool HasAnyValue => x.HasValue || y.HasValue;

		public SnapVector(float? xValue = null, float? yValue = null) { x = xValue; y = yValue; }
		public SnapVector(Vector2 vec) { x = vec.x; y = vec.y; }

		public Vector3 GetVector3 => new Vector3(x.HasValue ? x.Value : 0, y.HasValue ? y.Value : 0, 0);

		/// <summary>
		/// 删除在范围之外的数值
		/// </summary>
		/// <param name="xRange"></param>
		/// <param name="yRange"></param>
		public void ClampValue(float xRange, float yRange)
		{
			if (x.HasValue)
			{
				x = (x >= -xRange && x <= yRange) ? x : null;
			}
			if (y.HasValue)
			{
				y = (y >= -yRange && y <= yRange) ? y : null;
			}
		}

		public void ClampCenterSnapVec(out bool snapX, out bool snapY)
		{
			if (HasAnyValue == false)
			{
				x = null;
				y = null;
				snapX = false;
				snapY = false;
				return;
			}
			float xValue = x.Value;
			float yValue = y.Value;
			// 钳制x时，必须保证y轴的距离不能太远
			if (Mathf.Abs(xValue) > 0.03f || Mathf.Abs(yValue) > 1f)
			{
				snapX = false;
				x = null;
			}
			else
			{
				snapX = true;
			}
			if (Mathf.Abs(yValue) > 0.03f || Mathf.Abs(xValue) > 1f)
			{
				snapY = false;
				y = null;
			}
			else
			{
				snapY = true;
			}
		}

		/// <summary>
		/// 尝试取出参数的最小值并设定为自身的值
		/// </summary>
		/// <param name="other">比较对象</param>
		/// <param name="bindXY">XY是否同时吸附</param>
		public void TrySetMinValue(SnapVector other)
		{
			if (!x.HasValue)
			{
				x = other.x;
			}
			else if (other.x.HasValue && Mathf.Abs(other.x.Value) < Mathf.Abs(x.Value))
			{
				x = other.x;
			}
			if (!y.HasValue)
			{
				y = other.y;
			}
			else if (other.y.HasValue && Mathf.Abs(other.y.Value) < Mathf.Abs(y.Value))
			{
				y = other.y;
			}
		}

		/// <summary>
		/// 绑定同时设置xy最小值
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
		public bool TrySetMinDisValue_Bind(SnapVector other)
		{
			if (!other.x.HasValue || !other.y.HasValue)
			{
				return false;
			}
			if (x.HasValue && y.HasValue)
			{
				if (other.GetVector3.sqrMagnitude > GetVector3.normalized.sqrMagnitude)
				{
					return false;
				}
			}
			x = other.x;
			y = other.y;
			return true;
		}

		public bool TryGetSqrDistance(out float dis)
		{
			if (x.HasValue == false && x.HasValue == false)
			{
				dis = -1;
				return false;
			}
			else
			{
				dis = (x.HasValue ? (x * x) : 0).Value + (y.HasValue ? (y * y) : 0).Value;
				return true;
			}
		}

		public SnapVector Reverse()
		{
			if (this.x.HasValue)
			{
				this.x = -this.x.Value;
			}
			if (this.y.HasValue)
			{
				this.y = -this.y.Value;
			}
			return this;
		}
	}
}
