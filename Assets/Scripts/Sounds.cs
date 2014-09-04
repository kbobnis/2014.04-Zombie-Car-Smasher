using UnityEngine;
using System.Collections;

public class Sounds : MonoBehaviour {

	public static AudioClip CartonImpact;
	public static AudioClip PickUpFuel;
	public static AudioClip TurnCar;
	public static AudioClip NoMoreFuel;
	public static AudioClip CarDrive;
	public static AudioClip LowFuelSignal;

	private static bool isMuted;

	public static void LoadSounds(){
		CartonImpact = Resources.Load<AudioClip> ("Audio/car_crash3");
		PickUpFuel = Resources.Load<AudioClip> ("Audio/water_drop");
		TurnCar = Resources.Load<AudioClip> ("Audio/ms_page_turn");
		NoMoreFuel = Resources.Load<AudioClip> ("Audio/engine_stop3");
		CarDrive = Resources.Load<AudioClip> ("Audio/ms_auto_running");
		LowFuelSignal = Resources.Load<AudioClip> ("Audio/ms_fuel_ending");

		isMuted = PlayerPrefs.GetInt("audioMuted")>0;

		Mute (isMuted);
	}

	public static void Mute(bool mute){
		AudioListener.volume = mute ? 0 : 1;
		PlayerPrefs.SetInt("audioMuted", mute?1:0);
		isMuted = mute;
	}

	public static bool IsMuted(){
		return isMuted;
	}

}
