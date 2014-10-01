using UnityEngine;
using System.Collections;

public class MinigameNotification : MonoBehaviour {

	private string Text;
	private Texture Texture;
	public Vector3 Pos;
	private float LiveTime = 0.5f;

	public void Prepare(string text, Texture texture, Vector3 pos){
		Text = text;
		Pos = new Vector3( pos.x / Screen.width, (Screen.height - pos.y) / Screen.height);
		Texture = texture;
	}

	void Update(){

		LiveTime -= Time.deltaTime;
		if (LiveTime <= 0) {
			Destroy(this);
		}
		Pos.y -= Time.deltaTime / 3 ;
	}


	void OnGUI(){
		GuiHelper.ButtonWithText(Pos.x, Pos.y, 0.20, 0.20, Text, Texture, GuiHelper.MicroFont, delegate(){});
	}
}
