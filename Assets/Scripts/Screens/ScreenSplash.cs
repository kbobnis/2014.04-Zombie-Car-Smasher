using UnityEngine;
using System.Collections;
using GooglePlayGames;


public class ScreenSplash : BaseScreen {
	// Use this for initialization
	override protected void StartInner (){
		GuiDepth = Game.Me.ClosestGui;
		FB.Init(delegate {});

		if (!CarSmasherSocial.Authenticated && CarSmasherSocial.GetPreviousAnswer() == CarSmasherSocial.AuthenticationAnswer.NeverAsked || CarSmasherSocial.GetPreviousAnswer() == CarSmasherSocial.AuthenticationAnswer.Accepted){
			CarSmasherSocial.InitializeSocial (false, null, null, this);
		}
		Game.Me.Player.Load ();

		Prepare(delegate() {
			ScreenQuit sq = gameObject.AddComponent<ScreenQuit>();
			sq.Prepare(delegate(){
				sq.gameObject.AddComponent<ScreenSplash>();
				Destroy(sq);
			});
			Destroy(this);
		}, false);


	}

	override protected void OnGUIInner(){
		Texture texture = SpriteManager.GetIntro();
		float scale = (float)Screen.height / (float)texture.height;
		int height =  Mathf.RoundToInt( scale * texture.height);
		int width = Mathf.RoundToInt( scale * texture.width);
		int x = (Screen.width - width) / 2;
		GUI.DrawTexture(new Rect(x, 0 , width, height), texture);

		GuiHelper.ButtonWithText(0.3, 0.8, 0.3, 0.3, "Classic", SpriteManager.GetRoundButton(), GuiHelper.MicroFont, delegate(){
			Minigame m = gameObject.AddComponent<Minigame>();
			Destroy(this);
			m.PrepareRace(Game.Me.Player, ScreenAfterMinigameClassic.PrepareScreen, Mission.Classic, Game.Me.ClassicCarConfig);
		});

		GuiHelper.ButtonWithText(0.29, 0.95, 0.15, 0.15, "?", SpriteManager.GetRoundButton(), GuiHelper.MicroFont, delegate(){
			ScreenText m = gameObject.AddComponent<ScreenText>();
			m.Prepare(delegate() {
				m.gameObject.AddComponent<ScreenSplash>();
				Destroy(m);
			}, "Classic mode", "In classic mode you drive as far as you get. " +
				"Pick up oil stains to have fuel, avoid other obstacles."+
				"\n\nThere are google leaderboards and achievements with global scores, connect to it if you want to compare with others.");
			Destroy(this);
		});

		GuiHelper.ButtonWithText(0.75, 0.8, 0.3, 0.3, "Adventure", SpriteManager.GetRoundButton(), GuiHelper.MicroFont, delegate(){

			gameObject.AddComponent<ScreenAdvModeStart>();
			Destroy(this);
		});
		GuiHelper.ButtonWithText(0.74, 0.95, 0.15, 0.15, "?", SpriteManager.GetRoundButton(), GuiHelper.MicroFont, delegate(){
			ScreenText m = gameObject.AddComponent<ScreenText>();
			m.Prepare(delegate() {
				m.gameObject.AddComponent<ScreenSplash>();
				Destroy(m);
			}, "Adventure mode", "In adventure mode you have your own car. Make missions, collect coins to upgrade car and drive further. " +
			"Pick up oil stains to have fuel, avoid other obstacles."+
			"\n\nThere are google leaderboards with global scores, connect to it if you want to compare with others.");
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
