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
	public float Value;
	private GameObject Parent;

	public float Lane{
		set {

			InGamePosition igp = GetComponent<InGamePosition>();
			if (igp == null){
				igp = gameObject.AddComponent<InGamePosition>();
			}
			igp.x = value;
		}
	}

	public TileContent TileContent {
		get { return _TileContent; }
		set { 
			_TileContent = value;
			gameObject.name = value.ToString();

			if (gameObject.GetComponent<SpriteRenderer>()){
				Destroy(gameObject.GetComponent<SpriteRenderer>());
			}
			if (value == TileContent.NONE){
				return ;
			}

			SpriteRenderer r = gameObject.AddComponent<SpriteRenderer>();
			gameObject.transform.localScale = new Vector3(1, 1, 1);
			r.sortingLayerName = "Layer4";
			switch(value){
			case TileContent.CACTUS:
				r.sprite = SpriteManager.GetCactus();
				break;
			case TileContent.WALL:
				r.sprite = SpriteManager.GetWall();
				break;
			case TileContent.HOLE:
				r.sprite = SpriteManager.GetHole();
				break;
			case TileContent.COIN:
				Texture2D coin = (Texture2D) SpriteManager.GetCoin();
				r.sprite = Sprite.Create(coin, new Rect(0, 0, coin.width, coin.height), new Vector2(0.5f, 0.5f));
				break;
			case TileContent.BUFF_OIL:
				r.sprite = SpriteManager.GetOil();
				break;
			}
		}
	}

	public void InitMe(int distance, int i, bool canBeWall, bool canBeHole, bool canBeOil, Environment env){
		TileContent = TileContent.NONE;
		float ticket = Random.Range(0f, 1f);
		SpriteRenderer r = null;
		if (canBeWall && ticket < env.WallChance ){
			TileContent = TileContent.WALL;
		} 
		ticket -= env.WallChance;
		if (canBeHole && ticket > 0 && ticket < env.HoleChance){
			TileContent = TileContent.HOLE;
		} 
		ticket -= env.HoleChance;
		if (canBeOil && ticket > 0 && ticket < env.BuffFuelChance){
			TileContent = TileContent.BUFF_OIL;
			Value = env.BuffOilValue;
		} 
		ticket -= env.BuffFuelChance;
		float coinChance = env.GetCoinChance (distance);
		if (ticket > 0 && ticket < coinChance) {
			TileContent = TileContent.COIN;
			Value = env.BuffCoinValue;
		}

		InGamePosition igp = gameObject.AddComponent<InGamePosition>();
		igp.x = i;
		igp.y = distance;
	}


}