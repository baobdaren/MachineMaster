using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class AudioButton : NiceButton
{
    [SerializeField]
    public int num;
    // ----------------//
    // --- 序列化
    // ----------------//


    // ----------------//
    // --- 公有成员
    // ----------------//


    // ----------------//
    // --- 私有成员
    // ----------------//
    private AudioSource audioSrc
    {
        get
        {
			if(_audioSrc == null)
			{
                _audioSrc = GetComponent<AudioSource>();
			}
            return _audioSrc;
        }
    }
    private AudioSource _audioSrc;


    // ----------------//
    // --- Unity消息
    // ----------------//


	// ----------------//
	// --- 公有方法
	// ----------------//
	public override void OnPointerClick ( PointerEventData eventData )
	{
        audioSrc.Play();
		base.OnPointerClick(eventData);
	}

	//public override void OnPointerExit(PointerEventData eventData)
	//{
 //       base.OnPointerExit(eventData);
 //       //base.OnPointerExit(eventData);
 //       Debug.LogError("Exit");
 //       this.DoStateTransition(SelectionState.Normal, true);
	//}

	//public override void OnMove(AxisEventData eventData)
	//{
	//	base.OnMove(eventData);
 //       Debug.LogError("OnMove");
	//}
	//public override void OnPointerEnter(PointerEventData eventData)
	//{
 //       base.OnPointerEnter(eventData);
 //       Debug.LogError("Enter");
 //   }

	//public override void OnPointerDown(PointerEventData eventData)
	//{
	//	base.OnPointerDown(eventData);
 //       Debug.LogError("Down");
 //   }

	//public override void OnDeselect(BaseEventData eventData)
	//{
	//	base.OnDeselect(eventData);
 //       Debug.LogError("OnDeselect");
 //   }

	//public override void OnSelect(BaseEventData eventData)
	//{
	//	base.OnSelect(eventData);
 //       Debug.LogError("OnSelect");

 //   }

    // ----------------//
    // --- 私有方法
    // ----------------//

}
