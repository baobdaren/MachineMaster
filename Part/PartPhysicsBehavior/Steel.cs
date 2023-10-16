using System.Collections.Generic;
using UnityEngine;

public class Steel : PlayerPartBase
{
    // ------------- //
    // --- 序列化
    // ------------- //

    // ------------- //
    // --- 私有成员
    // ------------- //

    // ------------- //
    // --- 公有成员
    // ------------- //
    public SteelAccessor MyAccesstor { get => Accessor as SteelAccessor; }
    // ------------- //
    // --- Unity消息
    // ------------- //

    // ------------- //
    // --- 私有方法
    // ------------- //
 //   protected override void ChangeSize()
	//{
 //       Debug.LogError("Steel 修改尺寸" + MyCtrlData.MainSizeIndex);
 //       if (_originSize == Vector2.zero)
 //       {
 //           _originSize = GetComponent<SpriteRenderer>().size;
 //       }
 //       GetComponent<BoxCollider2D>().autoTiling = true;
 //       GetComponent<SpriteRenderer>().drawMode = SpriteDrawMode.Tiled;
 //       GetComponent<SpriteRenderer>().size = Vector2.right * (MyCtrlData.MainSizeIndex + 1) * _originSize.x + Vector2.up * _originSize.y;
 //       Debug.LogError("Steel 尺寸参数为- " + GetComponent<SpriteRenderer>().size);
 //       //GameObject.DestroyImmediate(GetComponent<SpriteRenderer>());
	//}

    // ------------- //
    // --- 公有方法
    // ------------- //

    // ------------- //
    // --- 类型
    // ------------- //

}
