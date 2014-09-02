using UnityEngine;
using System.Collections;

public class Sounds : MonoBehaviour {

	public static AudioClip CartonImpact;
	public static AudioClip PickUpFuel;
	public static AudioClip TurnCar;
	public static AudioClip NoMoreFuel;
	public static AudioClip CarDrive;

	private static bool isMuted;

	public static void LoadSounds(){
		CartonImpact = Resources.Load<AudioClip> ("Audio/carton_impact");
		PickUpFuel = Resources.Load<AudioClip> ("Audio/water_drop");
		TurnCar = Resources.Load<AudioClip> ("Audio/page_turn");
		NoMoreFuel = Resources.Load<AudioClip> ("Audio/paper_falling");
		CarDrive = Resources.Load<AudioClip> ("Audio/paper_slide");

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
