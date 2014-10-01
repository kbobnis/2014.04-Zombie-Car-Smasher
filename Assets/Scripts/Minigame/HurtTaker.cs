using UnityEngine;
using System.Collections;

public class HurtTaker : MonoBehaviour {

	private float FadeTime = 2;
	private float LostTime;
	public bool HasLost;
	public string LostReason;

	void Update(){
		if (LostTime != 0 &&  LostTime + FadeTime < Time.time) {
			HasLost = true;
		}
	}

	void OnGUI(){
		if (LostReason != null){
			GuiHelper.DrawText(LostReason, GuiHelper.SmallFont, 0.2, 0.4, 0.6, 0.1);
		}
	}


	public void TakeHurt(Tile fromWhat){
		Handheld.Vibrate ();

		ShieldCompo sc = GetComponent<ShieldCompo> ();
		if (sc == null || !sc.TakeThis(fromWhat)){
			Die (fromWhat);
		}

	}

	private void Die(Tile fromWhat){
		//null is when out of oil
		if (fromWhat != null && (fromWhat.TileContent == TileContent.WALL || fromWhat.TileContent == TileContent.HOLE) ) {
			PlaySingleSound.SpawnSound (Sounds.CartonImpact, Camera.main.transform.position);	
		} 
		LostReason = "Hit into obstacle";
		LostTime = Time.time;
		Destroy (GetComponent<Speeder> ());
		Destroy (GetComponent<CarTurner> ());
	}

	public void OutOfOil(){
		PlaySingleSound.SpawnSound(Sounds.NoMoreFuel, Camera.main.transform.position, 0.4f);
		Die (null);
		LostReason = "Out of fuel";
	}
}
