using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasePartConnection: ILayerSettable
{
	// ------------- //
	// -- ˽�г�Ա
	// ------------- //
	protected Vector2 _position;

	// ------------- //
	// -- ���г�Ա
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
	/// (���ӵ�����е����:������������ĸ������)
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
	// -- ���з���
	// ------------- //
	public BasePartConnection(Vector2 anchorPos)
	{
		AnchorPosition = anchorPos;
	}

	// ------------- //
	// -- ˽�з���
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
