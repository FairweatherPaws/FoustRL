using UnityEngine;
using System.Collections;

public class BloodEffectTimeOut : MonoBehaviour {
	private float deathCount = 15f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		deathCount -= Time.deltaTime;
		if (deathCount < 0) {Destroy(this.gameObject);}
	}
}
