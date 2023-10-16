public struct Signal
{
	public static readonly Signal Default = new Signal() { SignalValue = 0 };
	public float SignalValue { get; set; }

	public int GetSwitchSignal {
		get {
			if (SignalValue > 0) {
				return 1;
			}
			else if (SignalValue < 0) {
				return -1;
			}
			else {
				return 0;
			}
		}
	}

	public float GetDistanceSignal {
		get {
			return SignalValue;
		}
	}

	public float GetRotateSignal{
		get {
			return SignalValue;
		}
	}
}
