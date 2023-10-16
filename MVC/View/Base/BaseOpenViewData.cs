using UnityEngine;

public class BaseOpenViewData 
{
	public static readonly BaseOpenViewData Empty = new BaseOpenViewData();
}

public class ViewCreateOpenData : BaseOpenViewData
{

}
public class ViewEditOpenData : BaseOpenViewData
{
	public ViewEditOpenData(PlayerPartCtrl  playerPartCtrl) 
	{
		PlayerPartCtrl = playerPartCtrl;
	}
	public readonly PlayerPartCtrl PlayerPartCtrl;
}
public class ViewModelingOpenData : BaseOpenViewData
{
	public ViewModelingOpenData(PlayerPartCtrl partCtrl)
	{
		PartCtrl = partCtrl;
	}
	public readonly PlayerPartCtrl PartCtrl;
}
public class ViewPartGroupOpenData : BaseOpenViewData
{
	public ViewPartGroupOpenData(BasePartCtrl partCtrl)
	{
		PartCtrl = partCtrl;
	}
	public readonly BasePartCtrl PartCtrl;
}
public class ViewScenePartOpenData : BaseOpenViewData
{
	public	ViewScenePartOpenData(ScenePartCtrl partCtrl)
	{
		ScenePartCtrl = partCtrl;
	}
	public readonly ScenePartCtrl ScenePartCtrl;
}
public class ViewSimulateOpenData : BaseOpenViewData
{

}
public class ViewStartOpenData : BaseOpenViewData
{

}
