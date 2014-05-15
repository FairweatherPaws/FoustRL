using UnityEngine;
using System.Collections;

public class SpinAttackButton : MonoBehaviour {

	private GameObject gc;
	public bool highlightTrigger = false;
	public bool dimTrigger = false;
	private Color oldColour;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnMouseEnter() {
		oldColour = renderer.material.color;
	}

	void OnMouseOver() {
		gc = GameObject.FindGameObjectWithTag("GameController");
		GCScript Script1 = gc.GetComponent<GCScript>();
		if (Script1.joustbonus > 4) {
			renderer.material.color = new Color(1f, 1f, 1f);
			if (Input.GetMouseButtonDown(0)){
				Script1.spinBtnPressed = true;
			}
		}
		else {renderer.material.color = new Color(0.5f, 0.5f, 0.5f);}
	}

	void OnMouseExit(){
		gc = GameObject.FindGameObjectWithTag("GameController");
		GCScript Script1 = gc.GetComponent<GCScript>();
		if (Script1.joustbonus > 4) {
			renderer.material.color = new Color(0.8f, 0.8f, 0.8f);
		}
		else {renderer.material.color = new Color(0.5f, 0.5f, 0.5f);}
	}
}
