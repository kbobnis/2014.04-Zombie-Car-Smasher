using UnityEngine;
using System.Collections.Generic;



public class Street : MonoBehaviour {

	public Dictionary<int, GameObject> Tiles = new Dictionary<int, GameObject>();

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void Prepare(int inGameY, float WallChance, float HoleChance){
		SpriteRenderer streetRenderer = gameObject.AddComponent<SpriteRenderer>();
		streetRenderer.sprite = Resources.Load<Sprite>("Images/street");
		streetRenderer.sortingLayerName = "Layer1";
		streetRenderer.receiveShadows = true;
		streetRenderer.castShadows = true;
		Vector3 size = streetRenderer.sprite.bounds.size;
		gameObject.transform.localScale = new Vector3(InGamePosition.tileW * 3 / size.x, InGamePosition.tileH  / size.y);
		
		InGamePosition tmp = gameObject.AddComponent<InGamePosition>();
		tmp.x = 0;
		tmp.y = inGameY;
		
		//left, center, right
		for(int i=-1; i < 2; i++){
			GameObject Tile = new GameObject();
			Tile tmp2 = Tile.AddComponent<Tile>();
			tmp2.InitMe(WallChance, HoleChance, gameObject, i, inGameY);

			Tiles.Add(i, Tile);
		}
	}

	public void UnloadResources(){
		foreach(KeyValuePair<int, GameObject> Tile in Tiles){
			Destroy(Tile.Value);
		}
		Tiles.Clear();
	}


	

}