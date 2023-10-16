using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


public class MaterialConfig : UniqueConfigBase
{
	// ------------------ //    
	// --- 序列化    
	// ------------------ //
	public Material SpriteLit;
	public Material SpriteUnlit;

	[Header("建模")]
	public Material SymbolLine;

	[Header("零件")]
	public Material Part_Edit;
	public Material Part_Physics;

	[Header("编辑/目标区域")]
	public Material EditAreaBox;
	public Material RedErrorLine;

	[Header("其他")]
	public Material SimpleOffset;
	public Material Rotate;
	public Material FlashLed;

	// ------------------ //    
	// --- 公有成员    
	// ------------------ //
	public static MaterialConfig Instance { private set; get; }

	// ------------------ //   
	// --- 私有成员    
	// ------------------ //


	// ------------------ //    
	// --- Unity消息    
	// ------------------ //
	public override void InitInstance()
	{
		Instance = this;
	}


	// ------------------ //    
	// --- 公有方法   
	// ------------------ //

	// ------------------ //   
	// --- 私有方法
	// ------------------ //


	// ------------------ //
	// --- 类型
	// ------------------ //
}

