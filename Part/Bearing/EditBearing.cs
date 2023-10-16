using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 编辑状态下的光标对象
/// </summary>
public class EditBearing : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	// ----------------//
	// --- 序列化
	// ----------------//
	[SerializeField]
	private GameObject buttonsParent;
	[SerializeField]
	private Button buttonDeleteConnection;
	[SerializeField]
	private Button buttonConnectToBearing;
	[SerializeField]
	private Button buttonConnectFixed;
	[SerializeField]
	private Button buttonDisconnect;
	[SerializeField]
	private SpriteRenderer _displayIcon;

	/// <summary>
	/// 激活周围图标后，用鼠标移出此触发器，来决定关闭周围图标
	/// </summary>
	[SerializeField]
	private Collider2D displayingTrigger;
	// ----------------//
	// --- 公有成员
	// ----------------//
	// ----------------//
	// --- 私有成员
	// ----------------//
	private bool _shaftConnectable = false;
	private bool _fixedConnectable = false;
	private bool _deleteable = false;
	private bool _disconnectable = false;
	private bool PointerStayin { get => displayingTrigger.enabled; }


	// ----------------//
	// --- Unity消息
	// ----------------//
	private void Awake()
	{
		buttonDeleteConnection.onClick.AddListener(OnClick_DeleteConnection);
		buttonConnectFixed.onClick.AddListener(OnClick_FixedConnect);
		buttonConnectToBearing.onClick.AddListener(OnClick_ShaftConnect);
		buttonDisconnect.onClick.AddListener(OnClick_Disconnect);
	}

	private void Start()
	{
		StartCoroutine(Progress_RotateCircle());
	}

	void OnEnable()
	{
		buttonsParent.SetActive(false);
		displayingTrigger.enabled = false;
		buttonDeleteConnection.gameObject.SetActive(false);
		buttonConnectToBearing.gameObject.SetActive(false);
	}

	// ----------------//
	// --- 公有方法
	// ----------------//
	/// <summary>
	/// 轴承的显示状态
	/// 轴承的隐藏与否由父物体管理
	/// </summary>
	/// <param name="deleteable"></param>
	/// <param name="shaftConnectable"></param>
	public void SetButtonsDisplay(bool deleteable, bool shaftConnectable, bool fixedConnectable, bool disconnnectable)
	{
		//Debug.LogError($"功能 {(deleteable ? "" : "不")} 可删除 {(shaftConnectable ? "" : "不")} 可连接 { (disconnnectable ? "" : "不")} 可断开");
		_shaftConnectable = shaftConnectable;
		_fixedConnectable = fixedConnectable;
		_deleteable = deleteable;
		_disconnectable = disconnnectable;
		//      if (_connectable)
		//      {
		//          _displayIcon.color = ColorConfig.Instance.BearingConnectableColor * 0.88f;
		//      }
		//      else if (_deleteable)
		//      {
		//          _displayIcon.color = ColorConfig.Instance.BearingDeleteableColor * 0.88f;
		//      }
		//else
		//{
		//          _displayIcon.color = ColorConfig.Instance.BearingDisableColor;
		//}
		foreach (var item in GetComponentsInChildren<Rigidbody2D>())
		{
			item.simulated = true;
			item.bodyType = RigidbodyType2D.Static;
		}
		// 暂时没有对编辑状态的轴承设置SortingLayer，暂时设置在所有零件之上显示
		// 但这会导致一个问题，对于下层和中层的轴承，会显示在上层零件之上
		// 以后可能需要设计高亮轴承所连接的两个零件
		//gameObject.GetComponent<SpriteRenderer>().sortingLayerID = GameLayerManager.Instance.EditBearingSortingLayer;
		//DeleteIcon.GetComponent<SpriteRenderer>().sortingLayerID = GameLayerManager.Instance.EditBearingSortingLayer;
		// 设置时已经激活则刷新一下
		if (PointerStayin)
		{
			OnPointerEnter(null);
		}

		_displayIcon.color = Color.white * 0.80f;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		Debug.LogWarning("进入轴承图标");
		// 特殊显示被连接的零件
		PartMaterialSetter.Instance.FocusConnection = PartConnectionManager.Instance.AllEditConnection.Find(itemConn => itemConn.EditBearing == this);

		displayingTrigger.enabled = true;
		buttonsParent.gameObject.SetActive(true);
		//if (_connectable && _deleteable)
		//      {
		//          _displayIcon.color = ColorConfig.Instance.BearingConnectableColor;
		//      }
		//      else if(_deleteable)
		//      {
		//          _displayIcon.color = ColorConfig.Instance.BearingDeleteableColor;
		//      }
		_displayIcon.color = Color.white;
		buttonConnectToBearing.gameObject.SetActive(_shaftConnectable);
		buttonConnectFixed.gameObject.SetActive(_fixedConnectable);
		buttonDeleteConnection.gameObject.SetActive(_deleteable);
		buttonDisconnect.gameObject.SetActive(_disconnectable);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		Debug.LogWarning("退出轴承图标");
		_displayIcon.color = Color.white * 0.80f;

		PartMaterialSetter.Instance.FocusConnection = null;
		displayingTrigger.enabled = false;
		buttonsParent.gameObject.SetActive(false);
		buttonDeleteConnection.gameObject.SetActive(false);
		buttonConnectToBearing.gameObject.SetActive(false);
		buttonDisconnect.gameObject.SetActive(false);
	}

	//   public void OnPointerClick(PointerEventData eventData)
	//{
	//       //Debug.Assert(ModelEdit.Instance.GetConnectMain != null);
	//       if (_connectable && eventData.button == PointerEventData.InputButton.Left) // 左键点击链接
	//       {
	//           var part = ModelScenePart.Instance.EditingScenePart ?? ModelEdit.Instance.GetConnectMain;
	//           Debug.Assert(part != null);
	//           bool result = PartConnectionManager.Instance.TryConnectPartToBearing(part, this);
	//		if (result)
	//		{
	//               buttonConnectToBearing.gameObject.SetActive(false);
	//		}
	//           if (result == false)
	//           {
	//               GameMessage.Instance.PrintMessageAtScreenCenter("链接失败");
	//           }
	//       }
	//       else if (_deleteable && eventData.button == PointerEventData.InputButton.Right) // 右键点击删除
	//       { 
	//	    PartConnectionManager.Instance.DeleteConnection(this);
	//       }
	//       ModelEdit.Instance.DirtyFlag_BearingColor = true;
	//   }

	// ----------------//
	// --- 私有方法
	// ----------------//
	private IEnumerator Progress_RotateCircle()
	{
		while (gameObject)
		{
			yield return new WaitForSecondsRealtime(0.1f);
			if (buttonConnectToBearing.enabled)
			{
				buttonConnectToBearing.transform.eulerAngles += (Vector3.forward * 8);
			}
		}
	}

	private void OnClick_DeleteConnection()
	{
		if (_deleteable == false) return;
		OnPointerExit(null);
		PartConnectionManager.Instance.DeleteConnection(this);
	}

	private void OnClick_FixedConnect()
	{
		if (_fixedConnectable == false) return;
		OnClick_ConnectBearing(true);
	}

	private void OnClick_ShaftConnect()
	{
		if (_shaftConnectable == false) return;
		OnClick_ConnectBearing(false);
	}

	private void OnClick_ConnectBearing(bool isFixed)
	{
		var part = ModelScenePart.Instance.EditingScenePart ?? ModelPlayerPartEditor.Instance.GetConnectMain;
		Debug.Assert(part != null);
		bool result = PartConnectionManager.Instance.TryConnectPartToBearing(part, isFixed, this);
		if (result)
		{
			if (isFixed)
			{
				buttonConnectFixed.gameObject.SetActive(false);
			}
			else
			{
				buttonConnectToBearing.gameObject.SetActive(false);
			}
		}
		if (result == false)
		{
			GameMessage.Instance.PrintMessageAtScreenCenter("链接失败");
		}
		// 以当前编辑零件未更新对象
		PartConnectionManager.Instance.UpdateEditBearings(PartManager.Instance.EditingPart);
	}

	private void OnClick_Disconnect()
	{
		if (_disconnectable == false) return;
		var part = ModelScenePart.Instance.EditingScenePart ?? ModelPlayerPartEditor.Instance.GetConnectMain;
		PartConnectionManager.Instance.DisconnectPartFromBearing(this, part);
		PartConnectionManager.Instance.UpdateEditBearings(PartManager.Instance.EditingPart);
	}



	// ----------------//
	// --- 类型
	// ----------------//
}