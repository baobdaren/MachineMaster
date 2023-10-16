using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using UnityEngine.Events;
using Sirenix.OdinInspector;

[RequireComponent(typeof(Collider2D))]
public class Dialog: MonoBehaviour
{
	// ------------- //
	// -- 序列化
	// ------------- //
	[SerializeField]
	private GameObject TextBackGround;
	[SerializeField]
	private Collider2D playerEnterTriggle;
	[SerializeField]
	private List<AbsPlayerTask> PreviousTask;
	[SerializeField]
	private List<Dialog> PreviousDialog;
	[SerializeField]
	[ListDrawerSettings(NumberOfItemsPerPage = 20)]
	private List<DialogDate> dialogDates = new List<DialogDate>();
	[SerializeField]
	private TextMeshProUGUI textDialog;

	// ------------- //
	// -- 私有成员
	// ------------- //
	private int dialogIndex = 0;

	// ------------- //
	// -- 公有成员
	// ------------- //
	public bool Finished { get => dialogIndex >= dialogDates.Count; }

	// ------------- //
	// -- Unity 消息
	// ------------- //
	private void Awake()
	{
		if (playerEnterTriggle == null)
			playerEnterTriggle = GetComponentInChildren<Collider2D>(true);
		Debug.Assert(playerEnterTriggle.isTrigger == true, gameObject);
		TextBackGround.gameObject.SetActive(false);
	}

	private void Start()
	{
		StartCoroutine(Cor_Dialog());
	}

	private void Update()
	{
		transform.rotation = Quaternion.identity;
	}
	// ------------- //
	// -- 公有方法
	// ------------- //
	public void DialogForceFinish()
	{
		dialogIndex = dialogDates.Count;
	}

	// ------------- //
	// -- 私有方法
	// ------------- //
	private IEnumerator Cor_Dialog()
	{
		//do
		//{
		//	PreviousTask.RemoveAll(item => item.TaskFinished);
		//	PreviousDialog.RemoveAll(item => item.Finished);
		//	yield return new WaitForSeconds(1);
		//}
		//while (PreviousTask != null && PreviousTask.Count > 0 && PreviousDialog != null && PreviousDialog.Count != 0);
		bool playerInTriggleLastFrame = false;
		TextBackGround.gameObject.SetActive(false);
		do
		{
			if (PlayerManager.Instance.PlayerInTheArea(playerEnterTriggle) == true)
			{
				TextBackGround.transform.localScale = Vector3.right;
				TextBackGround.gameObject.SetActive(true);
				TextBackGround.transform.DOScale(Vector3.one, 0.1f);
				yield return new WaitForSeconds(0.1f);
				if (PreviousTask.Find(a => !a.TaskFinished) == null)
				{
					if (PreviousDialog.Find(a => !a.Finished) == null) break;
					yield return Cor_PrintStepByStep("钳制对话未完成", 0.2f);
				}
				else 
				{
					yield return Cor_PrintStepByStep("钳制任务未完成", 0.2f);
				}
				playerInTriggleLastFrame = true;
			}
			else if(playerInTriggleLastFrame)
			{
				playerInTriggleLastFrame = false;
				//TextBackGround.gameObject.SetActive(false);
				TextBackGround.transform.localScale = Vector3.one;
				TextBackGround.transform.DOScale(Vector3.right, 0.3f).onComplete = new TweenCallback(() =>TextBackGround.gameObject.SetActive(false));
			}
			else
			{
				yield return new WaitForSeconds(0.5f);
			}
		}
		while (true);
		yield return new WaitUntil(delegate { return PlayerManager.Instance.PlayerInTheArea(playerEnterTriggle) == true; });
		if (TextBackGround.gameObject.activeSelf == false)
		{
			TextBackGround.gameObject.SetActive(true);
			TextBackGround.transform.localScale = Vector3.right;
			TextBackGround.transform.DOScale(Vector3.one, 0.3f);
		}
		yield return new WaitForSeconds(0.29f);
		yield return Cor_PrintStepByStep("连接中");
		yield return new WaitForSeconds(1);
		while (dialogIndex < dialogDates.Count)
		{
			yield return new WaitUntil(delegate { return PlayerManager.Instance.PlayerInTheArea(playerEnterTriggle) == true; });
			//yield return new WaitUntil(delegate { return Keyboard.current.fKey.wasPressedThisFrame; });
			string content = dialogDates[dialogIndex].DialogContent;
			yield return Cor_PrintStepByStep(content);
			yield return new WaitForSeconds(0.6f);
			dialogIndex++;
		}
		yield return Cor_PrintStepByStep("NULL");
		yield return new WaitForSeconds(0.8f);
		TextBackGround.transform.DOScale(Vector3.right, 0.2f);
	}

	private IEnumerator Cor_PrintStepByStep(string str, float spendTime = -1)
	{
		spendTime = (spendTime == -1 ? 1.2f : spendTime) / str.Length ;
		for (int i = 0; i <= str.Length; i++)
		{
			textDialog.text = str.Substring(0, i);
			yield return new WaitForSeconds(spendTime);
		}
	}
}
