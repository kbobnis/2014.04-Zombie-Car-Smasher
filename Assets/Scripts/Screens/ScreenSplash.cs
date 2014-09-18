using UnityEngine;
using System.Collections;

public class ScreenSplash : MonoBehaviour {

	public bool ShowingSplash;
	public bool GooglePlayFinished;
	// Use this for initialization
	void Start () {
		FB.Init(delegate {});
		CarSmasherSocial.InitializeSocial(false, AfterGooglePlay, AfterGooglePlay);
		Sounds.LoadSounds ();
		ShowingSplash = true;
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnGUI(){
		if (ShowingSplash){
			Texture texture = Resources.Load("Images/intro", typeof(Texture)) as Texture;
			float scale = (float)Screen.height / (float)texture.height;
			int height =  Mathf.RoundToInt( scale * texture.height);
			int width = Mathf.RoundToInt( scale * texture.width);
			int x = (Screen.width - width) / 2;

			GUI.DrawTexture(new Rect(x, 0 , width, height), texture);

			if (GooglePlayFinished){

				GuiHelper.ButtonWithText(0.3, 0.85, 0.3, 0.3, "Classic", SpriteManager.GetRoundButton(), GuiHelper.MicroFont, delegate(){
					ShowingSplash = false;
					Minigame m = gameObject.AddComponent<Minigame>();
					Destroy(this);
					m.PrepareRace(Game.Me.ClassicCarConfig, ScreenAfterMinigameClassic.PrepareScreen, new Mission(new AchievQuery[]{}, new AchievQuery[]{}, new Reward(0, 0), "")) ;
				});


				GuiHelper.ButtonWithText(0.75, 0.85, 0.3, 0.3, "Adventure", SpriteManager.GetRoundButton(), GuiHelper.MicroFont, delegate(){
					ShowingSplash = false;
					gameObject.AddComponent<ScreenAdvModeStart>();
					Destroy(this);
				});

			} else {
				GuiHelper.DrawText("Authenticating\n google play. \nJust a second ", GuiHelper.SmallFont, 0, 0.7, 1, 0.4);
			}

			GoogleAnalyticsKProjekt.LogScreenOnce (Minigame.SCREEN_MAIN);

			GuiHelper.DrawText("K Bobnis: Design, Programming\nM Bartynski: Design, Concept", GuiHelper.MicroFont, 0.2, 0, 0.6, 0.17);

			Texture soundButton = Sounds.IsMuted()?SpriteManager.GetSoundButtonMuted():SpriteManager.GetSoundButton();
			if (GUI.Button(new Rect(GuiHelper.PercentW(0.75), GuiHelper.PercentH(0.5), GuiHelper.PercentW(0.15), GuiHelper.PercentH(0.14)), soundButton, GuiHelper.CustomButton)){
				Sounds.Mute(!Sounds.IsMuted());
			}

			Texture fbButton = SpriteManager.GetFbIcon();
			if (GUI.Button(new Rect(GuiHelper.PercentW(0.06), GuiHelper.PercentH(0.5), GuiHelper.PercentW(0.15), GuiHelper.PercentH(0.14)), fbButton, GuiHelper.CustomButton)){
				CarSmasherSocial.FB.Like();
			}
		}
	}

	private void AfterGooglePlay(){
		GooglePlayFinished = true;
	}

}
