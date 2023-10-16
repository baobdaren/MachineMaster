using System.Collections;
using TMPro;
using UnityEngine;
using static PartModelingNodeMap;

public class SymbolIOFace : MonoBehaviour
{
	// ---------------- //
	// -- 序列化
	// ---------------- //
	[SerializeField]
	private Color _inputColor;
	[SerializeField]
	private Color _outputColor;
	[SerializeField]
	private TextMeshPro _texData;
	[SerializeField]
	private TextMeshPro _texTitle;

	// ---------------- //
	// -- 公有成员
	// ---------------- //
	public Symbol MySymbol
	{
		private set
		{
			if(_symbol == null)
			{
				_symbol = value;
			}
		}
		get => _symbol;
	}
	public bool IsInputFace
	{
		private set;
		get;
	}
	public int IOIndex { private set; get; }

	// ---------------- //
	// -- 私有成员
	// ---------------- //
	private CircleCollider2D _circle;
	private MouseEnterOverExit _mouseSwipe;
	private Symbol _symbol;

	// ---------------- //
	// -- Unity消息
	// ---------------- //
	private void Awake ( )
	{
		if (!gameObject.TryGetComponent ( out _circle))
			_circle = gameObject.AddComponent<CircleCollider2D> ( );
		if (!gameObject.TryGetComponent(out _mouseSwipe))
			_mouseSwipe = gameObject.AddComponent<MouseEnterOverExit>();
		//if (!gameObject.TryGetComponent(out MouseDoubleClick doubleClickCMpnt))
		//	doubleClickCMpnt = gameObject.AddComponent<MouseDoubleClick>();
		//doubleClickCMpnt.OnDoubleClick.AddListener(() =>
		//{
		//	ModelModeling.Instance.UsingPart.ModelMap.DeleteNodeConnection(MySymbol.GetMyNode, IOIndex, IsInputFace);
		//	ModelModeling.Instance.UsingPart.ModelMap.DeleteConnection(n)
		//}
		//);
	}

	private void Start ( )
	{
		_mouseSwipe.OnPressEnter.AddListener(OnEnter);
		//_mouseSwipe.OnOver.AddListener(OnOver);
		_texData.gameObject.SetActive(!IsInputFace);
	}

	//private void Update()
	//{
	//	_texData.SetText(MySymbol.MyNode.GetResult[0].ToString());
	//}

	// ---------------- //
	// -- 公有方法
	// ---------------- //
	public void InitIOFace (Symbol belongToSymbol, bool isInputIO, int ioIndex )
	{
		MySymbol = belongToSymbol;
		IsInputFace = isInputIO;
		IOIndex = ioIndex;
		SpriteRenderer sp = GetComponentInChildren<SpriteRenderer>();
		if (isInputIO)
		{
			_texTitle.text = InputTitle[Mathf.Clamp(ioIndex, 0, InputTitle.Length)];
			sp.color = _inputColor;
		}
		else
		{
			_texTitle.text = OutputTitle[Mathf.Clamp(ioIndex, 0, OutputTitle.Length)];
			sp.color = _outputColor;
		}
	}

	public bool IsOverlapMouse
	{
		get
		{
			return _circle.OverlapPoint(CameraActor.Instance.MouseWorldPos);
		}
	}

	// ---------------- //
	// -- 私有方法
	// ---------------- //
	private void OnEnter()
	{
		//if (ConnectToSymbol == null)
		{
			if (SymbolConnector.Instance.FirstSelectedSymbolIO == null)
			{
				Debug.LogError(name + "设置为起始符号");
				SymbolConnector.Instance.FirstSelectedSymbolIO = this;
			}
			else if(SymbolConnector.Instance.FirstSelectedSymbolIO != this)
			{
				Debug.LogError(name + "设置为终止符号");
				SymbolConnector.Instance.SecondSelectedSymbolIO = this;
			}
		}
	}
	//private void OnOver()
	//{
	//	if (/*g.touchCount == 1 && */ConnectToSymbol == null && SymbolConnectAction.Instance.StartSymbolFace != null)
	//	{
	//		SymbolConnectAction.Instance.EndSymbolFace = this;
	//	}
	//}
	//private IEnumerator Cor_UpdateLinePosition ( )
	//{
	//	yield return new WaitForSeconds(UnityEngine.Random.Range(0.1f, 0.2f));
	//	while(ConnectToSymbol != null)
	//	{
	//		IOData.UpdatePosition();
	//		yield return new WaitForSeconds(0.05f);
	//	}
	//}

	private static readonly string[] InputTitle = new string[] { "I1", "I2", "I3", "I4", "I5" };
	private static readonly string[] OutputTitle = new string[] { "O1", "O2", "O3", "O4", "O5" };
}