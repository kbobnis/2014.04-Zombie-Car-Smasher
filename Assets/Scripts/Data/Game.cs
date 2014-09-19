

public class Game{

	public static Game Me;

	public PlayerState Player;
	public CarConfig ClassicCarConfig;
	public Environment ClassicEnvironment;

	static Game(){
		Me = new Game ();

		Me.Player = new PlayerState ();
		Me.Player.CarConfig = new CarConfig (CarConfig.MODE_ADV);
		Me.Player.Coins = 20;

		Me.ClassicCarConfig = new CarConfig (CarConfig.MODE_CLASSIC);

		Me.ClassicEnvironment = new Environment ();
	}
}