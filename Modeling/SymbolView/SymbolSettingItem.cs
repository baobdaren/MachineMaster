using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 建模符号表达式设置中的单独的一项（如：常数=1）
/// </summary>
public class SymbolSettingItem : MonoBehaviour
{
    ////////////////////
    // -- 序列化
    ////////////////////
    [SerializeField]
    private TextMeshProUGUI _title;
    [SerializeField]
    private Toggle _toggle;
    [SerializeField]
    private TMP_InputField _inputField;


    ////////////////////
    // -- 公有成员
    ////////////////////
    public bool IsOn
    {
        get
        {
            return _toggle.isOn;
        }
        set
        {
            _toggle.isOn = value;
        }
    }

    public float Num
    {
        get
        {
			if(float.TryParse(_inputField.text, out float result))
			{
                return result;
			}
			else
			{
                return 0;
			}
        }
        set
        {
            _inputField.text = value.ToString();
        }
    }

    public string Title
    {
        get => _title?.text;
    }


    ////////////////////
    // -- 私有成员
    ////////////////////
    private int _currentSettingItemIndex;
    private Action<int, float> _callBack;


    ////////////////////
    ///// Unity消息
    ////////////////////
    void Start()
    {
        _toggle.onValueChanged.AddListener(OnValueChanged);
        _inputField.onValueChanged.AddListener(OnValueChanged);
    }


    ////////////////////
    // -- 公有方法
    ////////////////////
    public void CreateSettingItemUI (string title, bool ison, int itemIndex, Action<int, float> callBack)
    {
        _inputField.gameObject.SetActive(false);
        _toggle.gameObject.SetActive(true);
        _title.text = title;
        _toggle.isOn = ison;
        _currentSettingItemIndex = itemIndex;
        _callBack = callBack;
    }
    public void CreateSettingItemUI ( string title, float defaultNum, int itemIndex, Action<int, float> callBack)
    {
        _inputField.gameObject.SetActive(true);
        _toggle.gameObject.SetActive(false);
        _title.text = title;
        _inputField.text = defaultNum.ToString();
        _currentSettingItemIndex = itemIndex;
        _callBack = callBack;
    }
    public float GetValue ( )
    {
		if(_toggle.gameObject.activeSelf)
		{
            return _toggle.isOn ? 1 : 0;
		}
		else/*(_inputField.gameObject.activeSelf) */
		{
            if(float.TryParse(_inputField.text, out float result))
            {
                return result;
            }
			else
			{
                Debug.LogError("数字转换失败 内容=" + _inputField.text);
                return 0;
			}
		}
    }


    ////////////////////
    // -- 私有方法
    ////////////////////
    private void OnValueChanged(string value)
    {
		if (float.TryParse(value, out float num))
		{
            _callBack?.Invoke(_currentSettingItemIndex, num);
		}
    }
    private void OnValueChanged ( bool value )
    {
        _callBack?.Invoke(_currentSettingItemIndex, value?1:0);
    }
}