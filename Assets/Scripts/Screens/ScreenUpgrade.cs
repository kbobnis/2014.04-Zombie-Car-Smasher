using UnityEngine;
using System.Collections;

public class ScreenUpgrade : MonoBehaviour {

	private CarStatistic CarStatistic;

	// Use this for initialization
	void Start () {


	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI(){
		GuiHelper.DrawBackground (delegate() {
			gameObject.AddComponent<ScreenAdvModeStart>();
			Destroy(this);
		});

		PlayerState ps = Game.Me.Player;
		bool canAffordUpgrade =  ps.Coins >= CarStatistic.UpgradeCost ();
		string topText = CarStatistic.TopText ();
		string bottomText = CarStatistic.Info (canAffordUpgrade);

		GuiHelper.DrawAtTop (topText);
		GuiHelper.DrawBeneathLine (bottomText);
		if (canAffordUpgrade) {
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
