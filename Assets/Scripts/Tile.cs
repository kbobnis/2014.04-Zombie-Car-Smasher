using UnityEngine;

public enum TileContent{
	WALL, 
	HOLE,
	NONE
}



public class Tile : MonoBehaviour 
{
	public TileContent TileContent;
	private static Random Random = new Random();

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
	}

	public void InitMe(float wallChance, float holeChance, GameObject parent, int i, int inGameY){
		TileContent = TileContent.NONE;
		float sumOfChances = wallChance + holeChance;
		float ticket = Random.Range(0f, 1f);
		Debug.Log ("ticket was: " + ticket );
		SpriteRenderer r = null;
		if (ticket < wallChance ){
			TileContent = TileContent.WALL;
			r = gameObject.AddComponent<SpriteRenderer>();
			r.sprite = Resources.Load<Sprite>("Images/wall");
			Debug.Log("created wall");
		} else if (ticket < wallChance + holeChance){
			TileContent = TileContent.HOLE;
			r = gameObject.AddComponent<SpriteRenderer>();
			r.sprite = Resources.Load<Sprite>("Images/hole");
			Debug.Log("created hole");
		}
		transform.localScale = new Vector3(1, 1, 1);
		transform.parent = parent.transform;
		if (r != null){
			r.sortingLayerName = "Layer2";
			float scale = InGamePosition.tileW/(float) r.sprite.bounds.size.x ;
			transform.localScale = new Vector3(scale, scale, 0);
		}
		InGamePosition igp = gameObject.AddComponent<InGamePosition>();
		igp.x = i;
		igp.y = inGameY;
	}
	


}
