using UnityEngine;
using System.Collections;

public class ShieldCompo : MonoBehaviour {

	private int ShieldCount;
	private GameObject ShieldRenderer;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (ShieldCount < 1) {	
			Destroy(this);
		}
	}

	public void Prepare(Shield shield){
		ShieldCount = (int)shield.Value;
		ShieldRenderer = new GameObject ();
		SpriteRenderer r = ShieldRenderer.AddComponent<SpriteRenderer>();
		Texture2D shieldT = (Texture2D)SpriteManager.GetShield ();
		r.sprite = Sprite.Create (shieldT, new Rect (0, 0, shieldT.width, shieldT.height), new Vector2 (0.5f, 0.5f));
		r.sortingLayerName = "Layer4";
		ShieldRenderer.transform.parent = gameObject.transform;
		ShieldRenderer.name = "Shield";
		ShieldRenderer.transform.localScale = new Vector3 (1, 1, 0);

	}

	void OnDestroy(){
		Destroy (ShieldRenderer);
	}

	public bool TakeThis(Tile tile){
		ShieldCount--;
		PlaySingleSound.SpawnSound (Sounds.DoorSlam, gameObject.transform.position);
		GetComponent<Car> ().ShieldsUsed++;
		tile.TileContent = TileContent.NONE;
		return true;
	}


}
