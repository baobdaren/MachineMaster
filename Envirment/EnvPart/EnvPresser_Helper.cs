using Sirenix.OdinInspector;
using UnityEngine;

public class EnvPresser_Helper: MonoBehaviour
{
	// ------------- //
	// -- 序列化
	// ------------- //
	[SerializeField]
	[ReadOnly]
	public GameObject EnvPresserBody;
	// ------------- //
	// -- 私有成员
	// ------------- //

	// ------------- //
	// -- 公有成员
	// ------------- //

	// ------------- //
	// -- Unity 消息
	// ------------- //

	// ------------- //
	// -- 公有方法
	// ------------- //
	[Button("主物体-自动朝向")]
	private void SetAutoLookAt()
	{
		EnvPresserBody.GetComponent<EnvPresser>().SetDirector();
	}

	// ------------- //
	// -- 私有方法
	// ------------- //

}
