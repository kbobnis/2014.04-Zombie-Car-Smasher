using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScreenStartingMission : BaseScreen {

	public Mission Mission;
	private AfterButton BackButton;

	override protected void StartInner (){

		BackButton = delegate() {
			gameObject.AddComponent<ScreenAdvModeStart> ();
			Destroy (this);
		};

		Prepare(BackButton);
		Mission = Mission.SearchMissionForPlayer (Game.Me.Player);
	}

	override protected void OnGUIInner(){

		PlayerState state = Game.Me.Player;
		int upgAvailable = 0;
		foreach(KeyValuePair<CarStatisticType, CarStatistic> kvp in state.CarConfig.CarStatistics){
			if (kvp.Value.IsUnlockedFor(state) && kvp.Value.CanUpgrade(state.Coins)){
				upgAvailable ++;
			}
		}

		GuiHelper.DrawAtTop (Mission.Description);
		string text = "Reward: " + Mission.Reward.Description + 
			"\n\n "+upgAvailable + " upgrades are available" + 
			"\n\n\n\n Your best distance: " + HighScores.GetTopScore(HighScoreType.Adventure);

		GuiHelper.DrawBeneathLine (text);

		if (upgAvailable > 0){
			GuiHelper.ButtonWithText (0.85, 0.4, 0.13, 0.13, "", SpriteManager.GetUpArrow (), GuiHelper.SmallFont, BackButton);
		}

		Texture leaderBoard = SpriteManager.GetLeaderboard();
		GuiHelper.ButtonWithText(0.85, 0.585, 0.2, 0.15, "Scores", SpriteManager.GetRoundButton(), GuiHelper.MicroFont, delegate(){
			CarSmasherSocial.ShowLeaderBoard(GoogleLeaderboard.LEADERB_BEST_DISTANCES_ADV);
		});

		GuiHelper.YesButton(delegate(){
			Minigame mi = gameObject.AddComponent<Minigame>();
			Destroy(this);
			GoogleAnalyticsKProjekt.LogScreenOnce(AnalyticsScreen.GameAdv);
			mi.PrepareRace(Game.Me.Player, ScreenAfterMinigameAdv.PrepareScreen, Mission, Game.Me.Player.CarConfig);
		}, "Start");

	}
}
