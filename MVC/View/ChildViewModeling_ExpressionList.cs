using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChildViewModeling_ExpressionList : MonoBehaviour
{
    // -------------- //
    // --  序列化
    // -------------- //
    private Button _buttonItem;

    // -------------- //
    // --  公有成员
    // -------------- //


    // -------------- //
    // --  私有成员
    // -------------- //
    private readonly Dictionary<Button, ExpressionBase.SorttingType> _creatdButtons = new Dictionary<Button, ExpressionBase.SorttingType>();

	// -------------- //
	// --  Unity消息
	// -------------- //
	private void Awake()
	{
        _buttonItem = GetComponentInChildren<NiceButton>();
	}

	void Start()
    {
        InitAllButtons();
        DisplayExpressionListView(ExpressionBase.SorttingType.OUTPUT);
    }


    // -------------- //
    // --  公有方法
    // -------------- //

    public void DisplayExpressionListView (ExpressionBase.SorttingType displayIOType )
    {
		foreach(KeyValuePair<Button, ExpressionBase.SorttingType> item in _creatdButtons)
        {
            item.Key.gameObject.SetActive(item.Value == displayIOType);
        }
    }

    // -------------- //
    // --  私有方法
    // -------------- //
    private void InitAllButtons ( )
    {
		foreach (KeyValuePair<ExpressionID, ExpressionBase> item in ExpressionConfig.Instance.AllExpressionDict)
		{
			GameObject cloneItem = Instantiate(_buttonItem.gameObject, _buttonItem.transform.parent);
            cloneItem.SetActive(false);
            //cloneItem.name = item.Value.;
            cloneItem.GetComponentInChildren<TextMeshProUGUI>().text = item.Value.ExpressionButtonTex;
			Button cloneButtonCmpnt = cloneItem.GetComponentInChildren<Button>();
			cloneButtonCmpnt.onClick.AddListener(
				() =>
				{
					//ControllerModeling.Instance.AddSymbol(item.Value());
                    ControllerModeling.Instance.CreateNewNodeSymbol(item.Value.ID);
					//ModelModeling.Instance.UsingPart.ModelMap.CreateNode(item.Value());
				}
			);
			_creatdButtons.Add(cloneButtonCmpnt, item.Value.SortType);
		}
		DestroyImmediate(_buttonItem.gameObject);
	}
}
