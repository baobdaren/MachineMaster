using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchableImage : Image
{
    // ----------------//
    // --- 序列化
    // ----------------//
    [SerializeField] 
    private Sprite SecondSprite;

    // ----------------//
    // --- 公有成员
    // ----------------//
    public bool IsOpen
    {
        get => OpenSprite == sprite;
        set
        {
            SwitchSprite(value);
        }
    }


    // ----------------//
    // --- 私有成员
    // ----------------//
    private Sprite _openSprite;
    private Sprite OpenSprite
    {
        get
        {
			if(_openSprite == null)
			{
                _openSprite = SecondSprite;
			}
            return _openSprite;
        }
    }


    // ----------------//
    // --- Unity消息
    // ----------------//


    // ----------------//
    // --- 公有方法
    // ----------------//


    // ----------------//
    // --- 私有方法
    // ----------------//
    private void SwitchSprite ( bool open )
    {
		if((IsOpen && open) || (!IsOpen && !open))
		{
            return;
		}
        var last = sprite;
        sprite = SecondSprite;
        SecondSprite = last;
    }

}
