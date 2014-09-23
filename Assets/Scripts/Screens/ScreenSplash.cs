using UnityEngine;
using System.Collections;
using GooglePlayGames;


public class ScreenSplash : MonoBehaviour {

	// Use this for initialization
	void Start () {
		FB.Init(delegate {});

		if (!CarSmasherSocial.Authenticated && CarSmasherSocial.GetPreviousAnswer() == CarSmasherSocial.AuthenticationAnswer.NeverAsked || CarSmasherSocial.GetPreviousAnswer() == CarSmasherSocial.AuthenticationAnswer.Accepted){
			CarSmasherSocial.InitializeSocial (false, null, null, this);
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

		GuiHelper.DrawText("K Bobnis: Design, Programming\nM Bartynski: Design, Concept", GuiHelper.MicroFont, 0.1, 0, 0.8, 0.17);

		if (GUI.Button(new Rect(GuiHelper.PercentW(0.79), GuiHelper.PercentH(0.21), GuiHelper.PercentW(0.2), GuiHelper.PercentH(0.2)), SpriteManager.GetSettingsIcon(), GuiHelper.CustomButton)){
			ScreenOptions so = gameObject.AddComponent<ScreenOptions>();
			so.Prepare(delegate(){
				so.gameObject.AddComponent<ScreenSplash>();
				Destroy(so);
			});

			Destroy(this);
		}
	}



}
