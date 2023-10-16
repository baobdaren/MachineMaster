using System.Collections.Generic;
using UnityEngine;

public class ScenePartManager : Loadable<ScenePartManager>
{
	// ------------- //
	// -- 序列化
	// ------------- //

	// ------------- //
	// -- 私有成员
	// ------------- //

	// ------------- //
	// -- 公有成员
	// ------------- //
	public Dictionary<Hash128, ScenePartCtrl> AllScenePartCtrls { get; private set; } = new Dictionary<Hash128, ScenePartCtrl>();

	// ------------- //
	// -- Unity 消息
	// ------------- //

	// ------------- //
	// -- 公有方法
	// ------------- //

	// ------------- //
	// -- 私有方法
	// ------------- //
	protected override void OnLoadedArchive(ArchiveManager.Archive archive)
	{
		Dictionary<Hash128, ScenePart> allSceneParts = LevelProgressBase.Instance.AllSceneParts;
		AllScenePartCtrls = new Dictionary<Hash128, ScenePartCtrl>(allSceneParts.Count);
		// 记录所有场景零件的控制器
		foreach (KeyValuePair<Hash128, ScenePart> itemSceneParts in allSceneParts)
		{
			AllScenePartCtrls.Add(itemSceneParts.Key, itemSceneParts.Value.MyCtrl);
			PartManager.Instance.RegistPartDragingListener(itemSceneParts.Value.MyCtrl);
			PartManager.Instance.RegistPartDoubleClickListener(itemSceneParts.Value.MyCtrl);
		}
		List<ScenePartCtrlData> scenePartCtrlDataArchive = archive.scenePartList;
		if (scenePartCtrlDataArchive != null)
		{
			// 为具有存档的零件应用存档
			foreach (ScenePartCtrlData itemScenePartCtrlDataArchive in scenePartCtrlDataArchive)
			{
				if (allSceneParts.TryGetValue(itemScenePartCtrlDataArchive.PartID, out ScenePart result))
				{
					result.MyCtrl = new ScenePartCtrl(result, itemScenePartCtrlDataArchive);
				}
			}
		}
	}

	protected override void OnSaveingArchive(ArchiveManager.Archive archive)
	{
		archive.scenePartList = new List<ScenePartCtrlData>(AllScenePartCtrls.Count);
		foreach (KeyValuePair<Hash128, ScenePartCtrl> itemAllScenePart in AllScenePartCtrls)
		{
			archive.scenePartList.Add(itemAllScenePart.Value.GetDataClone);
		}
	}

	protected override void OnResetData()
	{
		AllScenePartCtrls.Clear();
	}
}
