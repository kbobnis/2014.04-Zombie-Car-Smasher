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
	private static Texture Intro;
	private static Texture GooglePlay;
	private static Texture InactiveGooglePlay;
	private static Texture LoadingScreen;
	private static Texture RemoveIcon;
	private static Texture SettingsIcon;
	private static Texture RectangleButton;
	private static Texture UpArrow;

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
		Intro = Resources.Load<Texture> ("Images/intro");
		GooglePlay = Resources.Load<Texture> ("Images/googlePlay");
		InactiveGooglePlay = Resources.Load<Texture> ("Images/googlePlayInactive");
		LoadingScreen = Resources.Load<Texture> ("Images/LoadingScreen");
		RemoveIcon = Resources.Load<Texture> ("Images/remove");
		SettingsIcon = Resources.Load<Texture> ("Images/settings");
		RectangleButton = Resources.Load<Texture> ("Images/rectangleButton");
		UpArrow = Resources.Load<Texture> ("Images/up_arrow");
	}

	static public Texture GetUpArrow(){
		return UpArrow;
	}

	static public Texture GetRectangleButton(){
		return RectangleButton;
	}

	static public Texture GetSettingsIcon(){
		return SettingsIcon;
	}

	static public Texture GetRemoveIcon(){
		return RemoveIcon;
	}

	static public Texture GetInactiveGooglePlay(){
		return InactiveGooglePlay;
	}

	static public Texture GetLoadingScreen(){
		return LoadingScreen;
	}

	static public Texture GetGooglePlay(){
		return GooglePlay;
	}

	static public Texture GetIntro(){
		return Intro;
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
