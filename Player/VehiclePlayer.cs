using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using DG.Tweening;

public class VehiclePlayer : PlayerBase
{
	// ----------------//
	// --- 序列化
	// ----------------//
	[SerializeField]
	float SecondsTopSpeed = 0.8f;

	[SerializeField]
	private WheelJoint2D _leftWheelJoint;

	[SerializeField]
	private WheelJoint2D _rightWheelJoint;
	[SerializeField]
	private Transform _headCam;

	[SerializeField]
	private FixedJoint2D _safeJoint;
	// ----------------//
	// --- 公有成员
	// ----------------//
	//public Vector2? SaveablePosition { get; private set; } = null;
	public float SaveableAngle { get; private set; }
	public bool EnableControl { get; set; }
	// ----------------//
	// --- 私有成员
	// ----------------//
	private enum PlayerControlState { Forward, Stop, Back, Free }
	private PlayerControlState CurrentState
	{
		get => _curState;
		set 
		{
			if (value != _curState)
			{
				// 建议反向加速是翻倍加速时间
				if (value == PlayerControlState.Forward || value == PlayerControlState.Back)
				{
					_startAccTime = Time.time;
				}
			}
			_curState = value;
		}
	}
	private float GetAccelerate {
		get
		{
			float r = Mathf.Clamp01((Time.time - _startAccTime) / 1f / 4f * 3 + 0.25f);
			return r;
		}
	}
	private PlayerControlState _curState = PlayerControlState.Stop;
	private float _startAccTime = -1;
	private AircraftPlayer myAirPlayer;
	private Vector3 myAirPlayerDefaultPos;
	private int VehicleStage = 0;
	private Coroutine saveCor;
	private Coroutine _idleAnimation;

	// ----------------//
	// --- Unity消息
	// ----------------//
	private void Awake()
	{
		_leftWheelJoint.useMotor = true;
		_rightWheelJoint.useMotor = true;
		myAirPlayer = GetComponentInChildren<AircraftPlayer>(true);
		myAirPlayerDefaultPos = myAirPlayer.transform.localPosition;
		myAirPlayer.gameObject.SetActive(false);
		Physics2D.IgnoreCollision(_safeJoint.GetComponent<Collider2D>(), _leftWheelJoint.connectedBody.GetComponent<Collider2D>());
		Physics2D.IgnoreCollision(_safeJoint.GetComponent<Collider2D>(), _rightWheelJoint.connectedBody.GetComponent<Collider2D>());
		Physics2D.IgnoreCollision(GetComponent<Collider2D>(), _leftWheelJoint.connectedBody.GetComponent<Collider2D>());
		Physics2D.IgnoreCollision(GetComponent<Collider2D>(), _rightWheelJoint.connectedBody.GetComponent<Collider2D>());
	}
	private void OnEnable()
	{
		EnableControl = true;
		_idleAnimation = StartCoroutine(ProgressIdleAnimation());
	}

	private void OnDisable()
	{
		StopCoroutine(_idleAnimation);
	}

	private void FixedUpdate()
	{
		if (_safeJoint == null)
		{
			Debug.Log("玩家被破坏");
			GameObject.Destroy(gameObject);
			//Time.timeScale = 0.01f;
		}
	}

	protected void Update()
	{
		if (Keyboard.current.qKey.wasPressedThisFrame && CameraActor.Instance.CurrentWorkingStatue == CameraActor.CameraWorkStates.Follow)
		{
			bool activeAircraft = CameraActor.Instance.CurrentFollowTarget == transform;
			myAirPlayer.transform.localPosition = myAirPlayerDefaultPos;
			CameraActor.Instance.CurrentFollowTarget = activeAircraft ? myAirPlayer.transform : transform;
			EnableControl = !activeAircraft;
			myAirPlayer.gameObject.SetActive(activeAircraft);
		}

		if (Keyboard.current.spaceKey.wasPressedThisFrame)
		{
			VehicleStage++;
			VehicleStage %= 4;
		}


		if (EnableControl)
		{
			PlayerControlState state = GetInputState();
			CurrentState = state;
			switch (CurrentState)
			{
				case PlayerControlState.Forward:
					SetForward();
					break;
				case PlayerControlState.Stop:
					SetBrake();
					break;
				case PlayerControlState.Back:
					SetBack();
					break;
				default:
					SetFree();
					break;
			}
		}
		else
		{
			SetBrake();
		}
	}

	// ----------------//
	// --- 公有方法
	// ----------------//
	//public void SetSavedPostionAndRotation(Vector2 pos, float angle)
	//{
	//	transform.SetPositionAndRotation(pos, Quaternion.Euler(0, 0, angle));
	//}
	// ----------------//
	// --- 私有方法
	// ----------------//
	private void SetForward()
	{
		if (_leftWheelJoint)
		{
			_leftWheelJoint.motor = new JointMotor2D()
			{
				maxMotorTorque = GameConfig.Instance.TorqueWheel * GetAccelerate,
				motorSpeed = -GameConfig.Instance.SpeedWheel * (VehicleStage / 3f)
			};
		}
		if (_rightWheelJoint)
		{
			_rightWheelJoint.motor = new JointMotor2D()
			{
				maxMotorTorque = GameConfig.Instance.TorqueWheel * GetAccelerate,
				motorSpeed = -GameConfig.Instance.SpeedWheel * (VehicleStage / 3f)
			};
		}
	}

