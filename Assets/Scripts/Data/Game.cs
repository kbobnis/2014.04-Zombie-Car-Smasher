

public class Game{

	public static Game Me;

	public PlayerState PlayerState;
	public CarConfig ClassicCarConfig;

	static Game(){
		Me = new Game ();

		Me.PlayerState = new PlayerState ();
		Me.PlayerState.CarConfig = new CarConfig (CarConfig.MODE_ADV);
		Me.PlayerState.Coins = 20;

		Me.ClassicCarConfig = new CarConfig (CarConfig.MODE_CLASSIC);

	}
}