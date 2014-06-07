using UnityEngine;
using System.Collections;


enum SIDE {
	LEFT, RIGHT, NOWHERE
}

public class CarTurner : MonoBehaviour {

	private SIDE WhereTurn = SIDE.NOWHERE;
	public static float TurnSpeed = 5f;

	private Texture2D LeftIcon;
	private Texture2D RightIcon;


	// Use this for initialization
	void Start () {

		LeftIcon = Resources.Load<Texture2D>("Images/left");
		RightIcon = Resources.Load<Texture2D> ("Images/right");
	
	}
	
	// Update is called once per frame
	void Update () {

		float howUpdate = 0;
		float oldX = GetComponent<InGamePosition>().x;

		switch(WhereTurn){
			case SIDE.LEFT:{
				
				int stopPoint = Mathf.FloorToInt(oldX );
				howUpdate += -1 * TurnSpeed * Time.deltaTime;
				if (oldX + howUpdate < stopPoint){
					howUpdate = -1 * oldX + stopPoint ;
					WhereTurn = SIDE.NOWHERE;
				}
				break;
			}
			case SIDE.RIGHT: { 
				howUpdate +=  TurnSpeed * Time.deltaTime;

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
		
		if (!IsTurning()){
			if(carX >= 0 && (GUI.RepeatButton(new Rect(GuiHelper.oneTenthW, Screen.height*(2/3f), GuiHelper.oneThirdW, GuiHelper.twentyPercent), LeftIcon ) || Input.GetKeyDown(KeyCode.A))  ){
				TurnLeft();
			}
			if(carX <= 0 && (GUI.RepeatButton(new Rect(Screen.width - GuiHelper.oneThirdW - GuiHelper.oneTenthW, Screen.height*(2/3f), GuiHelper.oneThirdW, GuiHelper.twentyPercent), RightIcon) || Input.GetKeyDown(KeyCode.D))){
				TurnRight();
			}
		}
	}


}
