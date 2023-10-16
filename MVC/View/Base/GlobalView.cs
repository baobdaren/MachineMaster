using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

/// <summary>
/// 全局类视图,这类视图在其他主视图打开时同时打开或关闭
/// </summary>
public abstract class GlobalView : BaseView
{
	// ------------- //
	// -- 序列化
	// ------------- //

	// ------------- //
	// -- 私有成员
	// ------------- //
	protected override CameraActor.CameraWorkStates? GetCameraWorkStateInThisView => null;
	protected abstract List<string> FollowViewNames { get; }
	// ------------- //
	// -- 公有成员
	// ------------- //

	// ------------- //
	// -- Unity 消息
	// ------------- //

	// ------------- //
	// -- 公有方法
	// ------------- //

	// ------------- //
	// -- 私有方法
	// ------------- //
	public override void SwitchView(BaseView openningView, BaseView exitingView, bool suspendLast, BaseOpenViewData openData)
	{
		//base.SwitchView(openningView, exitingView, suspendLast);

		bool inFollwoViews = exitingView != null && FollowViewNames.Contains(exitingView.GetViewName);
		bool enterFollowViews = FollowViewNames.Contains(openningView.GetViewName);

		if (DisplayingState != DisplayingStates.Closed && inFollwoViews && !enterFollowViews) ExitView(openData);
		if (DisplayingState == DisplayingStates.Closed && !inFollwoViews && enterFollowViews) EnterView(openData);
	}
}
