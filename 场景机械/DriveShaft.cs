using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class DriveShaft: MonoBehaviour
{
	// ------------- //
	// -- 序列化
	// ------------- //
	[SerializeField]
	private bool _useAnimation;

	[HideIf("@_useAnimation")]
	[SerializeField]
	private Rigidbody2D ConnectA;
	[HideIf("@_useAnimation")]
	[SerializeField]
	private Rigidbody2D ConnectB;
	[HideIf("@_useAnimation")]
	[SerializeField]
	[Range(0.01f, 3)]
	private float _rotationScale = 1;

	[SerializeField]
	private List<Sprite> _rotationSprites;

	[SerializeField]
	[Tooltip("旋转列表所走过的角度")]
	private int _degressForRotation = 20;


	[SerializeField]
	[HideIf("@!_useAnimation")]
	[Tooltip("度每秒 d/s")]
	private float _playAnimationSpeed = 10;
	// ------------- //
	// -- 私有成员
	// ------------- //
	private HingeJoint2D _jointA;
	private HingeJoint2D _jointB;
	private SpriteRenderer _render;
	private int _degreeEachSprite;
	// ------------- //
	// -- 公有成员
	// ------------- //

	// ------------- //
	// -- Unity 消息
	// ------------- //
	private void Awake()
	{
		_render = GetComponent<SpriteRenderer>();
		_degreeEachSprite = _degressForRotation / _rotationSprites.Count;
		SetNow();
	}

	private void Update()
	{
		if (_useAnimation)
		{
			SetSpriteToRotate(Time.time * _playAnimationSpeed % 360);
		}
		else
		{
			if (_jointA)
			{
				SetSpriteToRotate(ConnectB.transform.eulerAngles.z * _rotationScale);
			}
			else if (_jointB)
			{ 
				SetSpriteToRotate(ConnectB.transform.eulerAngles.z * _rotationScale);
			}
		}
	}

	// ------------- //
	// -- 公有方法
	// ------------- //

	// ------------- //
	// -- 私有方法
	// ------------- //
	[Button("立即匹配")]
	private void SetNow()
	{
		if (ConnectA == null || ConnectB == null) return;
		bool AAtLeft = ConnectA.position.x < ConnectB.position.x;
		Vector3 leftAnchor = AAtLeft ? ConnectA.position : ConnectB.position;
		Vector3 rightAnchor = AAtLeft ? ConnectB.position : ConnectA.position;

		transform.position = (leftAnchor + rightAnchor) / 2;
		transform.right = rightAnchor - leftAnchor;
		// 暂时 手动调整宽度 以适配传动表现
		//var oldBounds = GetComponent<SpriteRenderer>().bounds;
		//float disConnectObject = Vector3.Distance(ConnectA.position, ConnectB.position);
		//GetComponent<SpriteRenderer>().bounds = new Bounds(transform.position, new Vector3(disConnectObject / 2, oldBounds.size.y));

		GameObject realShaft = transform.GetChild(0).gameObject;

		while (realShaft.TryGetComponent<HingeJoint2D>(out HingeJoint2D joint))
		{
			GameObject.DestroyImmediate(joint);
		}

		realShaft.GetComponent<Rigidbody2D>().freezeRotation = true;

		_jointA = realShaft.AddComponent<HingeJoint2D>();
		_jointB = realShaft.AddComponent<HingeJoint2D>();
		_jointA.connectedBody = ConnectA;
		_jointA.anchor = transform.InverseTransformPoint(ConnectA.transform.position + Vector3.up);
		_jointB.connectedBody = ConnectB;
		_jointB.anchor = transform.InverseTransformPoint(ConnectB.transform.position + Vector3.up);
	}

	private void SetSpriteToRotate(float angle)
	{ 
		_render.sprite = _rotationSprites[(int)(Mathf.Abs(angle % _degressForRotation) / _degreeEachSprite)];
	}

	private void OnDrawGizmosSelected()
	{
		if(ConnectA == null || ConnectB == null) return;
		Gizmos.DrawLine(ConnectA.position, ConnectB.position);
	}
}
