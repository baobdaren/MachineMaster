using UnityEngine;

public abstract class AbsProgrammablePartBase : PlayerPartBase
{
	// ----------------//
	// --- 序列化
	// ----------------//

	// ----------------//
	// --- 公有成员
	// ----------------//
	public PartModelingNodeMap ModelMap 
	{
		get => MyCtrlData.ModelMap;
	}


	// ----------------//
	// --- 私有成员
	// ----------------//
	protected abstract ExpressionID GetRootExpression { get; }
	protected abstract AbsInterpreter GetInterpreter { get; }

	// ----------------//
	// --- Unity消息
	// ----------------//
	protected virtual void FixedUpdate()
	{
		SetInput(ModelMap.Caculate());
	}

	// ----------------//
	// --- 公有方法
	// ----------------//
	// 父类只负责对参数的解释器的原始数据进行修改，子类则从解释器获取解释后的参数
	public void SetInput(float[] args)
	{
		//Debug.LogError("修改可编程零件输入原始数据 " + args.ToString());
		GetInterpreter.SetInputData(args);
		On_InterpreterUpdated();
	}
	//public abstract void RunExpression ( InputSymbol input );
	//public abstract ExpressionID GetInputExpressionID ( );
	public abstract void On_InterpreterUpdated();
	// ----------------//
	// --- 私有方法
	// ----------------//


}
