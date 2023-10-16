using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSinglton<T> : MonoBehaviour where T : MonoSinglton<T>
{
	private static T _instance;
	public static T Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = FindObjectOfType(typeof(T)) as T;
				if (_instance == null)
				{
					GameObject.FindGameObjectWithTag(typeof(T).ToString()).TryGetComponent<T>(out _instance);
				}
				if (_instance == null)
				{
					Debug.LogError("找不到这个mono单例的实例-" + typeof(T).ToString());
				}
			}
			return _instance;
		}
	}

	private void Awake()
	{
		if (_instance == null)
		{
			_instance = GetComponent<T>();
		}
	}
}
