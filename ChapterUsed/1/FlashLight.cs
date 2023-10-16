using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


public class FlashLight : SerializedMonoBehaviour
{
	// ----------------//
	// --- 序列化
	// ----------------//
	public bool UseFlash;
	// ----------------//
	// --- 公有成员
	// ----------------//
	[Tooltip("距离上个时间电的闪烁间隔，闪烁时长，闪烁强度")]
	[ShowIf("@this.UseFlash")][ListDrawerSettings(NumberOfItemsPerPage = 20)]
	public List<LightFlashCtrlData> _ctrlData;

	// ----------------//
	// --- 私有成员
	// ----------------//
	[ReadOnly][SerializeField]
	private int dataIndex;
	private UnityEngine.Rendering.Universal.Light2D light2D;
	private Coroutine _corLight;

	// ----------------//
	// --- Unity消息
	// ----------------//
	private void Awake()
	{
		light2D = GetComponent<UnityEngine.Rendering.Universal.Light2D>();
	}

	private void OnEnable()
	{
		_corLight = StartCoroutine(FlashCor());
	}

	private void OnDisable()
	{
		StopCoroutine(_corLight);
	}
	// ----------------//
	// --- 公有方法
	// ----------------//
#if UNITY_EDITOR
	[ExecuteInEditMode]
	public void EditorStart()
	{
		StartCoroutine(FlashCor());
	}
#endif

	// ----------------//
	// --- 私有方法
	// ----------------//
	private IEnumerator FlashCor()
	{
		float lightOnIntensity = light2D.intensity;
		while (true)
		{
			if (_ctrlData != null && _ctrlData.Count > 0)
			{
				for (int i = 0; i < _ctrlData.Count; i++)
				{
					dataIndex = i;
					float startTime = Time.fixedTime;
					light2D.intensity = _ctrlData[i].Intensity * lightOnIntensity;
					while (Time.fixedTime - startTime < _ctrlData[i].KeepTime) // 关闭持续
					{
						yield return 0;
					}
				}
			}
#if UNITY_EDITOR
			if (UnityEditor.EditorApplication.isCompiling)
			{
				break;
			}
#endif
			yield return 0;
		}
	}

	// ----------------//
	// --- 类型
	// ----------------//
	public struct LightFlashCtrlData
	{
		[Min(0.001f)]
		public float KeepTime;
		[Range(0,1)]
		public float Intensity;
	}
}
