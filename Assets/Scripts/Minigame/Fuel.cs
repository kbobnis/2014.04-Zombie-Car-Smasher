using UnityEngine;

public class Fuel : MonoBehaviour {

	private float LowAmountFrom = 0.2f;
	private AudioSource LowFuelAudioSource;

	//public for _Amount visible in unity editor
	public float _Amount;
	public float Amount{
		get { return _Amount; }
		set {
			_Amount = value;
			if (_Amount > MaxAmount){
				_Amount = MaxAmount;
			}
			if (IsLowFuel() ){
				if (LowFuelAudioSource != null && !LowFuelAudioSource.isPlaying){
					LowFuelAudioSource.Play();
				}
			} else {
				if (LowFuelAudioSource != null && LowFuelAudioSource.isPlaying){
					LowFuelAudioSource.Stop();
				}
			}
		}
	}
	public float MaxAmount;
	// Use this for initialization
	void Start () {
		if (LowFuelAudioSource == null) {
			LowFuelAudioSource = gameObject.AddComponent<AudioSource> ();
			LowFuelAudioSource.clip = Sounds.LowFuelSignal;
			LowFuelAudioSource.loop = true;
			LowFuelAudioSource.Stop ();
		}
	}

	public void Prepare (CarStatistic fuelTank, CarStatistic startingOil){
		MaxAmount = (int)fuelTank.Value;
		Amount = (int)startingOil.Value;
	}


	public void PickedUpFuel(float fuelAmount){
		Amount += fuelAmount;
		PlaySingleSound.SpawnSound (Sounds.PickUpFuel, gameObject.transform.position);
	}

	public bool HasEnoughFuel(){
		return Amount > 0;
	}

	public void ChargeForDistance(float distance, float cost){
		Amount -= distance * cost;
	}

	public bool IsLowFuel(){
		return Amount / (float)MaxAmount < LowAmountFrom;
	}
	
	// Update is called once per frame
	void Update () {
		if (Amount < 0){
			Amount = 0;
			GetComponent<HurtTaker>().OutOfOil();
		}
	}

	void OnGUI(){

		if (IsLowFuel()) {
			GuiHelper.DrawElementBlink ("images/fuel", 0.015, 0.114, 0.07, 0.55, -1 , 0.55 * Amount / MaxAmount, true); 
			GuiHelper.DrawElementBlink ("images/border", 0.01, 0.1, 0.1, 0.55);
		} else {
			GuiHelper.DrawElement ("images/fuel", 0.015, 0.114, 0.07, 0.55, -1 , 0.55 * Amount / MaxAmount, true); 
			GuiHelper.DrawElement ("images/border", 0.01, 0.1, 0.1, 0.55);
		}

		GUI.DrawTexture(new Rect(GuiHelper.PercentW(0.0), GuiHelper.PercentH(0.6), GuiHelper.PercentW(0.15), GuiHelper.PercentH(0.1)), SpriteManager.GetOilTexture());
		
	}

	void OnDestroy() {
		if (GetComponent<AudioSource>() && GetComponent<AudioSource>().isPlaying) {
			GetComponent<AudioSource>().Stop();
		}
		if (LowFuelAudioSource != null) {
			LowFuelAudioSource.Stop();
			LowFuelAudioSource = null;
		}
	}



}
