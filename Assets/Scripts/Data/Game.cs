

public class Game{

	public static Game Me;

	public PlayerState Player;
	public CarConfig ClassicCarConfig;
	public Environment ClassicEnvironment;

	static Game(){
		Me = new Game ();
		Me.ClassicCarConfig = new CarConfig (CarConfig.MODE_CLASSIC);
		Me.ClassicEnvironment = new Environment ();
		Me.Player = new PlayerState(null, 0);
		Me.Player.Reset ();
	}

}