using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentsManager
{
	// ------------------ //    
	// --- 序列化    
	// ------------------ //
	public static ParentsManager Instance = new ParentsManager();


	// ------------------ //    
	// --- 公有成员    
	// ------------------ //
	public GameObject ParentOfPhysicsConnection
	{
		get
		{
			if (_parentOfConnections == null)
			{
				_parentOfConnections = new GameObject("= PhysicsConnections =");
				_parentOfConnections.SetActive(false);
			}
			return _parentOfConnections;
		}
	}
	GameObject _parentOfConnections;

	public GameObject ParentOfPhysicsParts
	{
		get
		{
			if (_physicsPartParent == null)
				_physicsPartParent = new GameObject("= PhysicsParts =");
			return _physicsPartParent;
		}
	}
	private GameObject _physicsPartParent;

	public GameObject ParentOfEditParts
	{
		get
		{
			if (_partParent == null)
				_partParent = new GameObject("= EditParts =");
			return _partParent;
		}
	}
	private GameObject _partParent;

	public GameObject ParentOfCraetedSymbol
	{
		get
		{
			if (_symbolParent == null)
				_symbolParent = new GameObject("= ParentOfSymbol =");
			return _symbolParent;
		}
	}
	private GameObject _symbolParent;

	public GameObject ParentOfSymbolConnectLine
	{
		get
		{
			if (_symbolConnectLinesParent == null)
				_symbolConnectLinesParent = new GameObject("= ParentOfSymbolConnectLines =");
			return _symbolConnectLinesParent;
		}
	}
	private GameObject _symbolConnectLinesParent;

	/// <summary>
	/// 连接
	/// </summary>
	public GameObject ParentOfEditBearing
	{
		get
		{
			if (_bearingParent == null)
				_bearingParent = new GameObject("= BearingParentEdit =");
			return _bearingParent;
		}
	}
	private GameObject _bearingParent;

	//public GameObject ParentOfPhysicsBearing
	//{
	//	get
	//	{
	//		if (_parentOfPhysicsBearing == null)
	//		{
	//			_parentOfPhysicsBearing = new GameObject("Parent Of Physics Bearing");
	//		}
	//		return _parentOfPhysicsBearing;
	//	}
	//	set
	//	{
	//		_parentOfPhysicsBearing = value;
	//	}
	//}
	//private GameObject _parentOfPhysicsBearing;

	// ------------------ //   
	// --- 私有成员    
	// ------------------ //


	// ------------------ //    
	// --- Unity消息    
	// ------------------ //

	// ------------------ //    
	// --- 公有方法   
	// ------------------ //

	// ------------------ //   
	// --- 私有方法
	// ------------------ //
}
