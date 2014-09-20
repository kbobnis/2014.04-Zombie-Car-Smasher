

public class Game{

	public static Game Me;

	public PlayerState Player;
	public CarConfig ClassicCarConfig;
	public Environment ClassicEnvironment;

	static Game(){
		Me = new Game ();

		Me.Player = new PlayerState (new CarConfig (CarConfig.MODE_ADV), 20);
		Me.ClassicCarConfig = new CarConfig (CarConfig.MODE_CLASSIC);
		Me.ClassicEnvironment = new Environment ();
	}
}