using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

	private float autoticker = 0;
	public int power, endurance, speed, joustbonus;
	public GameObject powerNum, endNum, speedNum, jbonusNum;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

		// refreshes stats every 15 frames
		if (autoticker < 15){autoticker++;}
		else 
		{

			powerNum.GetComponent<TextMesh>().text = power.ToString();
			endNum.GetComponent<TextMesh>().text = endurance.ToString();
			speedNum.GetComponent<TextMesh>().text = speed.ToString();
			jbonusNum.GetComponent<TextMesh>().text = joustbonus.ToString();

			autoticker = 0;
		}
	}

	void MoveFunct () {



	}
	
}
