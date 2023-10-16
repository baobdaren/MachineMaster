using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorManager:Loadable<SensorManager>
{
    // ---------------  //
    // --  公有成员
    // ---------------  //


    // ---------------  //
    // --  私有成员
    // ---------------  //
    private Dictionary<BaseSensor, List<Symbol>> SensorSymbols = new Dictionary<BaseSensor, List<Symbol>>();

    // ---------------  //
    // --  公有方法
    // ---------------  //
    public void RegistSensor(BaseSensor sensor)
    {
        if (SensorSymbols.ContainsKey(sensor))
        {
            return;
        }
        SensorSymbols.Add(sensor, new List<Symbol>());
        Debug.LogError("传感器+1 ，共：" + SensorSymbols.Count);
    }

    /// <summary>
    /// 注册一个传感器使用的符号，移除曾使用的
    /// </summary>
    /// <param name="symbol">刚创建的符号</param>
    /// <param name="sensor">该符号要使用的传感器</param>
    /// <param name="oldSensor">该符号曾使用的传感器</param>
    public void RegistSymbolUsedSensor (Symbol symbol,  BaseSensor sensor, BaseSensor oldSensor=null)
    {
		if(sensor != null && SensorSymbols.ContainsKey(sensor))
		{
            SensorSymbols[sensor].Add(symbol);
			if(oldSensor!=null)
			{
                SensorSymbols[sensor].Remove(symbol);
			}
		}
    }
    public void ClearSensor()
    {
        SensorSymbols.Clear();
    }
    public void RemoveSensor ( BaseSensor sensor )
    {
		if(sensor != null)
		{
			if(SensorSymbols.ContainsKey(sensor))
			{
				while(SensorSymbols[sensor].Count > 0)
				{
                    var obj = SensorSymbols[sensor][0];
					if (obj && obj.gameObject)
					{
                        SensorSymbols[sensor].RemoveAt(0);
                        GameObject.Destroy(obj.gameObject);
					}
				}
                SensorSymbols.Remove(sensor);
			}
		}
    }
    public bool GetNextSensors ( BaseSensor current, bool findNext ,out BaseSensor findResult)
    {
        findResult = null;
		if(SensorSymbols.Count == 0 || current == null || !SensorSymbols.ContainsKey(current))
		{
            return false;
		}
        using(var iter = SensorSymbols.GetEnumerator())
        {

            BaseSensor last = null;
            string findValue = current.GetType().ToString();
            BaseSensor firstFinded=null;
            Debug.LogError($"获取{findNext} （上下），类型：{findValue}");
            // 找到当前的
            while(iter.MoveNext())
            {
				if(firstFinded == null && iter.Current.Key.GetType().ToString() == findValue)
				{
                    firstFinded = iter.Current.Key;
				}
                if(iter.Current.Key == current)
                {
                    break;
                }
                else if(iter.Current.Key.GetType().ToString() == findValue)
                {
                    last = iter.Current.Key;
                }
            }
            if(findNext)
            {
                while(iter.MoveNext())
                {
                    if(iter.Current.Key.GetType().ToString() == findValue)
                    {
                        findResult = iter.Current.Key;
                        return true;
                    }
                }
                // 最后一个没有下一个
				if(findResult == null)
				{
                    findResult = firstFinded;
				}
            }
            else
            {
                if(last == null)
                {
                    // 当前的是第一个，没有上一个，这里从当前开始，找到最后一个以循环。
                    last = iter.Current.Key;
                    while(iter.MoveNext())
                    {
                        if(iter.Current.Key.GetType().ToString() == findValue)
                        {
                            last = iter.Current.Key;
                        }
                    }
                }
                // 总是保存最后找到的直到迭代完毕。
                findResult = last;
                return true;
            }
        }
        return false;
    }
    public bool GetFirstSensor (string findItemValue,out BaseSensor result)
    {
        Debug.LogError("查找第一个，类型" + findItemValue);
		foreach(var item in SensorSymbols)
		{
			if(item.Key.GetType().ToString() == findItemValue)
			{
                result = item.Key;
                return true;
			}
		}
        result = null;
        return false;
    }

	protected override void OnLoadedArchive(ArchiveManager.Archive archive)
	{
		//throw new System.NotImplementedException();
	}

	protected override void OnSaveingArchive(ArchiveManager.Archive archive)
	{
		//throw new System.NotImplementedException();
	}

    protected override void OnResetData()
	{
		//throw new System.NotImplementedException();
	}


	// ---------------  //
	// --  私有方法
	// ---------------  //
}
