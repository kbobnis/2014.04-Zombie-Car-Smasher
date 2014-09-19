using UnityEngine;

public enum TileContent{
	WALL, 
	HOLE,
	NONE,
	BUFF_OIL,
	CACTUS,
	COIN
}

public class Tile : MonoBehaviour 
{
	public TileContent _TileContent;

	public static float WallChance;
	public static float HoleChance;
	public static float BuffFuelChance;
	public const float CoinChance = 0.01f;


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
		} 
		ticket -= WallChance;
		if (canBeHole && ticket > 0 && ticket < HoleChance){
			TileContent = TileContent.HOLE;
			r = gameObject.AddComponent<SpriteRenderer>();
			r.sprite = SpriteManager.GetHole();
		} 
		ticket -= HoleChance;
		if (canBeOil && ticket > 0 && ticket < BuffFuelChance){
			TileContent = TileContent.BUFF_OIL;
			r = gameObject.AddComponent<SpriteRenderer>();
			r.sprite = SpriteManager.GetOil();
		} 
		ticket -= BuffFuelChance;
		if (ticket > 0 && ticket < CoinChance) {
			TileContent = TileContent.COIN;
			r = gameObject.AddComponent<SpriteRenderer>();
			Texture2D coin = (Texture2D) SpriteManager.GetCoin();
			r.sprite = Sprite.Create(coin, new Rect(0, 0, coin.width, coin.height), new Vector2(0.5f, 0.5f));
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


}