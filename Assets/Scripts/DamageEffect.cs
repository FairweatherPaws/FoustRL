using UnityEngine;
using System.Collections;

public class DamageEffect : MonoBehaviour {

	private float resize, selfDestructTimer;
	private GameObject gc;
	Color color;

	void Awake () {
		resize = 0;
		selfDestructTimer = 0;
		gc = GameObject.FindGameObjectWithTag("GameController");
		GCScript Script1 = gc.GetComponent<GCScript>();
		this.GetComponent<TextMesh>().text = Script1.totalAggro.ToString();
		color = this.renderer.material.color;
		Debug.Log(transform.position);

	}


	// Use this for initialization

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		resize += Time.deltaTime;
		selfDestructTimer += Time.deltaTime;
		this.GetComponent<TextMesh>().fontSize++;
		// if (resize > 0.02f) {this.GetComponent<TextMesh>().fontSize++; resize = 0;}
		if (selfDestructTimer > 0.6f) {color.a -= 0.05f; this.renderer.material.color = color;}
		if (selfDestructTimer > 1) {Destroy(this.gameObject);}
		transform.position = transform.position * 1.01f;
	
	}
}
