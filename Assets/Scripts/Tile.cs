using UnityEngine;

public enum TileContent{
	WALL, 
	HOLE,
	NONE,
	BUFF_OIL,
	CACTUS
}

public class Tile : MonoBehaviour 
{
	public TileContent _TileContent;

	public static float WallChance;
	public static float HoleChance;
	public static float BuffFuelChance;
	public static float BuffOilValue;


	public TileContent TileContent {
		get { return _TileContent; }
		set { 
			_TileContent = value;
			gameObject.name = value.ToString();
		}
	}

	void Start () {
	}

	// Update is called once per frame
	void Update () {

	}

	public void InitWithOil(GameObject parent, int i, int inGameY){

		SpriteRenderer r = null;
		TileContent = TileContent.BUFF_OIL;
		r = gameObject.AddComponent<SpriteRenderer>();
		r.sprite = SpriteManager.GetOil();
		transform.localScale = new Vector3(1, 1, 1);
		transform.parent = parent.transform;
		if (r != null){
			r.sortingLayerName = "Layer2";
			//float scale = InGamePosition.tileW/(float) r.sprite.bounds.size.x ;
			transform.localScale = new Vector3(1, 1, 0); //scale, scale, 0);
			r.receiveShadows = true;
			r.castShadows = true;
		}
		InGamePosition igp = gameObject.AddComponent<InGamePosition>();
		igp.x = i;
		igp.y = inGameY;
	}

	public void InitCactus(GameObject parent, float i, int inGameY){
		SpriteRenderer r = null;
		TileContent = TileContent.CACTUS;
		r = gameObject.AddComponent<SpriteRenderer>();
		r.sprite = SpriteManager.GetCactus();
		transform.localScale = new Vector3(1, 1, 1);
		transform.parent = parent.transform;
		if (r != null){
			r.sortingLayerName = "Layer4";
			//float scale = InGamePosition.tileW/(float) r.sprite.bounds.size.x ;
			transform.localScale = new Vector3(1, 1, 0); //scale, scale, 0);
			r.receiveShadows = true;
			r.castShadows = true;
		}
		InGamePosition igp = gameObject.AddComponent<InGamePosition>();
		igp.x = i;
		igp.y = inGameY;

	}

	public void InitMe(GameObject parent, int i, int inGameY, bool canBeWall, bool canBeHole, bool canBeOil){
		TileContent = TileContent.NONE;
		float ticket = Random.Range(0f, 1f);
		SpriteRenderer r = null;
		if (canBeWall && ticket < WallChance ){
			TileContent = TileContent.WALL;
			r = gameObject.AddComponent<SpriteRenderer>();
			r.sprite = SpriteManager.GetWall();
		} else if (canBeHole && ticket > WallChance && ticket < WallChance + HoleChance){
			TileContent = TileContent.HOLE;
			r = gameObject.AddComponent<SpriteRenderer>();
			r.sprite = SpriteManager.GetHole();
		} else if (canBeOil && ticket > WallChance + HoleChance && ticket < WallChance + HoleChance + BuffFuelChance){
			TileContent = TileContent.BUFF_OIL;
			r = gameObject.AddComponent<SpriteRenderer>();
			r.sprite = SpriteManager.GetOil();
		}

		transform.localScale = new Vector3(1, 1, 1);
		transform.parent = parent.transform;
		if (r != null){
			r.sortingLayerName = "Layer2";
			//float scale = InGamePosition.tileW/(float) r.sprite.bounds.size.x ;
			transform.localScale = new Vector3(1, 1, 0); //scale, scale, 0);
			r.receiveShadows = true;
			r.castShadows = true;
		}
		InGamePosition igp = gameObject.AddComponent<InGamePosition>();
		igp.x = i;
		igp.y = inGameY;
	}

	public void GMIsOn(GameObject g){
		//if crashed
		if (TileContent == TileContent.HOLE && g.GetComponent<Flyier>() == null){
			Minigame.Me.GameOver(Minigame.FELL_INTO_HOLE);
		}

		if (TileContent == TileContent.WALL ){
			if (g.GetComponent<Destroyer>() != null){
				gameObject.GetComponent<SpriteRenderer>().enabled = false;
				TileContent  = TileContent.NONE;
				Destroy(g);
			} else { //if (g.GetComponent<Flyier>() == null){ //even flying crash into walls
				Minigame.Me.GameOver(Minigame.CRASHED_INTO_WALL);
			}
		}

		if (TileContent == TileContent.BUFF_OIL){

			if (g.GetComponent<Destroyer>() != null){
				gameObject.GetComponent<SpriteRenderer>().enabled = false;
				TileContent  = TileContent.NONE;
				Destroy(g);
			} else  if (g.GetComponent<Flyier>() == null){ //flying object don't pick up oil buffs
				Minigame.Me.Car.GetComponent<Car>().PickedUpFuel(BuffOilValue);
				GetComponent<SpriteRenderer>().enabled = false;
				TileContent = TileContent.NONE;
			}
		}
	}
}