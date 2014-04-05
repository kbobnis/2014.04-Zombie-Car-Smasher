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

		float sumOfChances = wallChance + holeChance;
		float ticket = Random.Range(0f, 1f);
		Debug.Log ("ticket was: " + ticket );
		if (ticket < wallChance ){
			TileContent = TileContent.WALL;
			SpriteRenderer r = gameObject.AddComponent<SpriteRenderer>();
			r.sprite = Resources.Load<Sprite>("Images/wall");
			r.sortingLayerName = "Layer2";
			Debug.Log("created wall");
		} else if (ticket < wallChance + holeChance){
			TileContent = TileContent.HOLE;
			SpriteRenderer r = gameObject.AddComponent<SpriteRenderer>();
			r.sprite = Resources.Load<Sprite>("Images/hole");
			r.sortingLayerName = "Layer2";
			Debug.Log("created hole");
		}
		transform.parent = parent.transform;
		transform.localScale = new Vector3(1, 1, 1);
		InGamePosition igp = gameObject.AddComponent<InGamePosition>();
		igp.x = i;
		igp.y = inGameY;

	}
	


}
