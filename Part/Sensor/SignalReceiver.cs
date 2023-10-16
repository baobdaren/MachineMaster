using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignalReceiver : MonoBehaviour
{
    private BaseSensor m_connectSensor = null;
    public Signal GetLastSignal {
        get;
        private set;
    }

    ///////////-
    // 实例方法
    ///////////-
    public void AddReceiveTarget(BaseSensor sensor) {
		//sensor.OnSignal += Sensor_OnSignal;
        m_connectSensor = sensor;
    }

	private void Sensor_OnSignal(BaseSensor sensor, Signal signal) {
        Debug.LogError(signal.SignalValue);
        GetLastSignal = signal;
	}

	private void OnDestroy() {
        //m_connectSensor.OnSignal -= Sensor_OnSignal;
	}
}
