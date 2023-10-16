using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 可重置单例
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class ResetAbleInstance<T> where T: new()
{
    public static T Instance 
    {
        get
        {
			if (_instance == null)
			{
                _instance = new T();
			}
            return _instance;
        }
    }

    private static T _instance;

	protected ResetAbleInstance()
	{
	}

    // ------------------ //    
    // --- 序列化    
    // ------------------ //

    // ------------------ //    
    // --- 公有成员    
    // ------------------ //
    //private static List<ResetAbleInstance> resetAbleInstances = new List<ResetAbleInstance>();

	public static void ResetData()
    {
        _instance = new T();
        Debug.Log("重置模块-" + typeof(T));
    }

    // ------------------ //   
    // --- 私有成员    
    // ------------------ //

    // ------------------ //    
    // --- Unity消息    
    // ------------------ //

    // ------------------ //    
    // --- 公有方法   
    // ------------------ //

    // ------------------ //   
    // --- 私有方法
    // ------------------ //
}
