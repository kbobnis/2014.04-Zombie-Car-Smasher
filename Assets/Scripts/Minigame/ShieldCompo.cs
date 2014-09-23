using UnityEngine;
using System.Collections;

public class ShieldCompo : MonoBehaviour {

	private CarStatistic ShieldStatistic;
	private GameObject ShieldRenderer;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (ShieldStatistic.Value < 1) {	
			Destroy(this);
		}
	}

	public void Prepare(CarStatistic shield){
		ShieldStatistic = shield;
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
		ShieldStatistic.Downgrade (1);
		PlaySingleSound.SpawnSound (Sounds.DoorSlam, gameObject.transform.position);
		GetComponent<Car> ().ShieldsUsed++;
		BeingHit bh = tile.gameObject.AddComponent<BeingHit> ();
		bh.Prepare (GetComponent<Speeder> ().v);
		return true;
	}
	void OnGUI(){
		if ((int)ShieldStatistic.Value > 0){
			GuiHelper.DrawText("shield: "+(int)ShieldStatistic.Value, GuiHelper.SmallFontLeft, 0.1, 0.1, 0.3, 0.15);
		}
	}


}
