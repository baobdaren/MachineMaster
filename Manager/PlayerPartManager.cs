using System;
using System.Collections.Generic;
using UnityEngine;
using static ArchiveManager;

public class PlayerPartManager:Loadable<PlayerPartManager>
{
	// ------------------ //    
	// --- 序列化    
	// ------------------ //


	// ------------------ //    
	// --- 公有成员    
	// ------------------ //
	public List<PlayerPartCtrl> AllPlayerPartCtrls
	{
		get 
		{
			if (_allPartCtrls == null)
			{
				_allPartCtrls = new List<PlayerPartCtrl>();
			}
			return _allPartCtrls;
		}
	}
	[ES3NonSerializable]
	public Dictionary<PlayerPartCtrl, List<Collider2D>> PartCtrlToColliderList { get; private set; } = new Dictionary<PlayerPartCtrl, List<Collider2D>>();
	[ES3NonSerializable]
	public Dictionary<Collider2D, PlayerPartCtrl> ColliderToPartCtrl { get; set; } = new Dictionary<Collider2D, PlayerPartCtrl>();
	[ES3NonSerializable]
	public List<Collider2D> AllColliders { get; private set; } = new List<Collider2D>();

	// ------------------ //   
	// --- 私有成员    
	// ------------------ //
	[ES3Serializable]
	private List<PlayerPartCtrl> _allPartCtrls;
	/// <summary>
	/// 临时添加的刚体，保存引用以移除
	/// </summary>
	[ES3NonSerializable]
	private Collider2D[] _extraColliders;

	[ES3NonSerializable]
	private Dictionary<PartTypes, List<PlayerPartCtrl>> PartTypeToCtrl = new Dictionary<PartTypes, List<PlayerPartCtrl>>();

	//private List<List<PartCtrl>> _frontPartCtrlGroups;
	//private List<PartCtrl> _constructPartCtrls;
	//private List<List<PartCtrl>> _backPartCtrlGroups;

	// ------------------ //    
	// --- Unity消息    
	// ------------------ //

	// ------------------ //    
	// --- 公有方法   
	// ------------------ //

	public void AddPart(PlayerPartCtrl partCtrl)
	{
		RegistCtrlData(partCtrl);
		PartManager.Instance.RegistPartDragingListener(partCtrl);
		PartManager.Instance.RegistPartDoubleClickListener(partCtrl);
	}

	public void DeletePart(PlayerPartCtrl partCtrl)
	{
		//Debug.LogError($"删除零件 {partCtrl.MyPartType}");
		UnRegistCtrlData(partCtrl);
		//GameObject.DestroyImmediate(partCtrl.MyEditPartAccesstor.gameObject);
		partCtrl.DeltePartAccessorObject();
		//partCtrl.MyEditPartAccesstor = null;
	}



	// ------------------ //   
	// --- 私有方法
	// ------------------ //
	protected override void OnLoadedArchive(Archive archive)
	{
		foreach (PlayerPartCtrlData item in archive.playerPartList)
		{
			PlayerPartSuperFactory.CreatePlayerEditPart(item);
		}
	}

	protected override void OnSaveingArchive(Archive archive)
	{
		archive.playerPartList = new List<PlayerPartCtrlData>(AllPlayerPartCtrls.Count);
		for (int i = 0; i < AllPlayerPartCtrls.Count; i++)
		{
			archive.playerPartList.Add(AllPlayerPartCtrls[i].GetCoreDataCopy);
		}
	}

	protected override void OnResetData()
	{
		AllPlayerPartCtrls.Clear();
		PartCtrlToColliderList.Clear();
		ColliderToPartCtrl.Clear();
		AllColliders.Clear();
	}

	private void RegistCtrlData(PlayerPartCtrl partCtrl)
	{
        //Debug.Log("注册零件" + partCtrl.MyPartType);
        //Remove(part);
        if (!AllPlayerPartCtrls.Contains(partCtrl))
		{
			AllPlayerPartCtrls.Add(partCtrl);
		}
		if (!PartCtrlToColliderList.ContainsKey(partCtrl))
		{
			PartCtrlToColliderList.Add(partCtrl, partCtrl.MyEditPartAccesstor.AllColliders);
		}
		for (int i = 0; i < partCtrl.MyEditPartAccesstor.AllColliders.Count; i++)
		{
			//if (partCtrl.MyEditPartAccesstor.AllColliders[i].gameObject.activeSelf == false)
			//{
			//	continue;
			//	// 没有启用的对象碰撞器组件不注册
			//}
			AllColliders.Add(partCtrl.MyEditPartAccesstor.AllColliders[i]);
			if (!ColliderToPartCtrl.ContainsKey(partCtrl.MyEditPartAccesstor.AllColliders[i]))
			{
				ColliderToPartCtrl.Add(partCtrl.MyEditPartAccesstor.AllColliders[i], partCtrl);
			}
		}
	}

	private void UnRegistCtrlData(PlayerPartCtrl partCtrlData)
	{
		//Debug.LogError($"开始删除零件 - {partCtrlData}");
		int deleteColliderAmount = 0;
		if (!PartCtrlToColliderList.ContainsKey(partCtrlData))
		{
			return;
		}
		foreach (Collider2D item in PartCtrlToColliderList[partCtrlData])
		{
			if (item == null)
			{
				Debug.LogError("错误");
			}
			deleteColliderAmount++;
			ColliderToPartCtrl.Remove(item);
			AllColliders.Remove(item);
		}
		PartCtrlToColliderList.Remove(partCtrlData);
		AllPlayerPartCtrls.Remove(partCtrlData);
		Debug.Log($"完成删除零件 - {deleteColliderAmount} 个刚体");
		//if (deleteColliderAmount == 0)
		//{
		//	Debug.LogError("删除刚体为零，检查代码");
		//	UnityEditor.EditorApplication.isPaused = true;
		//}
	}


}
