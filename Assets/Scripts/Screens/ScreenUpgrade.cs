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
		bool canUpgrade = CarStatistic.CanUpgrade (ps.Coins);

		GuiHelper.DrawAtTop (CarStatistic.TopText ());

		string text = CarStatistic.Type.Name () + ": " + CarStatistic.ValueFormatted+"\n\n";
		text += CarStatistic.Info (canUpgrade);
		GuiHelper.DrawBeneathLine (text);
		if (canUpgrade) {
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
