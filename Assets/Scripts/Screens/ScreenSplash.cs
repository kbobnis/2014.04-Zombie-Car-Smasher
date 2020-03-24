using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class ScreenSplash : BaseScreen {
	// Use this for initialization
	override protected void StartInner (){
		GuiDepth = Game.Me.ClosestGui;

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
		GUI.DrawTexture(new Rect(x, GuiHelper.PercentH(0.1), width, height), texture);

		GuiHelper.ButtonWithText(0.3, 0.9, 0.3, 0.3, "Classic", SpriteManager.GetRoundButton(), GuiHelper.MicroFont, delegate(){
			Minigame m = gameObject.AddComponent<Minigame>();
			Destroy(this);
			m.PrepareRace(Game.Me.Player, ScreenAfterMinigameClassic.PrepareScreen, Mission.Classic, Game.Me.ClassicCarConfig);
		});

		GuiHelper.ButtonWithText(0.14, 0.78, 0.15, 0.15, "?", SpriteManager.GetRoundButton(), GuiHelper.MicroFont, delegate(){
			ScreenText m = gameObject.AddComponent<ScreenText>();
			m.Prepare(delegate() {
				m.gameObject.AddComponent<ScreenSplash>();
				Destroy(m);
			}, "Classic mode", "In classic mode you drive as far as you get. " +
				"Pick up oil stains to have fuel, avoid other obstacles."+
				"\n\nThere are google leaderboards and achievements with global scores, connect to it if you want to compare with others.");
			Destroy(this);
		});

		GuiHelper.ButtonWithText(0.7, 0.9, 0.3, 0.3, "Adventure", SpriteManager.GetRoundButton(), GuiHelper.MicroFont, delegate(){
			gameObject.AddComponent<ScreenAdvModeStart>();
			Destroy(this);
		});
		GuiHelper.ButtonWithText(0.85, 0.78, 0.15, 0.15, "?", SpriteManager.GetRoundButton(), GuiHelper.MicroFont, delegate(){
			ScreenText m = gameObject.AddComponent<ScreenText>();
			m.Prepare(delegate() {
				m.gameObject.AddComponent<ScreenSplash>();
				Destroy(m);
			}, "Adventure mode", 
			"In adventure mode you have your own car. Upgrade it with coins. " +
			"Make missions, collect coins in race. Drive further. " +
			"The farther you get the more coins there will be. " +
			"Pick up oil stains to have fuel, avoid other obstacles. "+
			"At some point shields will be available. " +
			"\n\nThere are google leaderboards with global scores, connect to it if you want to compare with others.");
			Destroy(this);
		});

		GuiHelper.DrawText("K Bobnis: Design, Programming\nM Bartynski: Design, Concept", GuiHelper.MicroFont, 0.1, 0.1, 0.8, 0.17);

		if (GUI.Button(new Rect(GuiHelper.PercentW(0.79), GuiHelper.PercentH(0.21), GuiHelper.PercentW(0.2), GuiHelper.PercentH(0.2)), SpriteManager.GetSettingsIcon(), GuiHelper.CustomButton)){
			gameObject.AddComponent<ScreenOptions>();

			Destroy(this);
		}
	}



}
