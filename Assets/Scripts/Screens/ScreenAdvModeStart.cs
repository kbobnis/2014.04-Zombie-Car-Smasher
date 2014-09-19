using UnityEngine;
using System.Collections;

public class ScreenAdvModeStart : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI(){

		PlayerState state = Game.Me.Player;

		GuiHelper.DrawBackground (delegate() {
			gameObject.AddComponent<ScreenSplash>();
			Destroy(this);
		});

		GuiHelper.DrawAtTop ("Adventure mode");
		GuiHelper.DrawText ("Earn coins, upgrade car, complete all missions.", GuiHelper.SmallFont, 0.1, 0.1, 0.8, 0.4);

		GUI.DrawTexture (new Rect (GuiHelper.PercentW(0.3), GuiHelper.PercentH (0.5), GuiHelper.PercentW (0.3), GuiHelper.PercentH (0.25)), state.CarConfig.CarTexture);

		GuiHelper.DrawButton ("Shields: " + state.CarConfig.Shield.Count, GuiHelper.MicroFont, 0.55, 0.43, 0.3, 0.15, delegate() {
			ScreenUpgrade su = gameObject.AddComponent<ScreenUpgrade>();
			su.PrepareWith(state.CarConfig.Shield);
			Destroy(this);
		});

		GuiHelper.DrawButton ("Combustion: " + state.CarConfig.Combustion.Value, GuiHelper.MicroFont, 0.1, 0.43, 0.3, 0.15, delegate() {
			ScreenUpgrade su = gameObject.AddComponent<ScreenUpgrade>();
			su.PrepareWith(state.CarConfig.Combustion);
			Destroy(this);
		});

		GuiHelper.DrawButton ("Wheel: " + state.CarConfig.Wheel.Value, GuiHelper.MicroFont, 0.1, 0.55, 0.3, 0.15, delegate() {
			ScreenUpgrade su = gameObject.AddComponent<ScreenUpgrade>();
			su.PrepareWith(state.CarConfig.Wheel);
			Destroy(this);
		});

		GuiHelper.DrawButton ("Starting oil: " + state.CarConfig.StartingOil.Value, GuiHelper.MicroFont, 0.55, 0.55, 0.3, 0.15, delegate() {
			ScreenUpgrade su = gameObject.AddComponent<ScreenUpgrade>();
			su.PrepareWith(state.CarConfig.StartingOil);
			Destroy(this);
		});

		GuiHelper.DrawButton ("Fuel tank: " + state.CarConfig.FuelTank.Capacity, GuiHelper.MicroFont, 0.55, 0.65, 0.3, 0.15, delegate() {
			ScreenUpgrade su = gameObject.AddComponent<ScreenUpgrade>();
			su.PrepareWith(state.CarConfig.FuelTank);
			Destroy(this);
		});


		GuiHelper.ButtonWithText(0.45, 0.85, 0.3, 0.3, "Select mission", SpriteManager.GetRoundButton(), GuiHelper.MicroFont, delegate() {
			ScreenSelectMission ssm = gameObject.AddComponent<ScreenSelectMission>();
			Destroy(this);
		});
	}
}
