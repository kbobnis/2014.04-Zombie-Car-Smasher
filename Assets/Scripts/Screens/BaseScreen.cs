using UnityEngine;
using System.Collections;

abstract public class BaseScreen : MonoBehaviour {

	protected AfterButton AfterButton;
	private bool ShowBackground;
	public int GuiDepth;

	abstract protected void StartInner ();

	void Start(){
		GuiDepth = --Game.Me.ClosestGui;
		StartInner ();
	}

	void OnDestroy(){
		//Game.Me.ClosestGui++;
	}

	public void Prepare (AfterButton afterButton, bool showBackground=true){
		AfterButton = afterButton;
		ShowBackground = showBackground;
	}

	virtual protected void UpdateInner(){
	}

	abstract protected void OnGUIInner();

	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape)){
			AfterButton();
		}
		UpdateInner ();
	}

	void OnGUI(){
		GUI.depth = GuiDepth;
		if (ShowBackground){
			GuiHelper.DrawBackground (delegate() {
				AfterButton();
			});
		}
		OnGUIInner ();
		GUI.depth = GuiDepth + 1;
	}


}
