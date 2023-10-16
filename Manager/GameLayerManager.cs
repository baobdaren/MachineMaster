using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class GameLayerManager
{
	// ----------------//
	// --- 公有成员
	// ----------------//
	public static GameLayerManager Instance = new GameLayerManager();

	public const int Default = 0;
	public const int IgnoreRayCast = 2;
	public const int DefalutEnv = 10;
	public const int RailConnBody = 17;
	public const int DefaultPartObjectLayer = 20;
	public const int EditPart = 19;
	public const int DefaultPartRenderLayer = 6;

	// ----------------//
	// --- 私有成员
	// ----------------//

	// ----------------//
	// --- 公有方法
	// ----------------//
	public static int GetPartGameObjectLayer(int layer)
	{
		return (DefaultPartObjectLayer + layer);
	}

	public static int GetPartRenderLayers(int layer)
	{
		return SortingLayer.NameToID("P" + layer);
	}

	public static int GetBearingLayer(params BasePartCtrl[] connectedParts)
	{
		int maxLayer = 0;
		foreach (var item in connectedParts)
		{
			maxLayer = Mathf.Max(maxLayer, item.Layer);
		}
		return SortingLayer.NameToID("Bearing" + maxLayer);
	}
	// ----------------//
	// --- 私有方法
	// ----------------//

	// ----------------//
	// --- 类型
	// ----------------//
}