	private void SetBack()
	{
		if (_leftWheelJoint)
		{
			_leftWheelJoint.motor = new JointMotor2D()
			{
				maxMotorTorque = GameConfig.Instance.TorqueWheel * GetAccelerate,
				motorSpeed = GameConfig.Instance.SpeedWheel * (VehicleStage / 3f)
			};
		}
		if(_rightWheelJoint)
		{
			_rightWheelJoint.motor = new JointMotor2D()
			{
				maxMotorTorque = GameConfig.Instance.TorqueWheel * GetAccelerate,
				motorSpeed = GameConfig.Instance.SpeedWheel * (VehicleStage / 3f)
			};
		}
	}

	private void SetBrake()
	{
		if (_leftWheelJoint)
		{ 
			_leftWheelJoint.motor = new JointMotor2D()
			{
				maxMotorTorque = GameConfig.Instance.TorqueWheel,
				motorSpeed = 0
			};
		}
		if(_rightWheelJoint)
		{
			_rightWheelJoint.motor = new JointMotor2D()
			{
				maxMotorTorque = GameConfig.Instance.TorqueWheel,
				motorSpeed = 0
			};
		}
	}

	private void SetFree()
	{
		if (_leftWheelJoint)
		{
			_leftWheelJoint.motor = new JointMotor2D()
			{
				maxMotorTorque = 0.00001f,
				motorSpeed = GameConfig.Instance.SpeedWheel
			};
		}
		if (_rightWheelJoint)
		{
			_rightWheelJoint.motor = new JointMotor2D()
			{
				maxMotorTorque = 0.00001f,
				motorSpeed = GameConfig.Instance.SpeedWheel
			};
		}
	}

	private PlayerControlState GetInputState()
	{
		if (Keyboard.current.leftShiftKey.isPressed)
		{
			return PlayerControlState.Stop;
		}
		if (Keyboard.current.aKey.isPressed)
		{
			return PlayerControlState.Forward;
		}
		if (Keyboard.current.zKey.isPressed)
		{
			return PlayerControlState.Back;
		}
		return PlayerControlState.Free;
	}

	private IEnumerator WaitForSavePos()
	{
		Rigidbody2D playerRigid = GetComponent<Rigidbody2D>();
		while (true)
		{
			if (this == null) yield break;
			if (playerRigid.velocity.sqrMagnitude < 0.01f && playerRigid.angularVelocity < 3)
				break;
			else
			{
				GameMessage.Instance.PrintMessageAtScreenCenter("等待玩家禁止以保存位置");
				yield return new WaitForSeconds(1f);
			}
		}
		GameMessage.Instance.PrintMessageAtScreenCenter("保存位置", false);
		Collider2D spwanCollider = LevelProgressBase.Instance.CurrentEditZone.SpwanCollider;
		//SaveablePosition = spwanCollider.offset + (Vector2)spwanCollider.transform.position;
		SaveableAngle = spwanCollider.transform.eulerAngles.z;
		//Debug.Log("保存位置:" + SaveablePosition.Value);
	}

	private IEnumerator ProgressIdleAnimation()
	{
		bool lastDirIsWorldRight = true;
		while (gameObject)
		{
			// 朝向空旷或者鼠标
			//yield return new WaitForSeconds(1f);
			//Vector3? lookAtDir = null;
			//Vector3 mousePosToHead = Camera.main.ScreenToWorldPoint(Mouse.current.position.value) - _headCam.position;
			//if (Vector2.SqrMagnitude(mousePosToHead) < 100)
			//{// 鼠标距离近时,跟随鼠标
			//	float angle = Vector2.SignedAngle(_headCam.right, mousePosToHead);
			//	if (Mathf.Abs(angle) < 30f)
			//	{
			//		lookAtDir = mousePosToHead;
			//	}
			//}
			//else // 不跟随鼠标时,尝试朝向更广阔的事业(射线更远)
			//{
			//	float frontColliderDis = Physics2D.Raycast(_headCam.transform.position, _headCam.transform.right).distance;
			//	Debug.Log("正前方距离" + frontColliderDis);
			//	float lookUpColliderDis = Physics2D.Raycast(_headCam.transform.position, _headCam.transform.right + new Vector3(0,0.2f)).distance;
			//	if (lookUpColliderDis > frontColliderDis)
			//	{
			//		lookAtDir = _headCam.transform.right + new Vector3(0, 0.2f);
			//	}
			//}
			//if (lookAtDir.HasValue)
			//{
			//	_headCam.transform.DORotate(lookAtDir.Value, 2f);
			//}

			// 朝向水平
			yield return new WaitForSeconds(1.1f);
			//Debug.Log(Vector3.Angle(transform.right, Vector2.right) < 20);
			if (Vector3.Angle(transform.right, Vector2.right) < 16)
			{
				_headCam.transform.DORotateQuaternion(Quaternion.Euler(0,0,0), 1f);
				if(lastDirIsWorldRight == false) yield return new WaitForSeconds(1.1f);
				lastDirIsWorldRight = true;
			}
			else
			{
				_headCam.transform.DORotateQuaternion(transform.rotation, 1f);
				if(lastDirIsWorldRight == true) yield return new WaitForSeconds(1.1f);
				lastDirIsWorldRight = false;
			}
		}
	}
	// ----------------//
	// --- 类型
	// ----------------//
}
