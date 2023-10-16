using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class SimplePlatform : SerializedMonoBehaviour
{
	// ----------------//
	// --- 序列化
	// ----------------//
	[SerializeField]
	private SpriteRenderer BackGround;

	[SerializeField]
	private Sprite WaittingSprite;
	[SerializeField]
	private Sprite IdleSprite;
	[SerializeField]
	private Sprite RunnningSprite;

	[SerializeField]
	private PlayerTrigger _startStation;
	[SerializeField]
	private PlayerTrigger _endStation;

	[SerializeField]
	private Transform _startPlatform;
	[SerializeField]
	private Transform _endPlatform;
	[SerializeField]
	private Transform _workPlatform;

	[SerializeField]
	[Range(0,5)]
	private float _speed = 0.1f;
	//[SerializeField]
	//private float _brakeDis = 1f;

	[SerializeField]
	private PathType TestPathType;
	// ----------------//
	// --- 公有成员
	// ----------------//

	// ----------------//
	// --- 私有成员
	// ----------------//
	[ReadOnly]
	[SerializeField]
	private Vector3 _endPosition;
	[ReadOnly]
	[SerializeField]
	private Vector3 _startPosition;
	private bool _playerInside = false;
	private bool? _isMovingToEndPoint = null;
	private bool _moving = false;

	private Vector3 _startPoint;
	private Vector3 _moveVec;

	// ----------------//
	// --- Unity消息
	// ----------------//
	private void Awake()
	{
		AtuoFitNow();
		_startPoint = transform.position;
		_moveVec = _endPosition - _startPoint;
		_workPlatform.gameObject.SetActive(false);
	}

	private void Start()
	{
		GetComponentInChildren<PlayerTrigger>();
	}

	private void Update()
	{
		if (Keyboard.current.xKey.wasPressedThisFrame)
		{
			StartCoroutine(ProgressMoveingPlatform());
		}
	}

	// ----------------//
	// --- 公有方法
	// ----------------//

	// ----------------//
	// --- 私有方法
	// ----------------//
	private IEnumerator ProgressMoveingPlatform()
	{
		if(_moving == true) yield break;
		bool atStartStation = _startStation.TargetStayIn;
		if (atStartStation == false && _endStation.TargetStayIn == false) yield break;
		_moving = true;
		Vector3 beiginPos = atStartStation? _startPosition : _endPosition;
		Vector3 targetPos = atStartStation? _endPosition : _startPosition;
		Vector2 direction = (targetPos - _startPoint).normalized;
		_workPlatform.position = (atStartStation ? _startPlatform : _endPlatform).position;
		_workPlatform.gameObject.SetActive(true);
		_startPlatform.gameObject.SetActive(false);
		_endPlatform.gameObject.SetActive(false);
		Rigidbody2D platformRigid = _workPlatform.GetComponentInChildren<Rigidbody2D>();
		//float sqrDisSum = Vector3.SqrMagnitude(beiginPos - targetPos);
		Animation anim = _workPlatform.GetComponent<Animation>();
		anim.Play("可伸缩平台 反向");
		yield return new WaitUntil(() => anim.isPlaying == false);
		DG.Tweening.Core.TweenerCore<Vector3, DG.Tweening.Plugins.Core.PathCore.Path, DG.Tweening.Plugins.Options.PathOptions> op = DOTweenModulePhysics2D.DOPath(_workPlatform.GetComponent<Rigidbody2D>(), new Vector2[] { _workPlatform.transform.position, targetPos }, 15, TestPathType);
		yield return op.WaitForCompletion();
		//while (true)
		//{

		//	float sqrDisNow = Vector3.SqrMagnitude(targetPos - _workPlatform.position);
		//	float velocity = Mathf.Clamp(sqrDisNow, 0.0000001f, _speed * Time.fixedDeltaTime);
		//	if (sqrDisNow < 1)
		//	{ 
		//		velocity = sqrDisNow;
		//	}
		//	//platform.transform.position += direction * velocity;
		//	platformRigid.MovePosition(platformRigid.position + direction * velocity);
		//	yield return new WaitForFixedUpdate();
		//	if (Vector2.Distance(_workPlatform.position, targetPos) < 0.0001f )
		//	{
		//		yield return new WaitForFixedUpdate();
		//		_workPlatform.transform.position = targetPos;
		//		break;
		//	}
		//}
		anim.Play("可伸缩平台");
		yield return new WaitUntil(() => anim.isPlaying == false);
		_moving = false;
		_workPlatform.gameObject.SetActive(false);
		_startPlatform.gameObject.SetActive(true);
		_endPlatform.gameObject.SetActive(true);
	}

	[Button("立即适配")]
	private void AtuoFitNow()
	{
		float platformHeight = _workPlatform.GetComponent<SpriteRenderer>().sprite.bounds.size.y/2;
		// 可能还需要减去平台高度的一般才可以完美适应
		_endPosition = BackGround.transform.position + Vector3.down * (BackGround.bounds.extents.y - platformHeight);
		_startPosition = BackGround.transform.position + Vector3.up * (BackGround.bounds.extents.y - platformHeight);

		_startStation.transform.position = _startPosition;
		_endStation.transform.position = _endPosition;

		_startPlatform.transform.position = _startPosition;
		_endPlatform.transform.position = _endPosition;
		Debug.Log($"平台{gameObject.name}适配完成");
	}
	// ----------------//
	// --- 类型
	// ----------------//
}
