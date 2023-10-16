using UnityEngine;

/// <summary>
/// 暂时没有使用
/// 通过shader调整，是弹簧有微笑的压缩效果
/// </summary>

public class SpringCtrl : MonoBehaviour
{
	// ------------------ //    
	// --- 序列化    
	// ------------------ //

	// ------------------ //    
	// --- 公有成员    
	// ------------------ //
	private Spring springMain;
	private Material material;

	// ------------------ //   
	// --- 私有成员    
	// ------------------ //



	// ------------------ //   
	// --- unity 消息
	// ------------------ //
	private void Awake()
	{
		springMain = GetComponentInParent<Spring>();
		material = GetComponent<SpriteRenderer>().material;
	}

	// ------------------ //    
	// --- 公有方法   
	// ------------------ //
	private void Update()
	{
		//if (springMain.SliderJoint != null && springMain.SpringJoint != null)
		//{

		//	float rate = Vector2.Distance(springMain.TopAndBottom[0].position, springMain.TopAndBottom[1].position) / springMain.LimitRange[1];
		//	Debug.LogError($"当前比率 = {rate}");
		//	material.SetFloat("Rate", rate);
		//}
	}

	// ------------------ //   
	// --- 私有方法
	// ------------------ //
}
