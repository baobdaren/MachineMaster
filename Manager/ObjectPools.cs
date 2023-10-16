using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPools
{
    public static ObjectPools Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new ObjectPools();
            }
            return _instance;
        }
    }

    Transform poolObjectParent = new GameObject("pool game object parenet").transform;

    private static ObjectPools _instance;

    private readonly Dictionary<Type, List<IPoolableObject>> _objPools = new Dictionary<Type, List<IPoolableObject>>();
    private readonly Dictionary<string, List<GameObject>> _goPools = new Dictionary<string, List<GameObject>>();

    ObjectPools()
    {
        poolObjectParent.gameObject.SetActive(false);
    }

    public T GetItem<T>(Type name) where T : class, IPoolableObject, new()
    {
        if (!_objPools.ContainsKey(name))
        {
            _objPools.Add(name, new List<IPoolableObject>());
        }
        if (_objPools[name].Count == 0)
        {
            _objPools[name].Add(new T());
        }
        T r = _objPools[name][0] as T;
        _objPools[name].RemoveAt(0);
        r.SetRun();
        return r;
    }

    public void SaveItem<T>(IPoolableObject obj, int maxNum = 50) where T : class, IPoolableObject, new()
    {
        if (obj == null)
        {
            return;
        }
        if (!_objPools.ContainsKey(obj.GetType()))
        {
            _objPools.Add(obj.GetType(), new List<IPoolableObject>());
        }
        obj.SetStop();
        _objPools[obj.GetType()].Add(obj);
        while (_objPools[obj.GetType()].Count > maxNum)
        {
            _objPools[obj.GetType()].RemoveAt(_objPools[obj.GetType()].Count - 1);
        }
    }

    public GameObject GetGoItem(string name, string resPath = null)
    {
        if (!_goPools.ContainsKey(name))
        {
            if (!string.IsNullOrEmpty(resPath))
            {
                SaveGoItem(name, GameObject.Instantiate(Resources.Load(resPath) as GameObject));
            }
            else
            {
                return null;
            }
        }
        if (_goPools[name].Count == 0)
        {
            Debug.LogError("意外的，mono对象池存在，但是被清空");
            return null;
        }
        if (_goPools[name].Count == 1)
        {
            return GameObject.Instantiate(_goPools[name][0]);
        }
        else
        {
            GameObject r = _goPools[name][_goPools[name].Count - 1];
            _goPools[name].RemoveAt(_goPools[name].Count - 1);
            return r;
        }
    }

    public void SaveGoItem(string name, GameObject obj, int maxNum = 50)
    {
        if (string.IsNullOrEmpty(name))
        {
            return;
        }
        if (!_goPools.ContainsKey(name))
        {
            _goPools.Add(name, new List<GameObject>());
        }
        if (obj != null)
        {
            obj.transform.SetParent(poolObjectParent);
            obj.SetActive(false);
            _goPools[name].Add(obj);
        }
        while (_goPools[name].Count > maxNum)
        {
            GameObject.Destroy(_goPools[name][_goPools[name].Count - 1].gameObject);
            _goPools[name].RemoveAt(_goPools[name].Count - 1);
        }
    }

    public bool TrySaveGoItem(GameObject gameObject)
    {
        if (gameObject == null || gameObject.GetComponent<IPoolableObject>() == null)
        {
            return false;
        }
        IPoolableObject v = gameObject.GetComponent<IPoolableObject>();
        if (v != null)
        {
            v.SetStop();
            return true;
        }
        return false;
    }

    public Dictionary<string, string> PoolsInfo()
    {
        Dictionary<string, string> info = new Dictionary<string, string>();
        foreach (var item in _objPools)
        {
            info.Add(item.Key.Name, item.Value.Count.ToString());
        }

        foreach (var item in _goPools)
        {
            info.Add(item.Key, item.Value.Count.ToString());
        }

        return info;
    }
}

public interface IPoolableObject
{
    void SetRun();
    void SetStop();
}
