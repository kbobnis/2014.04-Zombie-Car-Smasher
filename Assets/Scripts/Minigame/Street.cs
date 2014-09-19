using UnityEngine;
using System.Collections.Generic;

public class Street : MonoBehaviour {

	public Dictionary<int, GameObject> Tiles = new Dictionary<int, GameObject>();

	private static Sprite StreetSprite = Resources.Load<Sprite>("Images/street4");

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void Prepare(int inGameY, GameObject previousStreet, bool noObstacles, Environment env){
		SpriteRenderer streetRenderer = gameObject.AddComponent<SpriteRenderer>();
		streetRenderer.sprite = StreetSprite;
		streetRenderer.sortingLayerName = "Layer1";
		streetRenderer.receiveShadows = true;
		streetRenderer.castShadows = true;
		Vector3 size = streetRenderer.sprite.bounds.size;
		gameObject.transform.localScale = new Vector3(InGamePosition.tileW * 5.5f / size.x, InGamePosition.tileH  / size.y);
		
		InGamePosition tmp = gameObject.AddComponent<InGamePosition>();
		tmp.x = 0;
		tmp.y = inGameY;

		bool[] foundWall = new bool[]{ false, false, false };
		bool[] foundHole = new bool[]{ false, false, false} ;
		bool[] foundObstacle = new bool[]{ false, false, false};
		bool[] foundOil = new bool[]{false, false, false};
		//getting previous street
		int i2=0; 
		if (previousStreet != null){
			foreach(GameObject tileGmTmp in previousStreet.GetComponent<Street>().Tiles.Values){
				Tile tileTmp = tileGmTmp.GetComponent<Tile>();

				if (tileTmp.TileContent == TileContent.WALL){
					foundObstacle[i2] = foundWall[i2] = true;
				}
				if (tileTmp.TileContent == TileContent.HOLE){
					foundObstacle[i2] = foundHole[i2] = true;
				}
				if (tileTmp.TileContent == TileContent.BUFF_OIL){
					foundOil[i2] = true;
				}
				i2++;
			}
		}
		bool metOilInThisLevel = false; 
		//left, center, right
		for(int i=-1; i < 2; i++){
			bool canBeWall = true;
			bool canBeHole = true;
			if (i == -1){
				canBeWall = !foundWall[0] && !foundWall[1];
				canBeHole = !foundHole[0] && !foundHole[1];
			}
			if (i == 1){
				canBeWall = !foundWall[1] && !foundWall[2];
				canBeHole = !foundHole[1] && !foundHole[2];
			}
			if (i == 0){
				canBeWall = !foundWall[0] && !foundWall[1] && !foundWall[2];
				canBeHole = !foundHole[0] && !foundHole[1] && !foundHole[2];
			}

			bool canBeOil = !metOilInThisLevel; //there can be only one oil on each level
			if ((i == -1 && foundOil[2]) || (i == 1 && foundOil[0])){  //oils can not be separated by 2 tiles on following levels
				canBeOil = false;
			}
			if (i == 1 && foundObstacle[0] && foundObstacle[1]){
				canBeOil = false;
			}

			if (noObstacles){
				canBeHole = false;
				canBeWall = false;
			}

			GameObject Tile = new GameObject();
			Tile tmp2 = Tile.AddComponent<Tile>();

			tmp2.InitMe(gameObject, i, inGameY, canBeWall, canBeHole, canBeOil, env);

			if (Random.value < 0.1){
				GameObject cactus = new GameObject();
				Tile tmp3 = cactus.AddComponent<Tile>();
				tmp3.InitCactus(gameObject, Random.value<0.5?-2f:2f, inGameY);
			}

			Tiles.Add(i, Tile);

			if (i < 1){
				if (tmp2.TileContent == TileContent.WALL){
					foundObstacle[i+1] = foundWall[i+1] = true;
					foundObstacle[i+2] = foundWall[i+2] = true; //we dont want two obstacles on the same level
				}
				if (tmp2.TileContent == TileContent.HOLE){
					foundObstacle[i+1] = foundHole[i+1] = true;
					foundObstacle[i+2] = foundHole[i+2] = true; //we dont want two obstacles on the same level
				}
			}
			if (tmp2.TileContent == TileContent.BUFF_OIL){
				metOilInThisLevel = true;
			}
		}
	}

	public void UnloadResources(){
		foreach(KeyValuePair<int, GameObject> Tile in Tiles){
			Destroy(Tile.Value);
		}
		Tiles.Clear();
	}


	

}