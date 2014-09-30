using UnityEngine;
using System.Collections;

public class ScreenOptions : BaseScreen {

	override protected void StartInner (){
	}

	override protected void OnGUIInner(){

		GuiHelper.DrawAtTop ("Settings");

		float textY = 0.25f;
		float buttonY = 0.04f;
		GuiHelper.DrawText ("Sound is " + (Sounds.IsMuted () ? "off" : "on") + ".", GuiHelper.SmallFontLeft, 0.1, textY, 0.8, 0.1);
		GuiHelper.ButtonWithText(0.8, textY + buttonY, 0.2, 0.2, (Sounds.IsMuted()?"on":"off"), SpriteManager.GetRoundButton(), GuiHelper.SmallFont, delegate(){
			Sounds.Mute(!Sounds.IsMuted());
		});

		textY += 0.15f;
		GuiHelper.DrawText ("Visit fb fan page", GuiHelper.SmallFontLeft, 0.1, textY, 0.8, 0.1);
		GuiHelper.ButtonWithText(0.8, textY + buttonY, 0.15, 0.15, "", SpriteManager.GetFbIcon(), GuiHelper.SmallFont, delegate(){
			CarSmasherSocial.FB.Like();
		});

		textY += 0.15f;
		string isNow = CarSmasherSocial.Authenticated ? "on" : "off";
		string willBe = CarSmasherSocial.Authenticated ? "disconnect" : "connect";
		GuiHelper.DrawText( "Google games is " + isNow + ". Click to ", GuiHelper.MicroFontLeft, 0.1, textY, 0.8, 0.1);
		Texture googlePlay = CarSmasherSocial.Authenticated ? SpriteManager.GetGooglePlay () : SpriteManager.GetInactiveGooglePlay ();
		GuiHelper.ButtonWithText(0.8, textY + buttonY-0.015, 0.2, 0.2, willBe, googlePlay, GuiHelper.MicroFont, delegate(){
			CarSmasherSocial.InitializeOrLogOut(true, null, null, this);
		});

		textY += 0.15f;

		if (GUI.Button(new Rect(GuiHelper.PercentW(0.12), GuiHelper.PercentH(0.7), GuiHelper.PercentW(0.15), GuiHelper.PercentH(0.14)), SpriteManager.GetRemoveIcon (), GuiHelper.CustomButton)){
			//Game.Me.Player.Reset();
			//Game.Me.Player.Save();
			PlayerPrefs.DeleteAll();
			Game.Me.Player.Reset();
		}
	}
}
