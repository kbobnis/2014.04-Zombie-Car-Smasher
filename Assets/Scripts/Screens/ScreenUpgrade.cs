using UnityEngine;
using System.Collections;

public class ScreenUpgrade : BaseScreen {

	private CarStatistic CarStatistic;

	override protected void StartInner (){
		Prepare(delegate() {
			gameObject.AddComponent<ScreenAdvModeStart>();
			Destroy(this);
		});
	}

	override protected void OnGUIInner(){

		PlayerState ps = Game.Me.Player;
		bool canAffordUpgrade = CarStatistic.CanUpgrade (ps.Coins);

		GuiHelper.DrawAtTop ("Upgrade " +  CarStatistic.Type.Name());

		string text = CarStatistic.Type.Description()+"\n\n";

		bool dependenciesOk = true;
		string dependencyText = "";
		foreach (Dependency dependency in CarStatistic.Dependencies) {
			if (!dependency.IsMet()){
				dependenciesOk = false;
				dependencyText = dependency.FailText;
			}
		}
		bool minimumOk = CarStatistic.Type.AboveMinimum (CarStatistic.Type.ValueForLevel (CarStatistic.Level + 1));

		if (dependenciesOk && minimumOk){
			text += "Coins: " + ps.Coins + " - " + CarStatistic.UpgradeCost() + " = " + (ps.Coins - CarStatistic.UpgradeCost()) +"\n";
			float valueBefore = CarStatistic.Value;
			float valueAfter = CarStatistic.Type.ValueForLevel(CarStatistic.Level+1);
			float valueDiff = valueAfter - valueBefore;
			text += CarStatistic.Type.Name () + ": " + CarStatistic.ValueFormatted+" + " + string.Format("{0:0.00}", valueDiff) + " = "  +string.Format("{0:0.00}", valueAfter)+ "\n\n";
			if (canAffordUpgrade){
				text += "Upgrade?";
			} else {
				text += "You have not enough coins";
			}
		} else if (!minimumOk){
			text += "This statistic has best possible value";
		} else if (!dependenciesOk){
			text += dependencyText;
		} 

		GuiHelper.DrawBeneathLine (text);
		if (canAffordUpgrade) {
			GuiHelper.YesButton(delegate() {
				ps.BuyAndUpgrade(CarStatistic);
				ps.Save();
			});
		}
	}

	public void PrepareWith(CarStatistic carStatistic){
		CarStatistic = carStatistic;
	}

}
