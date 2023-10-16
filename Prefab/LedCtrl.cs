using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedCtrl : MonoBehaviour
{
    // ----------------//
    // --- 序列化
    // ----------------//
    [SerializeField]
    GameObject _originObject;

    // ----------------//
    // --- 公有成员
    // ----------------//
    public int Amount
    {
        set
        {
			if(value < 0)
			{
                value = 0;
			}
			while(LedList.Count < value)
			{
                LedList.Add(GameObject.Instantiate(_originObject, _originObject.transform.parent));
			}
			while(LedList.Count > value)
			{
                var removeObj = LedList[0];
                LedList.RemoveAt(0);
                GameObject.Destroy(removeObj);
			}
            _amount = value;
        }
        get => _amount;
    }
    public float FlashStepTime
    {
        get; set;
    } = 1;


    // ----------------//
    // --- 私有成员
    // ----------------//
    private int _amount;
    private List<GameObject> LedList
    {
        get
        {
			if(_ledList == null)
			{
                _ledList = new List<GameObject>() { _originObject };
			}
            return _ledList;
        }
    }
    private List<GameObject> _ledList;


	// ----------------//
	// --- Unity消息
	// ----------------//
	private void Awake ( )
	{
        _originObject.gameObject.SetActive(false);
	}
	void Start()
    {
        
    }

	private void OnEnable ( )
	{
        StartCoroutine(LedsUpdate());
	}


    // ----------------//
    // --- 公有方法
    // ----------------//
    IEnumerator LedsUpdate ( )
    {
        int flashIndex = 0;
        int lastIndex = flashIndex;
		foreach(var item in LedList)
		{
            item.gameObject.SetActive(true);
		}
		while(true)
		{
            yield return new WaitForSeconds(Mathf.Max(0.1f, Mathf.Abs(FlashStepTime)));
			if(flashIndex == LedList.Count && FlashStepTime > 0)
			{
                flashIndex = 0;
			}
			else if(flashIndex == -1 && FlashStepTime < 0)
			{
                flashIndex = LedList.Count - 1;
			}
            LedList[lastIndex].GetComponent<SwitchableImage>().IsOpen = false;
            LedList[flashIndex].GetComponent<SwitchableImage>().IsOpen = true;
            lastIndex = flashIndex;
			flashIndex += FlashStepTime > 0 ? 1 : -1;
		}
    }


	// ----------------//
	// --- 私有方法
	// ----------------//

}
