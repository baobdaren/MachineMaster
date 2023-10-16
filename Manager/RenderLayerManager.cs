using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderLayerManager
{
    //// ------------- //
    //// -- 序列化
    //// ------------- //
    //public static RenderLayerManager Instance = new RenderLayerManager();
    //private RenderLayerManager() { }

    //// ------------- //
    //// -- 公有成员
    //// ------------- //

    ////private const string EditBearingIconSortingLayer = "EditBearing"; // 编辑状态下的轴承
    //private const string EditBearingIconSortingLayer = "EditBearing"; // 编辑状态下的轴承图标

    //// sortingOrder
    //public const int BearingOrder = 2;
    //public const int LedOrder = 1;
     
    //public int EditBearingSortingLayer { get => SortingLayer.GetLayerValueFromName(EditBearingIconSortingLayer); }

    //public int GetEditAreaIndex { get => SortingLayer.NameToID("UI1"); }
    //public int GetEditBearingIndex { get => SortingLayer.NameToID("UI1"); }
    //public static int GetAdsortLineSortLayer { get => SortingLayer.NameToID("UI2"); }

    //// ------------- //
    //// -- 私有成员
    //// ------------- //
    ////List<string> allLayerNames = new List<string>() { "Default", "B3", "B2", "B1", "PA", "PB", "PC", "PD", "PE", "PF", "PG", "PH", "PI", "PJ", "PK", "F1", "F2", "F3", "UI1", "UI2", "UI3", };
    //private List<Color> _layer2Color;
    //// ------------- //
    //// -- 公有方法
    //// ------------- //
    //public Color GetPartColor(int layer)
    //{
    //    //return PartLayerToColor[Mathf.Clamp(layer, DefaultPartLayer, DefaultPartLayer+PartLayerCount) - DefaultPartLayer];
    //    return Color.green;
    //}
    ////public int GetPartSortingLayer ( PlayerPartCtrl  partCtrl)
    ////{
    ////    return GetPartSortingLayer(partCtrl.MyPartType);
    ////}
    ////public int GetPartSortingLayer(PartTypes partType)
    ////{
    ////    //return SortingLayer.NameToID("P" + (char)(PartConfig.Instance.SortingLayerList.IndexOf(partType) + 'A'));
    ////    return 0;
    ////}

    ////public (int,int) GetLedSortingLayerAndOrder(PlayerPartCtrl partCtrl)
    ////{ 
    ////    return (GetPartSortingLayer(partCtrl), LedOrder);
    ////}
    //// ------------- //
    //// -- 私有方法
    //// ------------- //
}
