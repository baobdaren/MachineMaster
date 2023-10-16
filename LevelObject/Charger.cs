using TMPro;
using UnityEngine;

public class Charger: MonoBehaviour
{
	// ------------- //
	// -- 序列化
	// ------------- //
	[SerializeField]
	[Header("旋转测量目标")]
	private Transform _rotateObject;

	[SerializeField]
	private TextMeshProUGUI _textChargeInfo0;
	[SerializeField]
	private TextMeshProUGUI _textChargeInfo1;
	[SerializeField]
	private TextMeshProUGUI _textChargeInfo2;
	[SerializeField]
	private TextMeshProUGUI _textChargeInfo3;
	// ------------- //
	// -- 私有成员
	// ------------- //
	private const float ChargeTargetNum = 360 * 64;
	private Rigidbody2D _rotateObjectRigid;
	private int _lastValue = -1;

	// ------------- //
	// -- 公有成员
	// ------------- //

	// ------------- //
	// -- Unity 消息
	// ------------- //
	private void Start()
	{
		_rotateObjectRigid = _rotateObject.GetComponent<Rigidbody2D>();
	}

	private void FixedUpdate()
	{
		int chargeValue = (int)(Mathf.Abs(_rotateObjectRigid.rotation) / ChargeTargetNum * 100);
		if (_lastValue != chargeValue)
		{
			_lastValue = chargeValue;
			_textChargeInfo0.text = $"{Mathf.Clamp(chargeValue - 0, 0, 100)} %";
			_textChargeInfo1.text = $"{Mathf.Clamp(chargeValue - 100, 0, 100)} %";
			_textChargeInfo2.text = $"{Mathf.Clamp(chargeValue - 200, 0, 100)} %";
			_textChargeInfo3.text = $"{Mathf.Clamp(chargeValue - 300, 0, 100)} %";
		}
	}
	// ------------- //
	// -- 公有方法
	// ------------- //

	// ------------- //
	// -- 私有方法
	// ------------- //

}
