using UnityEngine;
using System.Collections;

public class GCScript : MonoBehaviour {

	private float autoticker = 0;
	private GoTweenChain chain;
	public int power, endurance, speed, joustbonus, pLocX, pLocZ, oldLocX, oldLocZ, dLocX, dLocZ;
	public GameObject powerNum, endNum, speedNum, jbonusNum, player, enemy;
	private int[,] mapGrid, playerLoc;
	private bool conflict = false;
	private float countdown;

	// Use this for initialization
	void Start () {
		pLocX = 0;
		pLocZ = 0;
		countdown = 0;

	}
	
	// Update is called once per frame
	void Update () {

		if (countdown >= 0){countdown -= Time.deltaTime;}
		else		 
		{

			// checks for key input and if correct, moves
			if (Input.anyKeyDown)
			{
				oldLocX = pLocX;
				oldLocZ = pLocZ;
				if (Input.GetAxis("LERI") < 0)
				{pLocX -= 1;}
				if (Input.GetAxis("LERI") > 0)
				{pLocX += 1;}
				if (Input.GetAxis("UPDO") < 0)
				{pLocZ -= 1;}
				if (Input.GetAxis("UPDO") > 0)
				{pLocZ += 1;}
				if (Input.GetAxis("ULDR") < 0)
				{pLocX -= 1; pLocZ += 1;}
				if (Input.GetAxis("ULDR") > 0)
				{pLocX += 1; pLocZ -= 1;}
				if (Input.GetAxis("DLUR") < 0)
				{pLocX -= 1; pLocZ -= 1;}
				if (Input.GetAxis("DLUR") > 0)
				{pLocX += 1; pLocZ += 1;}
			
				MoveFunct();

			}

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
	}

	void MoveFunct () {

		foreach(GameObject enemy in GameObject.FindGameObjectsWithTag("foe"))
		{
			MonControl Script1 = enemy.GetComponent<MonControl>();
			if (Script1.monX == pLocX && Script1.monZ == pLocZ){conflict = true;}


		}

		if (conflict) {FightFunct();}
		else 
		{

			dLocX = pLocX - oldLocX;
			dLocZ = pLocZ - oldLocZ;
			if (dLocX > 1) {dLocX = 1;}
			if (dLocX < -1) {dLocX = -1;}
			if (dLocZ > 1) {dLocZ = 1;}
			if (dLocZ < -1) {dLocZ = -1;}
			
			GoTween playMoveTween = new GoTween(player.transform, 0.2f, new GoTweenConfig().position (new Vector3( dLocX * 5, 0, dLocZ * 5 ), true));
			chain = new GoTweenChain();
			chain.append(playMoveTween);
			chain.play();
			countdown = 0.25f;
		}
	}
	void FightFunct () {



		conflict = false;
	}
	
}
