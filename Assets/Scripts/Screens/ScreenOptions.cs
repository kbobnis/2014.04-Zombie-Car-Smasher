using UnityEngine;
using System.Collections;

public class ScreenOptions : BaseScreen {

	override protected void StartInner (){
		Prepare(delegate(){
			gameObject.AddComponent<ScreenSplash>();
			Destroy(this);
		});
	}

	override protected void OnGUIInner(){

		GuiHelper.DrawAtTop ("Settings");

		float textY = 0.3f;
		float buttonY = 0.04f;
		float diff = 0.1f;
		GuiHelper.DrawText ("Sounds", GuiHelper.SmallFontLeft, 0.1, textY, 0.8, 0.1);
		GuiHelper.ButtonWithText(0.8, textY + buttonY, 0.2, 0.15, (Sounds.IsMuted()?" Turn on":"Turn off"), SpriteManager.GetRoundButton(), GuiHelper.MicroFont, delegate(){
			Sounds.Mute(!Sounds.IsMuted());
		});

		textY += diff;
		GuiHelper.DrawText ("Fb fan page", GuiHelper.SmallFontLeft, 0.1, textY, 0.8, 0.1);
		GuiHelper.ButtonWithText(0.8, textY + buttonY, 0.13, 0.13, "", SpriteManager.GetFbIcon(), GuiHelper.SmallFont, delegate(){
			CarSmasherSocial.FB.Like();
		});

		textY += diff;
		string isNow = CarSmasherSocial.Authenticated ? "on" : "off";
		string willBe = CarSmasherSocial.Authenticated ? "disconnect" : "connect";
		GuiHelper.DrawText( "Google games are " + isNow, GuiHelper.MicroFontLeft, 0.1, textY, 0.8, 0.1);
		Texture googlePlay = CarSmasherSocial.Authenticated ? SpriteManager.GetGooglePlay () : SpriteManager.GetInactiveGooglePlay ();
		GuiHelper.ButtonWithText(0.8, textY + buttonY-0.015, 0.25, 0.2, willBe, googlePlay, GuiHelper.MicroFont, delegate(){
			CarSmasherSocial.InitializeOrLogOut(true, null, null, this);
		});

		textY += diff;
		bool vibrationsOn = Parameter.IsOn(ParameterType.VIBRATION);
		GuiHelper.DrawText ("Vibrations", GuiHelper.SmallFontLeft, 0.1, textY, 0.8, 0.1);
		GuiHelper.ButtonWithText (0.8, textY + buttonY, 0.2, 0.15, "Turn " + (vibrationsOn ? "off" : "on"), SpriteManager.GetRoundButton (), GuiHelper.MicroFont, delegate() {
			ParameterType.VIBRATION.Switch(!vibrationsOn);
		});

		textY += diff;
		bool fasterStart = Parameter.IsOn (ParameterType.FASTER_START);
		GuiHelper.DrawText ("Faster start", GuiHelper.SmallFontLeft, 0.1, textY, 0.8, 0.1);
		GuiHelper.ButtonWithText (0.8, textY + buttonY, 0.2, 0.15, "Turn " + (fasterStart ? "off" : "on"), SpriteManager.GetRoundButton (), GuiHelper.MicroFont, delegate() {
			ParameterType.FASTER_START.Switch(!fasterStart);
		});
		GuiHelper.ButtonWithText (0.66, textY + buttonY, 0.2, 0.1, "i", SpriteManager.GetRoundButton (), GuiHelper.MicroFont, delegate() {
			ScreenText st = gameObject.AddComponent<ScreenText>();
			Destroy(this);
			st.Prepare(delegate(){
				st.gameObject.AddComponent<ScreenOptions>();
				Destroy(st);
			}, "Faster start", "If your high score is good, your car will be faster in first 300 distance to save you time.");
		});
	}
}
