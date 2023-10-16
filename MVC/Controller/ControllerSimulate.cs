using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ControllerSimulate
{
	// ----------------//
	// --- 序列化
	// ----------------//


	// ----------------//
	// --- 公有成员
	// ----------------//
	public static ControllerSimulate Instance = new ControllerSimulate();


	// ----------------//
	// --- 私有成员
	// ----------------//
	private ChildViewSimulate_Command CmdView
	{
		get
		{
			if (_cmdView == null)
				_cmdView = GameObject.FindObjectOfType<ChildViewSimulate_Command>();
			return _cmdView;
		}
	}
	private ChildViewSimulate_Command _cmdView;
	private ModelSimulate Model => ModelSimulate.Instance;
	private Action SimulateProgressValueAddSetter;
	// ----------------//
	// --- 私有方法
	// ----------------//
	/// <summary>
	/// 创建所有物理零件
	/// 禁用状态下创建
	/// </summary>
	private IEnumerator Progress_CreatePhysicsPart(float waitSecondsEachPart)
	{
		Debug.Assert(ParentsManager.Instance.ParentOfPhysicsParts.activeSelf == true, "必须保持父物体被激活");
		List<PlayerPartCtrl> allPlayerParts = new List<PlayerPartCtrl>(PlayerPartManager.Instance.AllPlayerPartCtrls); //拷贝所有元素
		allPlayerParts.Sort((a, b) => { return a.MyPartType - b.MyPartType; });
		foreach (PlayerPartCtrl ctrlDataItem in allPlayerParts)
		{
			PlayerPartSuperFactory.CreatePhysicsPart(ctrlDataItem);
			PlayerPartBase part = ctrlDataItem.MyPhysicsPart;
			part.transform.SetParent(ParentsManager.Instance.ParentOfPhysicsParts.transform);
			//part.gameObject.SetActive(false); // 为了逐渐创建的效果禁用
			foreach (var item in part.AllActivedRigids)
			{
				item.simulated = false;
			}
			//Debug.LogError($"逐帧创建物理零件 当前帧：{Time.frameCount} 当前零件序号：{testIndex++}");
			GameMessage.Instance.PrintMessageAtScreenCenter($"创建零件 {ctrlDataItem.MyPartType}");
			yield return new WaitForSeconds(waitSecondsEachPart);
			SimulateProgressValueAddSetter();
		}
	}

	private GameObject CreateSliderCmdUI(ModelingExpressionNode node, int keyIndex)
	{
		var expression = node.Expression as Expression_SliderCmd;
		var createdScrollerObject = CmdView.CreateSliderCmd();
		CmdSlider slider = createdScrollerObject.GetComponentInChildren<CmdSlider>();
		slider.onValueChanged.AddListener((float value) =>
		{
			ModelSimulate.Instance.CommandDataCache[createdScrollerObject] = value;
		});
		slider.wholeNumbers = false;
		slider.minValue = node.SettingsValue.SettingValue[0]; ;
		slider.maxValue = node.SettingsValue.SettingValue[1]; ;
		slider.CmdKey = GetCmdKey(keyIndex);
		slider.onValueChanged.AddListener(TestSliderBug);
		return createdScrollerObject;
	}

	private GameObject CreateToggleCmdUI(ModelingExpressionNode node, int keyIndex)
	{
		var expression = node.Expression as Expression_ToggleCMD;
		var createdScrollerObject = CmdView.CreateToggleCmd();
		CmdToggle tog = createdScrollerObject.GetComponentInChildren<CmdToggle>();
		tog.onValueChanged.AddListener((bool b) =>
		{
			Debug.LogError($"通知数据修改-目标{expression.FuncDetail}，数据{b}");
			ModelSimulate.Instance.CommandDataCache[createdScrollerObject] = b ? 1 : 0;
		});
		tog.CmdKey = GetCmdKey(keyIndex);
		return createdScrollerObject;
	}

	private Key GetCmdKey(int index)
	{
		List<Key> keys = new List<Key>() { Key.A, Key.S, Key.D, Key.F, Key.G, Key.H, Key.J, Key.K, Key.L };
		if (index >= keys.Count)
		{
			Debug.LogError("按键不够");
			return Key.A;
		}
		return keys[index];
	}

	private void TestSliderBug(float value)
	{
		Debug.Log($"Slider == {value}");
	}
	// ----------------//
	// --- 公有方法
	// ----------------//
	///// <summary>
	///// 测试零件是否超出边界
	///// </summary>
	///// <param name="beyoundPartList"></param>
	///// <returns></returns>
	//public (bool, List<Vector3[]>) TestBeyongBound()
	//{ 
	//	List<Vector3[]> result = new List<Vector3[]>();
	//	result.AddRange(TransportLevel.Instacne.TryGetBeyondBoundaryColliders().Item2);
	//	return (result.Count > 0, result);
	//}

	public void CreateCommanderUI()
	{
		ModelSimulate.Instance.CommandDataCache.Clear();
		int cmdIndex = 0;
		foreach (PlayerPartCtrl item in PlayerPartManager.Instance.AllPlayerPartCtrls)
		{
			if (item.IsProgrammablePart && item.ModelMap != null && item.ModelMap.AllNodes.Count != 0)
			{
				foreach(var nodeItem in (item).ModelMap.AllNodes)
				{
					if (nodeItem.Expression.SortType == ExpressionBase.SorttingType.CMD)
					{
						switch (nodeItem.Expression.ID)
						{
							case ExpressionID.CMD_SLIDER:
								ModelSimulate.Instance.CommandDataCache.Add(CreateSliderCmdUI(nodeItem, cmdIndex++), nodeItem);
								break;
							case ExpressionID.CMD_TOGGLE:
								ModelSimulate.Instance.CommandDataCache.Add(CreateToggleCmdUI(nodeItem, cmdIndex++), nodeItem);
								break;
							default:
								break;
						}
					}
				}
			}
		}
	}


	public IEnumerator Cor_DeletePhysicsClones()
	{
		ParentsManager.Instance.ParentOfPhysicsParts.SetActive(false);
		ParentsManager.Instance.ParentOfPhysicsConnection.SetActive(false);
		if (ParentsManager.Instance.ParentOfPhysicsParts.transform.childCount + ParentsManager.Instance.ParentOfPhysicsConnection.transform.childCount > 100)
		{
			while (ParentsManager.Instance.ParentOfPhysicsParts.transform.childCount > 0)
			{
				// destroy会延迟删除，但始终在当前帧内
				GameObject.Destroy(ParentsManager.Instance.ParentOfPhysicsParts.transform.GetChild(0).gameObject);
				yield return 0;
			}
			while (ParentsManager.Instance.ParentOfPhysicsConnection.transform.childCount > 0)
			{
				GameObject.Destroy(ParentsManager.Instance.ParentOfPhysicsConnection.transform.GetChild(0).gameObject);
				yield return 0;
			}
		}
		else
		{
			GameObject.DestroyImmediate(ParentsManager.Instance.ParentOfPhysicsParts);
			GameObject.DestroyImmediate(ParentsManager.Instance.ParentOfPhysicsConnection);
		}
	}

	/// <summary>
	/// 创建物理模拟内容
	/// </summary>
	public IEnumerator ProgressCreateSimulation(Action progressValueSetter, Action<int> progressMaxValueSetter)
	{
		if (ModelSimulate.Instance.IsSimulating)
		{
			GameMessage.Instance.PrintMessageAtScreenCenter("正在运行");
			yield break;
		}

		//if (PartManager.Instance.GetConflictPart(out List<(BasePartCtrl, BasePartCtrl)> conflictPartList))
		//{
		//	foreach ((BasePartCtrl, BasePartCtrl) itemConflictPartPair in conflictPartList)
		//	{
		//		itemConflictPartPair.Item1.IgnoreCollision(itemConflictPartPair.Item2);
		//	}
		//}
		// 边界检测 暂时停用
		bool runnable = LevelProgressBase.Instance.CanRun(out Vector2[] result);
		//if (!runnable)
		//{
		//	GameMessage.Instance.PrintMessageAtScreenCenter("无法运行，超出边界");
		//	GameMessage.Instance.ShowErrorArea(result[0]);
		//	yield break;
		//}

		// 关闭所有场景零件的双击响应
		foreach (KeyValuePair<Hash128, ScenePart> itemScenePart in LevelProgressBase.Instance.AllSceneParts)
		{
			itemScenePart.Value.DoubleClickCmpnt.enabled = false;
			itemScenePart.Value.SetAsPhysics();
		}
		PlayerManager.Instance.Player.EnableControl = false;

		// 关闭编辑轴承图标
		ParentsManager.Instance.ParentOfEditBearing.SetActive(false);

		const float waitSeconds = 0.05f;
		progressMaxValueSetter(PlayerPartManager.Instance.AllPlayerPartCtrls.Count * 2 + PartConnectionManager.Instance.AllEditConnection.Count);
		SimulateProgressValueAddSetter = progressValueSetter;
		// 创建物理模拟对象
		// 1：在禁用物理零件父物体的状态下，逐帧创建所有零件并激活
		// 2：在显示物理零件父物体的状态下，单帧处理零件碰撞
		// 3：在显示物理零件父物体的状态下，关闭SetActive(false)所有物理零件
		// 4：在显示物理零件父物体的状态下，逐帧激活所有零件
		// 一：分步创建物理零件，但禁用物理模拟
		ParentsManager.Instance.ParentOfPhysicsParts.SetActive(true);    // 禁用父物体
		GameMessage.Instance.PrintMessageAtScreenCenter("0正在逐步创建零件");
		yield return Progress_CreatePhysicsPart(waitSeconds); // 逐帧创建所有物理零件

		// 保持编辑零件父物体及其所有子物体零件激活，才能检测冲突
		// 处理零件之间的碰撞，在一帧内完成，否则激活的物理零件会冲突导致位移
		//ParentsManager.Instance.ParentOfEditParts.SetActive(true);
		//GameMessage.Instance.PrintMessageAtScreenCenter("正在处理冲突");
		//IgnoreConflictPartInThisFrame();
		//GameMessage.Instance.PrintMessageAtScreenCenter("冲突处理完成");

		// 单独构建一个所有物理零件的列表
		List<PlayerPartBase> allPhysicsPart = new List<PlayerPartBase>(PlayerPartManager.Instance.AllPlayerPartCtrls.Count);
		foreach (var item in PlayerPartManager.Instance.AllPlayerPartCtrls) allPhysicsPart.Add(item.MyPhysicsPart);

		// 在启用物理零件父物体的情况下逐步激活并冻结（位移）
		GameMessage.Instance.PrintMessageAtScreenCenter("1正在逐步显示冻结零件");
		yield return Cor_FreezePart(allPhysicsPart, waitSeconds*2);

		// 创建物理连接
		GameMessage.Instance.PrintMessageAtScreenCenter("2正在逐步创建连接");
		yield return PartConnectionManager.Instance.CreatePhysicsConnections(waitSeconds, SimulateProgressValueAddSetter);

		//LayerWorker.Instance.Wrok(false);

		// 关闭编辑零件（冲突检测无需mono激活）
		ParentsManager.Instance.ParentOfEditParts.SetActive(false);

		// 解冻所有零件位移
		GameMessage.Instance.PrintMessageAtScreenCenter("3激活零件");
		UnFreezePart(allPhysicsPart);

		LevelProgressBase.Instance.StartPlay();
		Model.IsSimulating = true;
		SimulateProgressValueAddSetter = null;
		PlayerManager.Instance.Player.EnableControl = true;
	}

	/// <summary>
	/// 获取冲突零件，并处理所有冲突
	/// 在当前帧内解决
	/// </summary>
	//private void IgnoreConflictPartInThisFrame()
	//{
	//	if (PartManager.Instance.GetConflictPart(out List<(BasePartCtrl, BasePartCtrl)> conflictPartList))
	//	{
	//		foreach ((BasePartCtrl, BasePartCtrl) itemConflictPartPair in conflictPartList)
	//		{
	//			itemConflictPartPair.Item1.IgnoreCollision(itemConflictPartPair.Item2);
	//		}
	//	}
	//}

	/// <summary>
	/// 逐步激活所有物理零件
	/// 针对齿轮的特殊处理
	/// 多个齿轮连级啮合在一开始会导致卡顿，转动后消失，此方位为此问题而创建
	/// </summary>
	/// <param name="allPhysicsPart"></param>
	/// <returns></returns>
	private IEnumerator Cor_FreezePart(List<PlayerPartBase> allPhysicsPart, float waitSeconds)
	{
		//// 激活父物体
		////ParentsManager.Instance.ParentOfPhysicsParts.SetActive(true);
		//// 逐个禁用所有物理零件
		//foreach (var item in allPhysicsPart)
		//	item.gameObject.SetActive(false);
		//激活父物体
		ParentsManager.Instance.ParentOfPhysicsParts.SetActive(true);// !!!应该是这样
		
		// 逐帧启用所有物理零件，冻结位移，自由旋转
		foreach (var itemPart in allPhysicsPart)
		{
			GameMessage.Instance.PrintMessageAtScreenCenter($"显示零件 {itemPart.MyCtrlData.MyPartType}");
			itemPart.gameObject.SetActive(true);
			foreach (var itemPartRigid in itemPart.AllActivedRigids)
			{
				itemPartRigid.simulated = true;
				itemPartRigid.constraints = RigidbodyConstraints2D.FreezePosition;
				itemPartRigid.gravityScale = 0;
			}
			itemPart.gameObject.SetActive(true);
			SimulateProgressValueAddSetter();
			yield return new WaitForSeconds(waitSeconds);
		}
	}

	/// <summary>
	/// 解冻所有位移
	/// </summary>
	/// <param name="allPhysicsPart"></param>
	private void UnFreezePart(List<PlayerPartBase> allPhysicsPart)
	{
		foreach (var itemPart in allPhysicsPart)
		{
			//GameMessage.print($"解除冻结 {item.MyCtrlData.MyPartType}"); // 解除冻结后 似乎就好了
			GameMessage.Instance.PrintMessageAtScreenCenter($"解除冻结 {itemPart.MyCtrlData.MyPartType}");
			foreach (var itemPartRigid in itemPart.AllActivedRigids)
			{
				itemPartRigid.constraints = RigidbodyConstraints2D.None;
				itemPartRigid.gravityScale = 1;
				itemPartRigid.simulated = true;
			}
			SimulateProgressValueAddSetter();
			//yield return new WaitForSeconds(waitSeconds);
		}
	}

	private void IgnoreRigidColliders(Rigidbody2D a, Rigidbody2D b)
	{
		Collider2D[] aColliders = new Collider2D[128];
		Collider2D[] bColliders = new Collider2D[128];
		int aCollidersCount = a.GetAttachedColliders(aColliders);
		int bCollidersCount = b.GetAttachedColliders(bColliders);

		for (int i = 0; i < aCollidersCount; i++)
		{
			Collider2D itemACollider = aColliders[i];
			for (int j = 0; j < bCollidersCount; j++)
			{
				Collider2D itemBCollider = bColliders[j];
				Physics2D.IgnoreCollision(itemACollider, itemBCollider);
			}
		}
	}
}
