using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasePartConnection: ILayerSettable
{
	// ------------- //
	// -- 私有成员
	// ------------- //
	protected Vector2 _position;

	// ------------- //
	// -- 公有成员
	// ------------- //
	public Vector2 AnchorPosition
	{
		get
		{
			return _position;
		}
		set 
		{
			_position = value;
			if(EditBearing != null)
			{
				EditBearing.transform.position = _position;
			}
		}
	}

	public EditBearing EditBearing
	{
		protected set; get;
	}

	public PhysicsBearing BearingPhysics
	{
		get;set;
	}

	/// <summary>
	/// (连接到此轴承的零件:连接所属零件的刚体序号)
	/// 
	/// </summary>
	public Dictionary<IConnectableCtrl, ConnectionDatas> ConnectedParts
	{ get; protected set; } = new ();

	public int SetOrder
	{
		set
		{
			int startIndex = value;
			foreach (var item in EditBearing.GetComponentsInChildren<SpriteRenderer>())
			{
				item.sortingLayerID = SortingLayer.NameToID("Physics");
				item.sortingOrder = startIndex++;
			}
			Debug.Assert(startIndex - value < 100);
		}
	}

	// ------------- //
	// -- 公有方法
	// ------------- //
	public BasePartConnection(Vector2 anchorPos)
	{
		AnchorPosition = anchorPos;
	}

	// ------------- //
	// -- 私有方法
	// ------------- //
	public struct ConnectionDatas
	{
		public ConnectionDatas(int connectColliderIndex, bool isFixed)
		{
			ConnectColliderIndex = connectColliderIndex;
			IsFixed = isFixed;
		}
		public int ConnectColliderIndex;
		public bool IsFixed;
	}
}
