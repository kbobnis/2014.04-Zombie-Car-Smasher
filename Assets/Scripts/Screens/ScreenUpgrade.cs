using UnityEngine;
using System.Collections;

public class ScreenUpgrade : MonoBehaviour {

	private CarStatistic CarStatistic;

	void OnGUI(){
		GuiHelper.DrawBackground (delegate() {
			gameObject.AddComponent<ScreenAdvModeStart>();
			Destroy(this);
		});

		PlayerState ps = Game.Me.Player;
		bool canUpgrade = CarStatistic.CanUpgrade (ps.Coins);

		GuiHelper.DrawAtTop (CarStatistic.TopText ());
		GuiHelper.DrawBeneathLine (CarStatistic.Info (canUpgrade));
		if (canUpgrade) {
			GuiHelper.ButtonWithText(0.4, 0.8, 0.3, 0.3, "Yes", SpriteManager.GetRoundButton(), GuiHelper.SmallFont, delegate() {
				ps.BuyAndUpgrade(CarStatistic);
				ps.Save();
			});
		}
	}

	public void PrepareWith(CarStatistic carStatistic){
		CarStatistic = carStatistic;
	}

}
