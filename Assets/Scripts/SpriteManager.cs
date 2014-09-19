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
	private static Texture Leaderboard;
	private static Texture Achievements;
	private static Texture BackButton;
	private static Texture FbShareButton;
	private static Texture FbIcon;
	private static Texture CarAdventure;
	private static Texture CarDefault;
	private static Texture Coin;
	private static Texture Shield;

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
		Leaderboard = Resources.Load<Texture> ("Images/leaderboard");
		Achievements = Resources.Load<Texture> ("Images/achievement");
		BackButton = Resources.Load<Texture> ("Images/back_arrow");
		FbShareButton = Resources.Load<Texture> ("Images/fb_share");
		FbIcon = Resources.Load<Texture> ("Images/fb");
		CarAdventure = Resources.Load<Texture2D> ("Images/car_adventure");
		CarDefault = Resources.Load<Texture> ("Images/icon");
		Coin = Resources.Load<Texture> ("Images/coin");
		Shield = Resources.Load<Texture> ("Images/shield");
	}

	static public Texture GetShield(){
		return Shield;
	}

	static public Texture GetCoin(){
		return Coin;
	}

	static public Texture GetFbShareButton(){
		return FbShareButton;
	}

	static public Texture GetAchievements(){
		return Achievements;
	}

	static public Texture GetLeaderboard(){
		return Leaderboard;
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

	static public Texture GetCar() {
		return CarDefault;
	}

	static public Sprite GetCactus(){
		return Atlas[2];
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

	static public Texture GetBackButton(){
		return BackButton;
	}

	static public Texture GetFbIcon(){
		return FbIcon;
	}

	static public Texture GetCarAdventure(){
		return CarAdventure;
	}
}
