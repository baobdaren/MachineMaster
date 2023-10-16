using System;
using System.IO;
using UnityEngine;

[Serializable]
public class BundlePaths
{
	public static BundlePaths Instance
	{
		get
		{
			if(_ins == null)
			{
				_ins = new BundlePaths();
			}
			return _ins;
		}
	}
	private static BundlePaths _ins;

	private BundlePaths (  )
	{
		//ClientConfig configData = JsonConvert.DeserializeObject<ClientConfig>(configJsonStr);
		//Data = configData;
	}

	private ClientConfig Data;

	//public static readonly string PATH_BUNDLEROOT = @"O:\MechanicMaster";
	public static readonly string PATH_BUNDLEROOT = Application.streamingAssetsPath;
	//#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN
	//public static readonly string PATH_BUNDLEROOT = @"C:\Users\GODBO\Desktop\MechanicMaster";
	//#else
	//public static readonly string PATH_BUNDLEROOT = Application.persistentDataPath + "/";
	//#endif
	public static readonly string PATH_CONFIGURL = Application.streamingAssetsPath + "/Config/config.txt";
	public static readonly string PATH_DLL = Application.persistentDataPath + "/DLL.unity3d";
	public string PATH_VERSIONFILE
	{
		get
		{
			return CombinePath(PATH_BUNDLEROOT, Data.LocalVersionFilePath);
		}
	}
	public string PATH_TMPVERSIONFILE
	{
		get
		{
			return CombinePath(PATH_BUNDLEROOT, Data.LocalVersionTmpFilePath);
		}
	}

	/// <summary>
	/// 服务端
	/// </summary>
	public string PATH_SERVER
	{
		get
		{
			return "http://192.144.234.56/Asset/";
		}
	}


	public static string CombinePath ( string a, string b)
	{
		a = a.Replace(@"\", "/");
		b = b.Replace(@"\", "/");
		while(b.StartsWith("/") && b.Length > 0)
		{
			b = b.Substring(1);
		}
		return Path.Combine(a, b).Replace(@"\", "/");
	}

	[Serializable]
	private class ClientConfig
	{
		public string LocalVersionFilePath;
		public string LocalVersionTmpFilePath;
	}
}
