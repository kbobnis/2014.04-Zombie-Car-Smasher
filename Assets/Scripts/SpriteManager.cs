using UnityEngine;
using System.Collections.Generic;


class SpriteManager {

	private static Sprite[] Atlas;
	private static Texture LeftArrow;
	private static Texture RightArrow;
	private static Texture RoundButton;
	private static Texture OilTexture;
	private static Texture DistanceBorder;

	static SpriteManager(){
		Atlas = Resources.LoadAll<Sprite>("Images/atlas");
		LeftArrow = Resources.Load<Texture> ("Images/leftArrow");
		RightArrow = Resources.Load<Texture> ("Images/rightArrow");
		RoundButton = Resources.Load<Texture> ("Images/roundButton");
		OilTexture = Resources.Load<Texture> ("Images/oil");
		DistanceBorder = Resources.Load<Texture> ("Images/distanceBorder");
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
}
