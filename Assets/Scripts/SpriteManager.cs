using UnityEngine;
using System.Collections.Generic;


class SpriteManager {

	private static Sprite[] Atlas;
	private static Texture LeftArrow;
	private static Texture RightArrow;
	private static Texture RoundButton;
	private static Texture OilTexture;
	private static Texture DistanceBorder;
	private static Texture StartButton;
	private static Texture SoundButton;
	private static Texture SoundButtonMuted;

	static SpriteManager(){
		Atlas = Resources.LoadAll<Sprite>("Images/atlas");
		LeftArrow = Resources.Load<Texture> ("Images/leftArrow");
		RightArrow = Resources.Load<Texture> ("Images/rightArrow");
		RoundButton = Resources.Load<Texture> ("Images/roundButton");
		OilTexture = Resources.Load<Texture> ("Images/oil");
		DistanceBorder = Resources.Load<Texture> ("Images/distanceBorder");
		StartButton = Resources.Load<Texture> ("Images/start");
		SoundButton = Resources.Load<Texture> ("Images/note");
		SoundButtonMuted = Resources.Load<Texture> ("Images/note_muted");
	}

	static public Texture GetDistanceBorder(){
		return DistanceBorder;
	}

	static public Sprite GetWall() {
		return Atlas[6];
	}

	static public Sprite GetHole() {
		return Atlas [1];
	}

	static public Sprite GetOil() {
		return Atlas[9];
	}

	static public Sprite GetCar() {
		return Atlas [10];
	}

	static public Texture GetLeftArrow(){
		return LeftArrow;
	}

	static public Texture GetRightArrow(){
		return RightArrow;
	}

	static public Texture GetRoundButton(){
		return RoundButton;
	}

	static public Texture GetOilTexture(){
		return OilTexture;
	}

	static public Texture GetStartButton(){
		return StartButton;
	}

	static public Texture GetSoundButton(){
		return SoundButton;
	}
	static public Texture GetSoundButtonMuted(){
		return SoundButtonMuted;
	}
}
