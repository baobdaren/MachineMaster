using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class PartLed : MonoBehaviour
{
	// ----------------//
	// --- 序列化
	// ----------------//
	[SerializeField]
	private Sprite FlashLightSprite;
	[SerializeField]
	private Color FlashColor;


	// ----------------//
	// --- 公有成员
	// ----------------//
	//public static int PartFirstSortingLayerID
	//{
	//	get => SortingLayer.NameToID("P20");
	//}


	// ----------------//
	// --- 私有成员
	// ----------------//


	// ----------------//
	// --- Unity消息
	// ----------------//
//#if UNITY_EDITOR
//	private void OnEnable()
//	{
//		TurnOn();
//	}
//#endif


	// ----------------//
	// --- 公有方法
	// ----------------//
	public void TurnOn(PlayerPartCtrl partCtrl)
	{
		if (GetComponent<SpriteRenderer>() == null)
		{
			SpriteRenderer sp = gameObject.AddComponent<SpriteRenderer>();
			sp.sprite = FlashLightSprite;
			sp.material = MaterialConfig.Instance.FlashLed;
			sp.color = FlashColor;
		}
		//var sortingAndOrder = RenderLayerManager.Instance.GetLedSortingLayerAndOrder(partCtrl);
		//GetComponent<SpriteRenderer>().sortingLayerID = sortingAndOrder.Item1;
		//GetComponent<SpriteRenderer>().sortingOrder = sortingAndOrder.Item2;
		gameObject.SetActive(true);
	}

	public void TurnOff()
	{
		Destroy(GetComponent<SpriteRenderer>());
	}
	// ----------------//
	// --- 私有方法
	// ----------------//

}
