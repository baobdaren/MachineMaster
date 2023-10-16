using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class ExpressionBase
{
    // -------------- //
    // --  公有属性
    // -------------- //
    [SerializeField]
    [TextArea]
    public string FuncDetail;
    [Multiline]
    public string ExpressionButtonTex;
    public abstract int InputAmount { get; }
    public abstract int OutputAmount { get; }
    public abstract ExpressionID ID { get; }
    public abstract SorttingType SortType { get; }
    public virtual List<ExpressionSettingItem> SettingItemList { get; }


    // -------------- //
    // --  私有属性
    // -------------- //
    protected abstract float[] ExpressionFunc(float[] args, ExpressionSettingsValueSet settingValue);
    //public virtual Dictionary<string, KeyValuePair<ExpressionSettingType, float>> Settings
    //{
    //    get;
    //}
    //public  Dictionary<string,ExpressionSetting> Settings;


    // -------------- //
    // --  公有方法
    // -------------- //
    public ExpressionBase ( )
    {
		
    }

    public float[] Caculate(float[] inputs, ExpressionSettingsValueSet set)
    {
        float[] result = new float[0];
		try
		{
            result = ExpressionFunc(inputs, set);
		}
		catch (Exception ex)
		{
            Debug.LogError("运行错误" + ex.Message);
		}
        return result;
    }

    // -------------- //
    // --  私有方法
    // -------------- //
    public ExpressionSettingsValueSet CreateDefaultSettingsValueSet()
    {
        ExpressionSettingsValueSet result = new ExpressionSettingsValueSet();
		if (SettingItemList != null)
		{
		    foreach (var item in SettingItemList)
		    {
                result.SettingValue.Add(item.DefaultValue);
		    }
		}
        return result;
    }


    // -------------- //
    // --  私有方法
    // -------------- //
    protected static float[] ZERO = new float[] { 0 };
    public enum SorttingType
    {
        INPUT, OUTPUT, FUNC, SENSOR, CMD
    }
}




[Serializable]
public class ExpressionSettingItem
{
    public ExpressionSettingItem(string title, bool defaultValue)
    {
        SettingValueType = ExpressionSettingType.BOOL;
        SettingTitle = title;
        DefaultValue = defaultValue ? 1 : 0;
    }

    public ExpressionSettingItem(string title, float defaultValue, float limitMin = float.MinValue, float limitMax = float.MaxValue)
    {
        SettingValueType = ExpressionSettingType.NUM;
        SettingTitle = title;
        DefaultValue = defaultValue;
        LimitMin = limitMin;
        LimitMax = limitMax;
    }
	public  string SettingTitle;
    public  ExpressionSettingType SettingValueType;
    //public float setData;
    public float LimitMax;
    public float LimitMin;
    public float DefaultValue;
}