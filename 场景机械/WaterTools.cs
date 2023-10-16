using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(EdgeCollider2D))]
public class WaterTools: MonoBehaviour
{
	// ------------- //
	// -- 序列化
	// ------------- //
	[SerializeField]
	[Range(-1, 1)]
	private float TestVelocity = 1;
	[SerializeField]
	[Range(1, 20)]
	private int TestFrameDivide = 1;
	[SerializeField]
	[Range(0.001f, 1)]
	private float Tension = 0.025f;
	[SerializeField]
	[Range(0.01f, 1)]
	private float Transfer = 0.507f;
	[SerializeField]
	[Range(0.01f, 1)]
	private float Dampon = 0.568f;
	[SerializeField]
	[Range(0.01f, 1f)]
	private float BackCoolRatio = 0.472f;
	// ------------- //
	// -- 私有成员
	// ------------- //
	private int _waveColumnCount;
	private float _waveVertDistance;
	private Bounds _bounds;
	private Vector3[] _uvs;
	private MeshFilter _filter;
	private Vector3[] _vertexs;
	private WaveColumnData[] _waveColumnDatas;
	//private List<WavePushPointData> _wavePushPointDatas = new List<WavePushPointData>();
	// ------------- //
	// -- 公有成员
	// ------------- //

	// ------------- //
	// -- Unity 消息
	// ------------- //
	private void Awake()
	{
		_filter = GetComponent<MeshFilter>();
		_bounds = GetComponent<MeshRenderer>().bounds;
		_waveColumnCount = _filter.mesh.vertexCount / 2;
		_waveVertDistance = Mathf.Abs(_filter.mesh.vertices[0].x - _filter.mesh.vertices[1].x);
		_vertexs = _filter.mesh.vertices;
		_uvs = new Vector3[_vertexs.Length];
		_waveColumnDatas = new WaveColumnData[_waveColumnCount];
		for (int i = 0; i < _waveColumnCount; i++)
		{
			_waveColumnDatas[i] = (new WaveColumnData(i, _vertexs[i].y, _vertexs[i].x));
		}
		SetMeshRenderLayerID();
		transform.GetChild(0).gameObject.SetActive(true);
	}

	private void Start()
	{
		StartCoroutine(ProgressCreateMesh());
	}

