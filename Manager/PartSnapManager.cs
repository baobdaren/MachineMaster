using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 可吸附的鼠标类组件抽象类
/// 全是静态方法和成员，本来无需继承，但以防其他类注册可吸附对象列表，以继承的方式使用
/// </summary>
public class PartSnapManager
{
	// ----------------//
	// --- 序列化
	// ----------------//

	// ----------------//
	// --- 公有成员
	// ----------------//
	public static PartSnapManager Instance { get; private set; } = new PartSnapManager();

	public Dictionary<SnapableBase, ISettableShader> AllSnapableObjects { private set; get; } = new Dictionary<SnapableBase, ISettableShader>();
	public List<ISettableShader> SnapedParts { private set; get; } = new List<ISettableShader>(2);

	//public IPartSetShader SnapTouchedPart { private set; get; }
	// ----------------//
	// --- 私有成员
	// ----------------//
	private SnapCursor _snapCursor;
	private SnapCursor SnapCursor
	{
		get
		{
			if (_snapCursor == null)
			{
				_snapCursor = GameObject.Instantiate(GameConfig.Instance.SnapLineCursor).GetComponent<SnapCursor>();
				//_snapCursor.SnapLineX.GetComponent<SpriteRenderer>().sortingLayerID = RenderLayerManager.GetAdsortLineSortLayer;
				//_snapCursor.SnapLineY.GetComponent<SpriteRenderer>().sortingLayerID = RenderLayerManager.GetAdsortLineSortLayer;
				//_snapCursor.SnapTouchPointCircle.GetComponent<SpriteRenderer>().sortingLayerID = RenderLayerManager.GetAdsortLineSortLayer;
			}
			return _snapCursor;
		}
	}


	//private GameObject SnapLinesParent;
	//private GameObject[] SnapLines1
	//{
	//	get
	//	{
	//		if (_snapLines == null || _snapLines.Length != 2 || _snapLines[0] == null)
	//		{
	//			if (SnapLinesParent == null)
	//			{
	//				SnapLinesParent = new GameObject("SnapLines Parent");
	//			}
	//			_snapLines = new GameObject[2];
	//			for (int i = 0; i < 2; i++)
	//			{
	//				LineRenderer lr = new GameObject().AddComponent<LineRenderer>();
	//				lr.positionCount = 2;
	//				lr.colorGradient = new Gradient() { mode = GradientMode.Fixed, colorKeys = new GradientColorKey[] { new GradientColorKey(new Color(1, 0, 1), 0) } };
	//				lr.sortingLayerID = RenderLayerManager.GetAdsortLineSortLayer;
	//				lr.material = MaterialConfig.Instance.SpriteUnlit;
	//				_snapLines[i] = lr.gameObject;
	//				_snapLines[i].transform.SetParent(SnapLinesParent.transform);
	//			}
	//		}
	//		return _snapLines;
	//	}
	//}
	//private GameObject[] _snapLines = null;
	// ----------------//
	// --- Unity消息
	// ----------------//

	// ----------------//
	// --- 公有方法
	// ----------------//
	public void RegistSnapableTarget(SnapableBase ts, ISettableShader snapShader)
	{
		//Debug.LogWarning("注册可吸附对象" + ts.gameObject.name);
		if (!AllSnapableObjects.ContainsKey(ts))
		{
			//ts.gameObject.name += "已注册拖拽";
			AllSnapableObjects.Add(ts, snapShader);
		}
	}

	public void UnRegistSnapableTarget(SnapableBase ts)
	{
		//Debug.Log("从吸附列表移除" + ts.gameObject.name);
		if (AllSnapableObjects.ContainsKey(ts))
		{
			AllSnapableObjects.Remove(ts);
		}
	}

	/// <summary>
	/// 吸附效果的标准线
	/// </summary>
	/// <param name="offset"></param>
	/// <param name="ts"></param>
	public void DisplayX(Vector2 lineCenter)
	{
		// 判断显示吸附线
		SnapCursor.SnapLineX.gameObject.SetActive(true);
		SnapCursor.SnapLineX.position = lineCenter;
		SnapCursor.SnapLineX.gameObject.name = "显示0";
		return;
		//LineRenderer lr = SnapCursor.SnapLineX.GetComponent<LineRenderer>();
		//lr.startWidth = CameraActor.Instance.CameraViewSize / 800;
		//lr.SetPositions(new Vector3[] { new Vector3(lineCenter.x, lineCenter.y - 1, 0), new Vector3(lineCenter.x, lineCenter.y + 1, 0), });
	}

	public void DisplayY(Vector2 lineCenter)
	{
		SnapCursor.SnapLineY.gameObject.SetActive(true);
		SnapCursor.SnapLineY.position = lineCenter;
		SnapCursor.SnapLineY.gameObject.name = "显示1";
		return;
		//LineRenderer lr = SnapCursor.SnapLineY.GetComponent<LineRenderer>();
		//lr.startWidth = CameraActor.Instance.CameraViewSize / 800;
		//lr.SetPositions(new Vector3[] { new Vector3(lineCenter.x - 1, lineCenter.y, 0), new Vector3(lineCenter.x + 1, lineCenter.y, 0), });
	}

	public void DisplayCircle(Vector2 pos)
	{
		SnapCursor.SnapTouchPointCircle.gameObject.SetActive(true);
		SnapCursor.SnapTouchPointCircle.position = pos;
	}

	public void HideX()
	{
		SnapCursor.SnapLineX.gameObject.SetActive(false);
		SnapCursor.SnapLineX.gameObject.gameObject.name = "隐藏";

	}
	public void HideY()
	{
		SnapCursor.SnapLineY.gameObject.SetActive(false);
		SnapCursor.SnapLineY.gameObject.gameObject.name = "隐藏";
	}

	public void HideCircle()
	{
		SnapCursor.SnapTouchPointCircle.gameObject.SetActive(false);
	}

	// ----------------//
	// --- 私有方法
	// ----------------//
	private void UpdateSize()
	{ 
		
	}
	// ----------------//
	// --- 类型
	// ----------------//
}
