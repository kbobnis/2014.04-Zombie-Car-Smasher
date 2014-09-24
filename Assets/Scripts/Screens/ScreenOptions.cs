using UnityEngine;
using System.Collections;

public class ScreenOptions : BaseScreen {

	override protected void OnGUIInner(){
		GUI.depth = -1;
		GuiHelper.DrawAtTop ("Settings");

		Texture soundButton = Sounds.IsMuted()?SpriteManager.GetSoundButtonMuted():SpriteManager.GetSoundButton();
		if (GUI.Button(new Rect(GuiHelper.PercentW(0.12), GuiHelper.PercentH(0.25), GuiHelper.PercentW(0.15), GuiHelper.PercentH(0.14)), soundButton, GuiHelper.CustomButton)){
			Sounds.Mute(!Sounds.IsMuted());
		}
		
		if (GUI.Button(new Rect(GuiHelper.PercentW(0.12), GuiHelper.PercentH(0.4), GuiHelper.PercentW(0.15), GuiHelper.PercentH(0.14)), SpriteManager.GetFbIcon(), GuiHelper.CustomButton)){
			CarSmasherSocial.FB.Like();
		}
		
		Texture googlePlay = CarSmasherSocial.Authenticated ? SpriteManager.GetGooglePlay () : SpriteManager.GetInactiveGooglePlay ();
		if (GUI.Button(new Rect(GuiHelper.PercentW(0.12), GuiHelper.PercentH(0.55), GuiHelper.PercentW(0.15), GuiHelper.PercentH(0.14)), googlePlay, GuiHelper.CustomButton)){
			CarSmasherSocial.InitializeOrLogOut(true, null, null, this);
		}
		
		if (GUI.Button(new Rect(GuiHelper.PercentW(0.12), GuiHelper.PercentH(0.7), GuiHelper.PercentW(0.15), GuiHelper.PercentH(0.14)), SpriteManager.GetRemoveIcon (), GuiHelper.CustomButton)){
			//Game.Me.Player.Reset();
			//Game.Me.Player.Save();
			PlayerPrefs.DeleteAll();
			Game.Me.Player.Reset();
		}
		GUI.depth = 0;
	}
}
