using UnityEngine;
using System.Collections;
using GooglePlayGames;

public class ScreenSplash : MonoBehaviour {

	// Use this for initialization
	void Start () {
		FB.Init(delegate {});

		if (!CarSmasherSocial.Authenticated){
			ScreenLoading sl = gameObject.AddComponent<ScreenLoading> ();
			sl.Text = "Logging into google games";
			CarSmasherSocial.InitializeSocial (false, sl.EndMe, sl.EndMe);
		}
		Game.Me.Player.Load ();
		Sounds.LoadSounds ();
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnGUI(){
		Texture texture = SpriteManager.GetIntro();
		float scale = (float)Screen.height / (float)texture.height;
		int height =  Mathf.RoundToInt( scale * texture.height);
		int width = Mathf.RoundToInt( scale * texture.width);
		int x = (Screen.width - width) / 2;
		GUI.DrawTexture(new Rect(x, 0 , width, height), texture);

		GuiHelper.ButtonWithText(0.3, 0.85, 0.3, 0.3, "Classic", SpriteManager.GetRoundButton(), GuiHelper.MicroFont, delegate(){
			Minigame m = gameObject.AddComponent<Minigame>();
			Destroy(this);
			m.PrepareRace(Game.Me.Player, ScreenAfterMinigameClassic.PrepareScreen, Mission.Classic, Game.Me.ClassicCarConfig);
		});

		GuiHelper.ButtonWithText(0.75, 0.85, 0.3, 0.3, "Adventure", SpriteManager.GetRoundButton(), GuiHelper.MicroFont, delegate(){
			gameObject.AddComponent<ScreenAdvModeStart>();
			Destroy(this);
		});

		GoogleAnalyticsKProjekt.LogScreenOnce (ANALYTICS_SCREENS.SPLASH);

		GuiHelper.DrawText("K Bobnis: Design, Programming\nM Bartynski: Design, Concept", GuiHelper.MicroFont, 0.2, 0, 0.6, 0.17);

		Texture soundButton = Sounds.IsMuted()?SpriteManager.GetSoundButtonMuted():SpriteManager.GetSoundButton();
		if (GUI.Button(new Rect(GuiHelper.PercentW(0.75), GuiHelper.PercentH(0.5), GuiHelper.PercentW(0.15), GuiHelper.PercentH(0.14)), soundButton, GuiHelper.CustomButton)){
			Sounds.Mute(!Sounds.IsMuted());
		}

		if (GUI.Button(new Rect(GuiHelper.PercentW(0.06), GuiHelper.PercentH(0.5), GuiHelper.PercentW(0.15), GuiHelper.PercentH(0.14)), SpriteManager.GetFbIcon(), GuiHelper.CustomButton)){
				CarSmasherSocial.FB.Like();
			}

		Texture googlePlay = CarSmasherSocial.Authenticated ? SpriteManager.GetGooglePlay () : SpriteManager.GetInactiveGooglePlay ();
		if (GUI.Button(new Rect(GuiHelper.PercentW(0.06), GuiHelper.PercentH(0.3), GuiHelper.PercentW(0.15), GuiHelper.PercentH(0.14)), googlePlay, GuiHelper.CustomButton)){
			ScreenLoading sl = gameObject.AddComponent<ScreenLoading> ();
			sl.Text = CarSmasherSocial.Authenticated?"Logging out of google games":"Logging into google games";
			CarSmasherSocial.InitializeOrLogOut(true, sl.EndMe, sl.EndMe);
		}

		if (GUI.Button(new Rect(GuiHelper.PercentW(0.75), GuiHelper.PercentH(0.3), GuiHelper.PercentW(0.15), GuiHelper.PercentH(0.14)), SpriteManager.GetRemoveIcon (), GuiHelper.CustomButton)){
			Game.Me.Player.Reset();
			Game.Me.Player.Save();
		}
	}

}
