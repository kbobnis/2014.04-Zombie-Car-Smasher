using UnityEngine;
using System.Collections;

public enum ParameterType{
	FASTER_START, VIBRATION
}
public static class ParameterTypeMethods{

	public static bool Default(this ParameterType pt){
		switch (pt) {
			case ParameterType.FASTER_START: return true;
			case ParameterType.VIBRATION: return true;
		default:
			throw new UnityException("There is no default for parameter type: " + pt);
		}
	}

	public static void Switch(this ParameterType pt, bool on){
		PlayerPrefs.SetInt(pt.ToString(), on?1:0);
	}
}

public class Parameter : MonoBehaviour {

	public static bool IsOn(ParameterType t){
		return PlayerPrefs.GetInt (t.ToString(), t.Default () ? 1 : 0)==1;
	}

}
