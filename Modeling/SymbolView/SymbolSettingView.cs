using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 建模符号的双击后打开的相关悬浮窗
/// </summary>
public class SymbolSettingView : MonoBehaviour
{
    // ---------------- //
    // -- 序列化
    // ---------------- //
    [SerializeField]
    private Button _deleteButton;
    [SerializeField]
    private Button _closeButton;
    [SerializeField]
    private GameObject _item;

    // ---------- //-
    // -- 公有成员
    // ---------- //-
    public static SymbolSettingView Instance
    {
        get
        {
			if(_instacne == null)
			{
                GameObject view = GameObject.Instantiate(GameConfig.Instance.SymbolSettingCanvas);
                if(!view.TryGetComponent(out _instacne))
                {
                    _instacne = view.AddComponent<SymbolSettingView>();
                }
            }
            return _instacne;
        }
    }
	public Symbol ListenSymbol { private set; get; }


    // ---------------- //
    // -- 私有成员
    // ---------------- //
    private static SymbolSettingView _instacne;
	private readonly List<SymbolSettingItem> _createdSettingItemUIList = new List<SymbolSettingItem>();


    // ---------------- //
    // -- 公有方法
    // ---------------- //
    public bool Display (Symbol listenSymbol,  List<ExpressionSettingItem> settingItemList, ExpressionSettingsValueSet setValue)
    {
        ListenSymbol = listenSymbol;
        Debug.Log($"显示符号设置界面-{listenSymbol.GetType()}");
        gameObject.SetActive(true);
        CreateItems(settingItemList != null ? settingItemList.Count : 0);
        if(settingItemList != null)
        {
            int index = 0;
            foreach(ExpressionSettingItem item in settingItemList)
            {
                float startValue = setValue.SettingValue[index];
                //Debug.Log($" 设置({item.SettingTitle})为({item.Value.setData})");
                switch(item.SettingValueType)
                {
                    case ExpressionSettingType.BOOL:
                        _createdSettingItemUIList[index].CreateSettingItemUI(item.SettingTitle, startValue!=0, index, OnSettingItemValueChanged);
                        break;
                    case ExpressionSettingType.NUM:
                        _createdSettingItemUIList[index].CreateSettingItemUI(item.SettingTitle, startValue,  index, OnSettingItemValueChanged);
                        break;
                }
                index++;
            }
        }
        return true;
    }
    public void SetItem ( string item, ExpressionSettingType argType, float value )
    {
		for(int i = 0; i < _createdSettingItemUIList.Count; i++)
		{
			if(item == _createdSettingItemUIList[i].Title)
			{
				switch(argType)
				{
					case ExpressionSettingType.BOOL:
                        _createdSettingItemUIList[i].IsOn = value == 1;
						break;
					case ExpressionSettingType.NUM:
                        _createdSettingItemUIList[i].Num = value;
						break;
				}
                return;
			}
		}
    }
    public void Hide ( )
    {
        ListenSymbol = null;
		gameObject.SetActive(false); 
	}
    public void DoMove ( Vector2 tarPos )
    {
		if(Vector2.Distance(tarPos, transform.position) < 8)
		{
            transform.DOMove(tarPos, 0.3f);
		}
		else
		{
            transform.position = tarPos;
		}
    }


	// ---------------- //
	///// Unity消息
	// ---------------- //
	private void Awake ( )
	{
        _item.gameObject.SetActive(false);
        GetComponent<Canvas>().sortingLayerID = SortingLayer.NameToID("UI1");
        GetComponent<Canvas>().renderMode = RenderMode.WorldSpace;
        GetComponent<Canvas>().worldCamera = CameraActor.Instance.MainCamera;
        //Debug.LogError("UI = " + LayerMask.NameToLayer("UI"));
	}
    private void OnEnable ( )
	{
	    CameraActor.OnCameraViewSizeChanged += ChangedSize;
        ChangedSize(CameraActor.Instance.MainCamera.orthographicSize);
        _deleteButton.onClick.AddListener(OnClick_DeleteSymbol);
        _closeButton.onClick.AddListener(Hide);
	}
	private void OnDisable ( )
	{
	    CameraActor.OnCameraViewSizeChanged -= ChangedSize;
        _deleteButton.onClick.RemoveAllListeners();
        _closeButton.onClick.RemoveAllListeners();
    }


    //////////////--//
    // -- 私有方法
    //////////////--//
    private void CreateItems ( int len )
    {
		while(_createdSettingItemUIList.Count < len)
		{
            var cloneItem = GameObject.Instantiate(_item, _item.transform.parent);
            _createdSettingItemUIList.Add(cloneItem.GetComponent<SymbolSettingItem>());
		}
		for(int i = 0; i < _createdSettingItemUIList.Count; i++)
		{
            _createdSettingItemUIList[i].gameObject.SetActive(i < len);
		}
    }
    private SymbolSettingView ( )
    {
    }
    private void ChangedSize ( float s )
    {
        s /= 25f;
        transform.DOScale(new Vector3(s, s, 1), 0.3f);
    }

    private void OnSettingItemValueChanged ( int index, float value )
    {
        ListenSymbol.MyNode.SettingsValue.SettingValue[index] = value;
        ListenSymbol.MyNode.UpdateSymbol();
    }
    private void OnClick_DeleteSymbol ( )
    {
        ControllerModeling.Instance.DeleteSymbol(ListenSymbol);
        Hide();
    }
}

public enum ExpressionSettingType
{
    BOOL,NUM
}

