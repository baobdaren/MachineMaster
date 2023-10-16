using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ArchiveManager;

public abstract class Loadable<T>:ISaveable where T : Loadable<T>, new()
{
	// ------------------ //    
	// --- 序列化    
	// ------------------ //


	// ------------------ //    
	// --- 公有成员    
	// ------------------ //
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


	// ------------------ //   
	// --- 私有成员    
	// ------------------ //
	private static T _instance;


	// ------------------ //    
	// --- Unity消息    
	// ------------------ //

	// ------------------ //    
	// --- 公有方法   
	// ------------------ //
	public void LoadArchive(Archive archive)
	{
		OnResetData();
		OnLoadedArchive(archive);
	}
	public void SaveIntoArchive(Archive archive)
	{
		OnSaveingArchive(archive);
	}
	// ------------------ //   
	// --- 私有方法
	// ------------------ //
	protected abstract void OnResetData();
	protected abstract void OnLoadedArchive(Archive archive);
	protected abstract void OnSaveingArchive(Archive archive);
}
