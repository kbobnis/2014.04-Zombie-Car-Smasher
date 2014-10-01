using UnityEngine;
using System.Collections;

public class ShieldCompo : MonoBehaviour {

	private int shieldValue;
	private GameObject ShieldRenderer;

	// Use this for initialization
	void Start () {
	
	}
	
	public void Prepare(CarStatistic shield){
		shieldValue = (int)shield.Value;
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
		shieldValue--;
		PlaySingleSound.SpawnSound (Sounds.DoorSlam, gameObject.transform.position);
		GetComponent<Car> ().ShieldsUsed++;
		BeingHit bh = tile.gameObject.AddComponent<BeingHit> ();
		bh.Prepare (GetComponent<Speeder> ().v);
		if (shieldValue < 1) {
			gameObject.AddComponent<MinigameNotification> ().Prepare("Shield END", null, Camera.main.WorldToScreenPoint (tile.gameObject.transform.position));
			if (Parameter.IsOn(ParameterType.VIBRATION)){
				Handheld.Vibrate ();
			}
			Destroy (this);
		} else {
			gameObject.AddComponent<MinigameNotification> ().Prepare("-1 Shield", null, Camera.main.WorldToScreenPoint (tile.gameObject.transform.position));
		}
		return true;
	}
	void OnGUI(){
		if ((int)shieldValue > 0){
			GuiHelper.DrawText("shield: "+shieldValue, GuiHelper.SmallFontLeft, 0.11, 0.17, 0.3, 0.15);
		}
	}


}
