﻿using UnityEngine;
using System.Collections;

public class GCScript : MonoBehaviour {

	private float autoticker = 0;
	private GoTweenChain chain;
	public int power, endurance, speed, joustbonus, playHP, experience, pLocX, pLocZ, oldLocX, oldLocZ, dLocX, dLocZ, level, movement, qad;
	public int mapX, mapZ, killCount, spawnCount, monCount;
	public GameObject powerNum, endNum, speedNum, jbonusNum, moveNum, HPNum, EXPNum, levelNum, killsNum, gameOverText, player, enemy, stabVictim;
	public GameObject playerChargeHalo, playerChargeHaloSuper, playerChargeHaloThree;
	private int[,] mapGrid, playerLoc;
	private bool conflict = false;
	private bool blinkUp = true;
	private bool rotateCW = true;
	private float countdown, cooldown, ticker;
	public bool gameOver = false;
	public int newMonX, newMonZ, ranSide, ranSquareX, ranSquareZ, ranType;
	public Transform newMonType, goblin, ork, krusher, damageEffect, gOT;
	
	// Use this for initialization
	void Start () {
		pLocX = 0;
		pLocZ = 0;
		movement = 2;
		level = 0;
		power = 5;
		endurance = 0;
		experience = 0;
		playHP = 100;
		speed = 2;
		joustbonus = 0;
		killCount = 0;
		countdown = 0;
		cooldown = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (gameOver) {
			if (Input.anyKeyDown) {Application.Quit();}
			gameOverText.GetComponent<TextMesh>().text = "Yo' ass is dead!";
				// annoying ass blink loop
			if (blinkUp) {
				gameOverText.GetComponent<TextMesh>().fontSize++;
				if (gameOverText.GetComponent<TextMesh>().fontSize >= 120) {blinkUp = false;}
			}
			else {
				gameOverText.GetComponent<TextMesh>().fontSize--;
				if (gameOverText.GetComponent<TextMesh>().fontSize < 80) {blinkUp = true;}
			}

			if (rotateCW) {gOT.Rotate(0, 0, 100 * Time.deltaTime); countdown += Time.deltaTime;
				if (countdown > 0.3f) {rotateCW = false; countdown = 0;}
			}
			else {gOT.Rotate(0, 0, -100 * Time.deltaTime); countdown += Time.deltaTime;
				if (countdown > 0.3f) {rotateCW = true; countdown = 0;}
			}

		}

		else {

			if (countdown > 0) {countdown -= Time.deltaTime;}
			else 
			{
				// playerChargeHalo.transform.position = new Vector3(player.transform.position.x * 1.05f, 5.5f, player.transform.position.z * 1.05f);
				playerChargeHalo.transform.position = player.transform.position * 1.05f;
				playerChargeHalo.light.intensity = joustbonus;
				if (joustbonus >= 8) {playerChargeHaloSuper.light.intensity = joustbonus - 8;}
				else {playerChargeHaloSuper.light.intensity = 0;}
				if (joustbonus >= 16) {playerChargeHaloThree.light.intensity = joustbonus - 16;}
				else {playerChargeHaloThree.light.intensity = 0;}
				if (joustbonus >= 25 && joustbonus < 50) {player.particleSystem.emissionRate = (joustbonus - 24) * 100;
					player.particleSystem.startSpeed = 5 + 2 * (joustbonus - 24);}
				else {player.particleSystem.emissionRate = 0;}
				if (joustbonus >= 50) {player.particleSystem.Emit (4000); 
					player.particleSystem.startSpeed = 5 + (joustbonus - 50);
					player.particleSystem.emissionRate = 0;}
				if (movement <= 0){movement = speed;}
				else		 
				{
					cooldown -= Time.deltaTime;
					// checks for key input and if correct, moves
					if (Input.anyKeyDown && cooldown < 0)
					{
						cooldown = 0.1f;
						oldLocX = pLocX;
						oldLocZ = pLocZ;
						if (Input.GetAxis("LERI") < 0) { 
							if (Mathf.Abs ( pLocX - 1 ) < mapX) {pLocX -= 1; MoveFunct();}
						}
						if (Input.GetAxis("LERI") > 0) {
							if (Mathf.Abs ( pLocX + 1 ) < mapX) {pLocX += 1; MoveFunct();}
						}
						if (Input.GetAxis("UPDO") < 0) {
							if (Mathf.Abs ( pLocZ - 1 ) < mapZ){pLocZ -= 1; MoveFunct();}
						}
						if (Input.GetAxis("UPDO") > 0) {
							if (Mathf.Abs ( pLocZ + 1 ) < mapZ){pLocZ += 1; MoveFunct();}
						}
						if (Input.GetAxis("ULDR") < 0) {
							if (Mathf.Abs ( pLocX - 1 ) < mapX && Mathf.Abs (pLocZ + 1) < mapZ)	{pLocX -= 1; pLocZ += 1; MoveFunct();}
						}
						if (Input.GetAxis("ULDR") > 0) {
							if (Mathf.Abs ( pLocX + 1) < mapX && Mathf.Abs (pLocZ - 1) < mapZ)	{pLocX += 1; pLocZ -= 1; MoveFunct();}
						}
						if (Input.GetAxis("DLUR") < 0) {
							if (Mathf.Abs ( pLocX - 1 ) < mapX && Mathf.Abs (pLocZ - 1) < mapZ)	{pLocX -= 1; pLocZ -= 1; MoveFunct();}
						}
						if (Input.GetAxis("DLUR") > 0) {
							if (Mathf.Abs ( pLocX + 1) < mapX && Mathf.Abs (pLocZ + 1) < mapZ)	{pLocX += 1; pLocZ += 1; MoveFunct();}
						}
						if (Input.GetAxis("Wait") > 0) 
						{joustbonus = 0; MoveFunct();}
						
						
						
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
				killsNum.GetComponent<TextMesh>().text = killCount.ToString();
				
				autoticker = 0;
			}
		}
	}

	void MoveFunct () {
		movement--;
		joustbonus++;
	
		foreach(GameObject enemy in GameObject.FindGameObjectsWithTag("foe"))
		{

			MonControl Script1 = enemy.GetComponent<MonControl>();
			if (Script1.monX == pLocX && Script1.monZ == pLocZ){conflict = true; stabVictim = enemy;}
			//if (movement <= 0) {Script1.monTurn = true;}

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
			
			GoTween playMoveTween = new GoTween(player.transform, 0.2f, new GoTweenConfig().position (new Vector3( pLocX * 5, 2.5f, pLocZ * 5 )));
			chain = new GoTweenChain();
			chain.append(playMoveTween);
			chain.play();
			countdown = 0f;
			if (movement <= 0) {MonSpawn ();}
		}


	}
	void FightFunct () {

		MonControl Script3 = stabVictim.GetComponent<MonControl>();
		qad = Random.Range (power * joustbonus, 2 * power * joustbonus);
		if (Script3.monEnd >= qad) {qad = 0;}
		else {qad -= Script3.monEnd;}
		if (Script3.monHP > qad){Script3.monHP = Script3.monHP - qad;}
		else {experience += Script3.monExp; Destroy(stabVictim); killCount++;}
		if (qad > 0) 
		{
			Instantiate(damageEffect, new Vector3(
				(stabVictim.transform.position.x + player.transform.position.x) / 2 * 0.95f, 5f, 
				(stabVictim.transform.position.z + player.transform.position.z) / 2 * 0.95f), 
			            Quaternion.Euler(90, 330, 0));
		}
		// add level up flag here
		joustbonus = 0;
		conflict = false;
		countdown = 0f;
		if (movement <= 0) {MonSpawn ();}

	}

	void MonSpawn () {

		foreach(GameObject enemy in GameObject.FindGameObjectsWithTag("foe"))
		{
			ticker = 0.02f;
			while (ticker > 0) { ticker -= Time.deltaTime;}
			MonControl Script1 = enemy.GetComponent<MonControl>();
			if (movement <= 0) {Script1.monTurn = true; Script1.runOnce = true;}
			
		}


		if (experience >= (2*level+1)*10) 
		{
			power += 5;
			speed++;
			endurance += 5;
			playHP += Random.Range (5, 16) * 10;
			experience -= (2*level+1)*10;
			level++;
		}
		spawnCount = 0;
		monCount = 0;
	redux:
		ranSide = Random.Range (0, 3);
		ranSquareX = Random.Range (1, mapX*2 - 1);
		ranSquareZ = Random.Range (1, mapZ*2 - 1);
		ranType = Random.Range (0, (level+1)*25);
		if (ranSide == 0) {newMonX = -mapX + 1; newMonZ = ranSquareZ - mapZ;} // spawn on a random tile on edge clockwise from left
		if (ranSide == 1) {newMonZ = mapZ - 1; newMonX = ranSquareX - mapX;} 
		if (ranSide == 2) {newMonX = mapX - 1; newMonZ = ranSquareZ - mapZ;}
		if (ranSide == 3) {newMonZ = -mapZ + 1; newMonX = ranSquareX - mapX;}
		if (newMonX == pLocX && newMonZ == pLocZ) {goto redux;} // do not spawn on player
		foreach(GameObject enemy in GameObject.FindGameObjectsWithTag("foe")) // do not spawn on other enemy
		{
			monCount++;
			MonControl Script1 = enemy.GetComponent<MonControl>();
			if (Script1.monX == newMonX && Script1.monZ == newMonZ){goto redux;}	
		}
		if (ranType < 40) {newMonType = goblin;}
		if (40 < ranType && ranType < 70) {newMonType = ork;}
		if (70 < ranType && ranType < 120) {newMonType = krusher;}
		

		if (monCount < 20) {Instantiate(newMonType, new Vector3(newMonX*5f, 2.5f, newMonZ*5f), Quaternion.identity);}

		if (spawnCount < level) {spawnCount++; goto redux;}

	}
	
}
