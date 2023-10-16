using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChildViewSimulate_Command : MonoBehaviour
{
    // ----------------//
    // --- 序列化
    // ----------------//
    [SerializeField]
    private GameObject _toggleCmdOrigin;
    [SerializeField]
    private GameObject _sliderCmdOrogin;


	// ----------------//
	// --- 公有成员
	// ----------------//


	// ----------------//
	// --- 私有成员
	// ----------------//


	// ----------------//
	// --- Unity消息
	// ----------------//
	private void Awake ( )
	{
        _toggleCmdOrigin.SetActive(false);
        _sliderCmdOrogin.SetActive(false);
	}


	// ----------------//
	// --- 公有方法
	// ----------------//
	public GameObject CreateSliderCmd ()
    {
        GameObject result = Instantiate(_sliderCmdOrogin, _sliderCmdOrogin.transform.parent);
        result.SetActive(true);
        return result;
    }
    public GameObject CreateToggleCmd ()
    {
        GameObject result = Instantiate(_toggleCmdOrigin, _toggleCmdOrigin.transform.parent);
        result.SetActive(true);
        return result;
    }


    // ----------------//
    // --- 私有方法
    // ----------------//

}
