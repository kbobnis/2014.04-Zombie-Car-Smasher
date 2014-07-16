using UnityEngine;
using System.Collections;


enum SIDE {
	LEFT, RIGHT, NOWHERE
}

public class CarTurner : MonoBehaviour {

	private SIDE WhereTurn = SIDE.NOWHERE;
	public static float TurnSpeed = 5f;

	private bool pressed;

	private float LastY;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		float thisY = GetComponent<InGamePosition> ().y;
		if (LastY == 0) {
			LastY = thisY;
		}
		float deltaY = thisY - LastY;
		float howUpdate = 0;
		float oldX = GetComponent<InGamePosition>().x;

		switch(WhereTurn){
			case SIDE.LEFT:{
				
				int stopPoint = Mathf.FloorToInt(oldX );
				howUpdate += -1 * TurnSpeed * deltaY;
				if (oldX + howUpdate < stopPoint){
					howUpdate = -1 * oldX + stopPoint ;
					WhereTurn = SIDE.NOWHERE;
				}
				break;
			}
			case SIDE.RIGHT: { 
				howUpdate +=  TurnSpeed * deltaY;

				int stopPoint = (int)(oldX + 1);
				if (oldX + howUpdate > stopPoint){
					howUpdate = stopPoint - oldX;
					WhereTurn = SIDE.NOWHERE;
				}
				break;
			}
		}

		if (howUpdate != 0){
			GetComponent<InGamePosition>().x += howUpdate;
		}

		if (Input.touchCount == 0)
		{
			pressed = false;
		}
		LastY = thisY;
	}

	public void TurnLeft(){
		GetComponent<InGamePosition>().x -= 0.01f;
		WhereTurn = SIDE.LEFT;
	}

	public void  TurnRight(){
		GetComponent<InGamePosition>().x += 0.01f;
		WhereTurn = SIDE.RIGHT;
	}

	public bool IsTurning() {
		return WhereTurn != SIDE.NOWHERE;
	}

	void OnGUI() {
		float carX = GetComponent<InGamePosition>().x;


		if (!IsTurning() && !pressed){
			if(carX >= 0 && 
			   (
				GUI.RepeatButton(new Rect(GuiHelper.PercentW(0.01), GuiHelper.PercentH(0.75), GuiHelper.PercentW(0.44), GuiHelper.PercentH(0.24)),  SpriteManager.GetLeftArrow() ) 
				|| Input.GetKeyDown(KeyCode.A))  ){
				TurnLeft();
				pressed = true;
			}
			if(carX <= 0 && (GUI.RepeatButton(new Rect(GuiHelper.PercentW(0.55), GuiHelper.PercentH(0.75), GuiHelper.PercentW(0.44), GuiHelper.PercentH(0.24)), SpriteManager.GetRightArrow()) || Input.GetKeyDown(KeyCode.D))){
				TurnRight();
				pressed = true;
			}
		}

		//GUI.Label (new Rect (Screen.width * 4 / 5, 0, Screen.width / 5, Screen.height), "Touches: " + Input.touchCount + ", mouse button down: " + Input.GetMouseButtonDown(0));
	}


}