	private void Update()
	{
		if (Keyboard.current.fKey.wasPressedThisFrame)
		{
			AddWave();
		}
		if (Time.frameCount % TestFrameDivide != 0)
		{
			return;
		}
		for (int i = 0; i < _waveColumnDatas.Length; i++)
		{
			float leftDelta = (i == 0) ? (0) : (Transfer * (_waveColumnDatas[i - 1].CurrentHeight - _waveColumnDatas[i].CurrentHeight));
			float rightDelta = (i == _waveColumnDatas.Length - 1) ?(0) : (Transfer * BackCoolRatio * (_waveColumnDatas[i+1].CurrentHeight - _waveColumnDatas[i].CurrentHeight));
			float force = leftDelta;
			force += rightDelta;
			force += Tension * (_waveColumnDatas[i].BaseHeight - _waveColumnDatas[i].CurrentHeight);
			_waveColumnDatas[i].Velocity = Dampon * (_waveColumnDatas[i].Velocity + force);
			_waveColumnDatas[i].CurrentHeight += _waveColumnDatas[i].Velocity;
		}
		for (int i = 0; i < _waveColumnDatas.Length; i++)
		{
			_vertexs[_waveColumnDatas[i].VertexIndex].y = _waveColumnDatas[i].CurrentHeight;
		}
		
		UpdateWavePointPos();
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.collider.isTrigger == false && collision.rigidbody.bodyType != RigidbodyType2D.Static)
		{
			AddWave(collision.contacts[0].point, collision.relativeVelocity.magnitude);
		}
	}

	// ------------- //
	// -- 公有方法
	// ------------- //
	[Button("增加波浪")]
	public void AddWave()
	{
		AddWave(transform.position, TestVelocity);
	}

	public void AddWave(Vector2 pos, float velocity, int expandCount = 1) 
	{
		int pushVertIndex = (int)((pos.x - _waveColumnDatas[0].XPosition) / _waveVertDistance);
		pushVertIndex = Mathf.Clamp(pushVertIndex, 0, _waveColumnDatas.Length);
		Debug.Log($"触发破浪 位置{pushVertIndex}");

		_waveColumnDatas[pushVertIndex].Velocity = velocity;
		if (pushVertIndex > 0) _waveColumnDatas[pushVertIndex-1].Velocity = velocity;
		if (pushVertIndex < _waveColumnDatas.Length-1) _waveColumnDatas[pushVertIndex+1].Velocity = velocity;
	}

	// ------------- //
	// -- 私有方法
	// ------------- //
	[Button("设置MeshRender SortingLayer")]
	private void SetMeshRenderLayerID()
	{
		GetComponent<MeshRenderer>().sortingLayerID = SortingLayer.NameToID("Water");
	}

	private void SetPointsBalance()
	{ 
		EdgeCollider2D edge = GetComponent<EdgeCollider2D>();
		List<Vector2> points = new List<Vector2>(edge.points);
		if (edge.points[0].y != edge.points[edge.pointCount - 1].y)
		{
			points[points.Count - 1] = new Vector3(points[points.Count - 1].x, points[0].y);
			Debug.LogError("当前只能处理水平面,已自动归0");
		}
		if (points[0].y != 0)
		{
			Vector2 offsetBalance = new Vector2(-(points[points.Count-1].x + points[0].x)/2, -points[0].y);
			for (int i = 0; i < points.Count; i++)
			{
				points[i] += offsetBalance;
			}
			Debug.LogError("已自动保持Edge相对坐标为0");
			transform.position -= (Vector3)offsetBalance;
		}
		edge.SetPoints(points);

	}

	[Button("重置波浪")]
	private void ResetWave()
	{
		for (int i = 0; i < _waveColumnDatas.Length; i++)
		{
			_waveColumnDatas[i].CurrentHeight = _waveColumnDatas[i].BaseHeight;
			_waveColumnDatas[i].Velocity = 0;
		}
	}

	[Button("创建 Mesh")]
	[Tooltip("依据EdgeCollider2D创建一个任意多边形水体,EdgeCollider2D的起点和终点为水面,所以做好保持这两点水平(不能使用凹面).")]
	private void CreateMesh()
	{
		StartCoroutine(ProgressCreateMesh());
	}

	private IEnumerator ProgressCreateMesh()
	{
		SetPointsBalance();
		yield return null;
		EdgeCollider2D edge = GetComponent<EdgeCollider2D>();
		Vector2 waveFaceDir = edge.points[edge.pointCount - 1] - edge.points[0];
		float boundWidth = waveFaceDir.magnitude;
		waveFaceDir = waveFaceDir.normalized;
		_waveColumnCount = Mathf.FloorToInt(boundWidth / 0.1f);
		Debug.Log("水体表面顶点数" +  _waveColumnCount);
		float wavePointDistance = boundWidth / (_waveColumnCount-1);

		Vector3[] vertexs = new Vector3[_waveColumnCount * 2];
		for (int i = 0; i < _waveColumnCount; i++)
		{
			vertexs[i] = i * wavePointDistance * waveFaceDir + edge.points[0];
			vertexs[i+_waveColumnCount] = GetCrossOverPointVertical(edge, vertexs[i]);
		}

		int triangleIndex = -1;
		int[] triangles = new int[(_waveColumnCount - 1) * 6];
		for (int i = 0; i < _waveColumnCount - 1; i++)
		{
			triangles[++triangleIndex] = i;
			triangles[++triangleIndex] = i + 1;
			triangles[++triangleIndex] = i + _waveColumnCount;
			triangles[++triangleIndex] = i + 1;
			triangles[++triangleIndex] = i + _waveColumnCount;
			triangles[++triangleIndex] = i + _waveColumnCount + 1;
		}
		Debug.Log("水体顶点数共计" +  vertexs.Length);
		Mesh mesh = new Mesh();
		mesh.name = "自定义水体网格";
		mesh.SetVertices(vertexs);

		// 设置UV
		MeshRenderer mr = GetComponent<MeshRenderer>();
		mesh.RecalculateBounds();
		Bounds mrBounds = mr.bounds;
		Vector2[] uvs = new Vector2[vertexs.Length];
		for (int i = 0; i < vertexs.Length; i++)
		{
			uvs[i] = new Vector2((vertexs[i].x) / mrBounds.size.x + 0.5f, vertexs[i].y / mrBounds.size.y);
		}

		mesh.SetUVs(0, uvs);
		mesh.SetTriangles(triangles, 0);
		mesh.UploadMeshData(false);

		GetComponent<MeshFilter>().sharedMesh = mesh;
		Debug.Log("创建结果,顶点数:" + GetComponent<MeshFilter>().sharedMesh.vertexCount);

		GetComponentInChildren<PolygonCollider2D>(true).SetPath(0, edge.points);
		var l = GetComponentInChildren<PolygonCollider2D>(true).points.Length;
		Debug.Log("设置后 路径数" + l);
		//GetComponentInChildren<PolygonCollider2D>(true).
		//BaseVertPosition = new Vector3[_waveColumnCount];
		//for (int i = 0;i < _waveColumnCount;i++) { BaseVertPosition[i] = vertexs[i]; }
	}

	private void UpdateWavePointPos()
	{
		for (int i = 0; i < _vertexs.Length; i++)
		{
			_uvs[i] = new Vector2((_vertexs[i].x) / _bounds.size.x + 0.5f, _vertexs[i].y / _bounds.size.y);
		}
		_filter.mesh.SetUVs(0, _uvs);
		_filter.mesh.vertices = _vertexs;
		_filter.mesh.UploadMeshData(false);
	}

	private Vector2 GetCrossOverPointVertical(EdgeCollider2D edge, Vector2 waveFacePoint)
	{
		Vector2[] edgePoints = edge.points;
		Vector2 waveDir = edgePoints[edgePoints.Length-1] - edgePoints[0];
		for (int i = 1; i < edgePoints.Length; i++)
		{
			Vector2 leftPos = edgePoints[i].x < edgePoints[i - 1].x ? edgePoints[i] : edgePoints[i - 1];
			Vector2 rightPos = edgePoints[i].x < edgePoints[i-1].x ? edgePoints[i-1] : edgePoints[i];
			if (waveFacePoint.x < leftPos.x || waveFacePoint.x > rightPos.x)
			{
				if (i + 1 >= edgePoints.Length)
				{
					Debug.Assert(false, $"该点({waveFacePoint})的垂线无法和EdgeCollider2D相交");
				}
				continue;
			}
			float y = (waveFacePoint.x - leftPos.x) * (Mathf.Tan(Vector2.SignedAngle(waveDir, rightPos - leftPos) * Mathf.Deg2Rad));
			//Debug.Assert(false, $"该点({waveFacePoint})的相交点({waveFacePoint.x}, {-y})");
			return new Vector2(waveFacePoint.x, y + leftPos.y);
		}
		return waveFacePoint;
	}

	// ------------- //
	// -- 类型
	// ------------- //
	//private struct WavePushPointData
	//{
	//	public WavePushPointData(int pushPos, float pushDeepth, float keepTime)
	//	{
	//		PushPos = pushPos;	
	//		PushDeepth = pushDeepth;
	//		_startTime = Time.time;
	//		_endTime = _startTime + keepTime;
	//	}
	//	public int PushPos;
	//	public float PushDeepth;
	//	public float TotalTime => Time.time - _startTime;
	//	public float RemainTime => Mathf.Clamp(_endTime - Time.time, 0, float.MaxValue);

	//	private readonly float _endTime;
	//	private readonly float _startTime;
	//}

	private struct WaveColumnData
	{
		public WaveColumnData(int vertextIndex, float baseHeight, float xPosition)
		{
			CurrentHeight = 0;
			Velocity = 0;
			VertexIndex = vertextIndex;
			BaseHeight = baseHeight;
			XPosition = xPosition;
		}
		public float CurrentHeight;
		public float Velocity;
		public int VertexIndex;
		public float BaseHeight;
		public readonly float XPosition;
		public Vector2 Pos => new Vector2(XPosition, CurrentHeight + BaseHeight);
	}
}
