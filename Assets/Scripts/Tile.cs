using UnityEngine;

public enum TileContent{
	WALL, 
	HOLE,
	NONE,
	BUFF_OIL
}



public class Tile : MonoBehaviour 
{
	public TileContent TileContent;

	public static float WallChance;
	public static float HoleChance;
	public static float BuffFuelChance;
	public static float BuffOilValue;

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {

	}

	public void InitMe(GameObject parent, int i, int inGameY){
		TileContent = TileContent.NONE;
		float ticket = Random.Range(0f, 1f);
		//Debug.Log ("ticket was: " + ticket );
		SpriteRenderer r = null;
		if (ticket < WallChance ){
			TileContent = TileContent.WALL;
			r = gameObject.AddComponent<SpriteRenderer>();
			r.sprite = Resources.Load<Sprite>("Images/wall");
			//Debug.Log("created wall");
		} else if (ticket < WallChance + HoleChance){
			TileContent = TileContent.HOLE;
			r = gameObject.AddComponent<SpriteRenderer>();
			r.sprite = Resources.Load<Sprite>("Images/hole");
			//Debug.Log("created hole");
		} else if (ticket < WallChance + HoleChance + BuffFuelChance){
			TileContent = TileContent.BUFF_OIL;
			r = gameObject.AddComponent<SpriteRenderer>();
			r.sprite = Resources.Load<Sprite>("Images/oil");
		}

		transform.localScale = new Vector3(1, 1, 1);
		transform.parent = parent.transform;
		if (r != null){
			r.sortingLayerName = "Layer2";
			float scale = InGamePosition.tileW/(float) r.sprite.bounds.size.x ;
			transform.localScale = new Vector3(scale, scale, 0);
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
			g.GetComponent<ActionReceiver>().FellIntoHole();
		}

		if (TileContent == TileContent.WALL ){
			if (g.GetComponent<Destroyer>() != null){
				gameObject.GetComponent<SpriteRenderer>().enabled = false;
				TileContent  = TileContent.NONE;
				Destroy(g);
			} else {
				g.GetComponent<ActionReceiver>().CrashedIntoWall();
			}
		}

		if (TileContent == TileContent.BUFF_OIL){

			if (g.GetComponent<Destroyer>() != null){
				gameObject.GetComponent<SpriteRenderer>().enabled = false;
				TileContent  = TileContent.NONE;
			} else {
				Minigame.Me.Car.GetComponent<Fuel>().Amount += BuffOilValue;
				GetComponent<SpriteRenderer>().enabled = false;
			}
		}
	}
}




