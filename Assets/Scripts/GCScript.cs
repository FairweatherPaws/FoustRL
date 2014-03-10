using UnityEngine;
using System.Collections;

public class GCScript : MonoBehaviour {

	private float autoticker = 0;
	private GoTweenChain chain;
	public int power, endurance, speed, joustbonus, playHP, experience, pLocX, pLocZ, oldLocX, oldLocZ, dLocX, dLocZ, level, movement, qad;
	public GameObject powerNum, endNum, speedNum, jbonusNum, moveNum, HPNum, EXPNum, levelNum, player, enemy, stabVictim;
	private int[,] mapGrid, playerLoc;
	private bool conflict = false;
	private float countdown;
	public bool gameOver = false;
	public int newMonX, newMonZ, ranSide, ranSquare, ranType;
	public Transform newMonType, goblin;
	
	// Use this for initialization
	void Start () {
		pLocX = 0;
		pLocZ = 0;
		movement = 2;
		level = 0;
		power = 1;
		endurance = 1;
		experience = 0;
		playHP = 10;
		speed = 2;
		joustbonus = 0;
		countdown = 0;

	}
	
	// Update is called once per frame
	void Update () {
		if (gameOver) {Application.Quit();}
		if (countdown > 0) {countdown -= Time.deltaTime;}
		else 
		{
			if (movement <= 0){movement = speed;}
			else		 
			{

				// checks for key input and if correct, moves
				if (Input.anyKeyDown)
				{
					oldLocX = pLocX;
					oldLocZ = pLocZ;
					if (Input.GetAxis("LERI") < 0)
					{pLocX -= 1; MoveFunct();}
					if (Input.GetAxis("LERI") > 0)
					{pLocX += 1; MoveFunct();}
					if (Input.GetAxis("UPDO") < 0)
					{pLocZ -= 1; MoveFunct();}
					if (Input.GetAxis("UPDO") > 0)
					{pLocZ += 1; MoveFunct();}
					if (Input.GetAxis("ULDR") < 0)
					{pLocX -= 1; pLocZ += 1; MoveFunct();}
					if (Input.GetAxis("ULDR") > 0)
					{pLocX += 1; pLocZ -= 1; MoveFunct();}
					if (Input.GetAxis("DLUR") < 0)
					{pLocX -= 1; pLocZ -= 1; MoveFunct();}
					if (Input.GetAxis("DLUR") > 0)
					{pLocX += 1; pLocZ += 1; MoveFunct();}
					if (Input.GetAxis("Wait") > 0) 
					{MoveFunct();}
				


				}
			}
		}
		// refreshes stats every 15 frames
		if (autoticker < 15){autoticker++;}
		else 
		{
			
			powerNum.GetComponent<TextMesh>().text = power.ToString();
			endNum.GetComponent<TextMesh>().text = endurance.ToString();
			speedNum.GetComponent<TextMesh>().text = speed.ToString();
			jbonusNum.GetComponent<TextMesh>().text = joustbonus.ToString();
			moveNum.GetComponent<TextMesh>().text = movement.ToString();
			HPNum.GetComponent<TextMesh>().text = playHP.ToString();
			EXPNum.GetComponent<TextMesh>().text = experience.ToString();
			levelNum.GetComponent<TextMesh>().text = level.ToString();
			
			autoticker = 0;
		}
	}

	void MoveFunct () {
		movement--;
		joustbonus++;
		foreach(GameObject enemy in GameObject.FindGameObjectsWithTag("foe"))
		{

			MonControl Script1 = enemy.GetComponent<MonControl>();
			if (Script1.monX == pLocX && Script1.monZ == pLocZ){conflict = true; stabVictim = enemy;}
			if (movement <= 0) {Script1.monTurn = true; Script1.monMove = Script1.monSpe;}

		}

		if (conflict) 
		{
			FightFunct();
			pLocX = oldLocX;
			pLocZ = oldLocZ;
		}
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
			if (movement <= 0) {MonSpawn ();}
		}
	}
	void FightFunct () {

		MonControl Script3 = stabVictim.GetComponent<MonControl>();
		qad = 0;
		if (Script3.monEnd <= power * joustbonus) {qad = power * joustbonus;}
		if (Script3.monHP > qad){Script3.monHP = Script3.monHP - qad;}
		else {experience += Script3.monExp; Destroy(stabVictim);}
		// add level up flag here
		joustbonus = 0;
		conflict = false;
		countdown = 0.25f;
		if (movement <= 0) {MonSpawn ();}
	}

	void MonSpawn () {

		ranSide = Random.Range (0, 3);
		ranSquare = Random.Range (0, 10);
		ranType = Random.Range (0, level);
		if (ranSide == 0) {newMonX = -5; newMonZ = ranSquare - 5;}
		if (ranSide == 1) {newMonZ = 5; newMonX = ranSquare - 5;}
		if (ranSide == 2) {newMonX = 5; newMonZ = ranSquare - 5;}
		if (ranSide == 3) {newMonZ = -5; newMonX = ranSquare - 5;}
		if (ranType == 0) {newMonType = goblin;}
		Instantiate(newMonType, new Vector3(newMonX*5f, 2.5f, newMonZ*5f), Quaternion.identity);

	}
	
}
