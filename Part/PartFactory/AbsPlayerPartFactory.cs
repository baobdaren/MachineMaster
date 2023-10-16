using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.U2D;

public abstract class AbsPlayerPartFactory
{
	// ----------------//
	// --- 序列化 s
	// ----------------//


	// ----------------//
	// --- 公有成员
	// ----------------//

	/// !!!不需要再次实例化单例，访问超级工厂即可，超级工厂调用其他工厂实例
	//public static AbsPartFactory Intsance { get => SuperFactory.GetFactor<> }


	// ----------------//
	// --- 私有成员
	// ----------------//

	// ----------------//
	// --- Unity消息
	// ----------------//
		

	// ----------------// 
	// --- 公有方法
	// ----------------//
	/// <summary>
	/// 编辑模式零件只有自带的Accessor
	/// </summary>
	/// <param name="partCtrl"></param>
	/// <returns></returns>
	public GameObject CreateEditPart(PlayerPartCtrl partCtrl)
	{
		Debug.Log("创建Edit零件 " + partCtrl.MyPartType);
		// 实例化预制体
		GameObject editPart = PartConfig.Instance.InstantiatePart(partCtrl.MyPartType, true);
		foreach (Transform item in editPart.GetComponentsInChildren<Transform>())
		{
			item.gameObject.layer = GameLayerManager.EditPart;
		}
		// 获取组件访问器
		partCtrl.MyEditPartAccesstor = editPart.GetComponentInChildren<AbsBasePlayerPartAccessor>(true);
		// 注册零件选择器
		//partCtrl.MyEditPartAccesstor.PartDragCmpnt.OnDragStart.AddListener(() =>
		//{
		//	PartSelector.Instance.SelectClickedPart(partCtrl);
		//});

		SetAsEditPart(partCtrl);
		return partCtrl.MyEditPartAccesstor.gameObject;
	}

	/// <summary>
	/// 创建物理模拟模式的零件，物理模式零件需要实例化后添加PartBase组件
	/// </summary>
	/// <param name="part"></param>
	/// <returns></returns>
	public PlayerPartBase CreatePhysicsPart(PlayerPartCtrl partCtrl)
	{
		Debug.Log($"创建Physics零件 {partCtrl.MyPartType} 尺寸 {partCtrl.Size}");
		// 实例化
		GameObject physicsPart = PartConfig.Instance.InstantiatePart(partCtrl.MyPartType, false);
		// 物理零件需要PartBase
		partCtrl.MyPhysicsPart = PartConfig.Instance.AddPartBehaviorCmpnt(partCtrl.MyPartType, physicsPart, partCtrl);
		// 组件访问器
		partCtrl.MyPhysicsPart.Accessor = partCtrl.MyPhysicsPartAccesstor;
		//partCtrl.MyPhysicsPart.Accessor.MyPartCtrl = partCtrl;
		SetAsPhysicsPart(partCtrl, partCtrl.MyPhysicsPart.Accessor);
		//ModifyPart(partCtrl, false);
		foreach (Transform item in physicsPart.GetComponentInChildren<Transform>(true))
		{
			item.gameObject.layer = GameLayerManager.GetPartGameObjectLayer(partCtrl.Layer);
		}

		return partCtrl.MyPhysicsPart;
	}


	/// <summary>
	/// 设置编辑零件的一些属性
	/// </summary>
	/// <param name="accestor"></param>
	/// <param name="asEdit"></param> 
	/// <returns></returns>
	public void ModifyPart(PlayerPartCtrl partCtrl)
	{
		ApplyPartCoreData(partCtrl, true);
	}
	// ----------------//   
	// --- 私有方法
	// ----------------//
	protected abstract void OnSetSize(PlayerPartCtrl partCtrlData, AbsBasePlayerPartAccessor accesstor);
	protected abstract void OnCreatedAsEdit(PlayerPartCtrl partCtrlData, AbsBasePlayerPartAccessor accesstor);
	protected abstract void OnCreatedAsPhysics(PlayerPartCtrl partCtrlData, AbsBasePlayerPartAccessor accesstor);

	private void SetAsEditPart(PlayerPartCtrl partCtrl)
	{
		AbsBasePlayerPartAccessor editAccessor = partCtrl.MyEditPartAccesstor;
		editAccessor.transform.name = partCtrl.MyPartType.ToString();
		editAccessor.PartDragCmpnt.enabled = true;
		editAccessor.PartDragCmpnt.OnDraging.AddListener(newPos => partCtrl.Position = newPos);
		// 这里注册  在哪取消注册？？？
		PartSnapManager.Instance.RegistSnapableTarget(editAccessor.PartDragCmpnt, partCtrl);
		MaterialPropertyBlock block = new MaterialPropertyBlock();
		partCtrl.SwitchEditMaterials(MaterialConfig.Instance.Part_Edit);
		OnCreatedAsEdit(partCtrl, editAccessor);
		ApplyPartCoreData(partCtrl, true);
	}

	private void SetAsPhysicsPart(PlayerPartCtrl partCtrl, AbsBasePlayerPartAccessor part)
	{
		partCtrl.SwitchPhysicsMaterials(MaterialConfig.Instance.Part_Physics);
		foreach (var item in part.GetComponentsInChildren<PartLed>())
		{
			item.TurnOn(partCtrl);
		}
		GameObject.Destroy(part.PartDragCmpnt);
		OnCreatedAsPhysics(partCtrl,part);
		ApplyPartCoreData(partCtrl, false);
	}

	/// <summary>
	/// 设置零件属性
	/// 该方法也可以修改物理零件属性，修改物理零件属性外部不调用
	/// </summary>
	/// <param name="ctrlData"></param>
	/// <param name="asEdit"></param>
	private void ApplyPartCoreData(PlayerPartCtrl ctrlData, bool asEdit)
	{
		Debug.Assert(ctrlData != null);
		AbsBasePlayerPartAccessor editAccestor = (asEdit ? ctrlData.MyEditPartAccesstor : ctrlData.MyPhysicsPartAccesstor);
		OnSetSize(ctrlData, editAccestor);
		ApplyPositionAndRotation(ctrlData, editAccestor);
		ApplyColor(ctrlData, editAccestor);
		ApplyLayer(ctrlData, editAccestor);
	}

	protected virtual void ApplyLayer(PlayerPartCtrl partCtrl, AbsBasePlayerPartAccessor accesstor)
	{
		foreach (var item in accesstor.AllRenders)
		{
			item.sortingLayerID = GameLayerManager.GetPartRenderLayers(partCtrl.Layer);
		}
	}

	protected virtual void ApplyColor(PlayerPartCtrl partCtrl, AbsBasePlayerPartAccessor editAccesstor)
	{

	}

	protected virtual void ApplyPositionAndRotation(PlayerPartCtrl partCtrl, AbsBasePlayerPartAccessor accesstor)
	{
		accesstor.transform.SetPositionAndRotation(partCtrl.Position, partCtrl.Rotation);
	}

	// ----------------//
	// --- 类型
	// ----------------//
	public enum PartEdittingType { NORMAL, SELECT, DRAG }
}
