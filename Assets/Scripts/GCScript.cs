using UnityEngine;
using System.Collections;

public class GCScript : MonoBehaviour {

	private float autoticker = 0;
	private GoTweenChain chain;
	public int power, endurance, speed, joustbonus, playHP, experience, pLocX, pLocZ, oldLocX, oldLocZ, dLocX, dLocZ, level, movement, totalAggro, damageNumber;
	public int mapX, mapZ, killCount, spawnCount, monCount, maxPierce, curPierce;
	public GameObject powerNum, endNum, speedNum, jbonusNum, moveNum, HPNum, EXPNum, levelNum, killsNum, gameOverText, player, enemy, stabVictim, stabVic2;
	public GameObject playerChargeHalo, playerChargeHaloSuper, playerChargeHaloThree, background, prevStabVic, spinEffect, spinBtn;
	private int[,] mapGrid, playerLoc;
	private bool conflict = false;
	private bool blinkUp = true;
	private bool rotateCW = true;
	private bool piercingBlow = false;
	private bool hasPierced = false;
	private bool spinMode = false;
	public bool spinBtnPressed = false;
	private Quaternion currentDir;
	private float countdown, cooldown, ticker, spinLim;
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
		maxPierce = 1;
	}
	
	// Update is called once per frame
	void Update () {
		if (gameOver) {
			if (Input.anyKeyDown) {Application.LoadLevel (Application.loadedLevelName);}
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

			if (countdown > 0) {
				countdown -= Time.deltaTime;
				Color bgShift = background.renderer.material.color;
				if (bgShift.r < 1) {bgShift.r += 5 * Time.deltaTime;}
				if (bgShift.g > 0) {bgShift.g -= 5 * Time.deltaTime;}
				background.renderer.material.color = bgShift;
			}
			else 
			{
				Color bgShift = background.renderer.material.color;
				if (bgShift.r > 0) {bgShift.r -= 5 * Time.deltaTime;}
				if (bgShift.g < 1) {bgShift.g += 5 * Time.deltaTime;}
				background.renderer.material.color = bgShift;
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
							player.transform.rotation = Quaternion.Euler(90,90,0);
							if (Mathf.Abs ( pLocX - 1 ) < mapX) {pLocX -= 1; MoveFunct();}
						}
						if (Input.GetAxis("LERI") > 0) {
							player.transform.rotation = Quaternion.Euler(90,270,0);
							if (Mathf.Abs ( pLocX + 1 ) < mapX) {pLocX += 1; MoveFunct();}
						}
						if (Input.GetAxis("UPDO") < 0) {
							player.transform.rotation = Quaternion.Euler(90,0,0);
							if (Mathf.Abs ( pLocZ - 1 ) < mapZ){pLocZ -= 1; MoveFunct();}
						}
						if (Input.GetAxis("UPDO") > 0) {
							player.transform.rotation = Quaternion.Euler(90,180,0);
							if (Mathf.Abs ( pLocZ + 1 ) < mapZ){pLocZ += 1; MoveFunct();}
						}
						if (Input.GetAxis("ULDR") < 0) {
							player.transform.rotation = Quaternion.Euler(90,135,0);
							if (Mathf.Abs ( pLocX - 1 ) < mapX && Mathf.Abs (pLocZ + 1) < mapZ)	{pLocX -= 1; pLocZ += 1; MoveFunct();}
						}
						if (Input.GetAxis("ULDR") > 0) {
							player.transform.rotation = Quaternion.Euler(90,315,0);
							if (Mathf.Abs ( pLocX + 1) < mapX && Mathf.Abs (pLocZ - 1) < mapZ)	{pLocX += 1; pLocZ -= 1; MoveFunct();}
						}
						if (Input.GetAxis("DLUR") < 0) {
							player.transform.rotation = Quaternion.Euler(90,45,0);
							if (Mathf.Abs ( pLocX - 1 ) < mapX && Mathf.Abs (pLocZ - 1) < mapZ)	{pLocX -= 1; pLocZ -= 1; MoveFunct();}
						}
						if (Input.GetAxis("DLUR") > 0) {
							player.transform.rotation = Quaternion.Euler(90,225,0);
							if (Mathf.Abs ( pLocX + 1) < mapX && Mathf.Abs (pLocZ + 1) < mapZ)	{pLocX += 1; pLocZ += 1; MoveFunct();}
						}
						if (Input.GetAxis("Wait") > 0) {joustbonus = 0; MoveFunct();}
						if (Input.GetAxis("Spinzaku") > 0 || spinBtnPressed == true) 
						{
							if (joustbonus > 4) {joustbonus -= 5; Spinzaku();}
						}
						
						
						
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

			if (spinMode){
				player.transform.Rotate(0, 15f, 0, Space.World);
				spinLim -= 15;
				if (spinLim < 0) {
					spinMode = false; 
					player.transform.rotation = currentDir; 
					spinEffect.renderer.enabled = false;}
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
		
		totalAggro = Random.Range (power * joustbonus, 2 * power * joustbonus); //initial attack damage
		int plusoneX = pLocX - oldLocX; // get attack direction
		int plusoneZ = pLocZ - oldLocZ;
		curPierce = maxPierce;
		prevStabVic = stabVictim;
	doubleAttack: // piercing checkpoint
		piercingBlow = false; // prevents autoloop
		MonControl Script3 = stabVictim.GetComponent<MonControl>();
		Vector3 stabby = stabVictim.transform.position;
		Vector3 prevStabby = prevStabVic.transform.position;
		if (Script3.monEnd >= totalAggro) {totalAggro = 0;} // check if DR too high
		else {totalAggro -= Script3.monEnd;} // if not, reduce damage by DR
		damageNumber = totalAggro;
		if (Script3.monHP > totalAggro){ // do they survive?
			Script3.monHP = Script3.monHP - totalAggro; // if so, reduce damage from HP
		}

		else {
			totalAggro -= Script3.monHP; //if not, reduce damage by HP
			experience += Script3.monExp; //give XP
			Destroy(stabVictim); //slay
			killCount++; //ding
			pLocX += plusoneX;
			pLocZ += plusoneZ;
			if (curPierce > 0) {
				foreach(GameObject enemy in GameObject.FindGameObjectsWithTag("foe")) //check if anyone's behind the dead guy
				{
					MonControl Script1 = enemy.GetComponent<MonControl>();
					if (Script1.monX == pLocX && Script1.monZ == pLocZ){ // if someone is
						piercingBlow = true; // loop trigger
						stabVic2 = enemy; // sets new target
						curPierce--;
					}
					//if (movement <= 0) {Script1.monTurn = true;}
				}
			}

		}
		if (damageNumber > 0) 
		{
			if (hasPierced) {
				Instantiate(damageEffect, new Vector3(
					(stabby.x + prevStabby.x) / 2 * 0.95f, 
					5f, 
					(stabby.z + prevStabby.z) / 2 * 0.95f), 
				            Quaternion.Euler(90, 330, 0));

			}
			else {
				Instantiate(damageEffect, new Vector3(
					(stabby.x + player.transform.position.x) / 2 * 0.95f, 
					5f, 
					(stabby.z + player.transform.position.z) / 2 * 0.95f), 
				            Quaternion.Euler(90, 330, 0));
				}
			hasPierced = false;
		}
		// add level up flag here
		if (piercingBlow) {prevStabVic = stabVictim; stabVictim = stabVic2; hasPierced = true; goto doubleAttack;}
		joustbonus = 0;
		conflict = false;
		countdown = 0f;
		if (movement <= 0) {MonSpawn ();}

	}

	void MonSpawn () {

		countdown = 0.5f;

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

	void Spinzaku() {
		foreach(GameObject enemy in GameObject.FindGameObjectsWithTag("foe")) //check for foes in range of 1
		{
			MonControl Script1 = enemy.GetComponent<MonControl>();
			if (Mathf.Sqrt((Script1.monX-pLocX)*(Script1.monX-pLocX)+(Script1.monZ-pLocZ)*(Script1.monZ-pLocZ)) < 2){ // if someone is
				totalAggro = 25 + 15 * level;
				if (Script1.monEnd > totalAggro) {totalAggro = 0;}
				else {totalAggro -= Script1.monEnd;}
				damageNumber = totalAggro;
				Instantiate(damageEffect, new Vector3(
					(enemy.transform.position.x + player.transform.position.x) / 2 * 0.95f, 
					5f, 
					(enemy.transform.position.z + player.transform.position.z) / 2 * 0.95f), 
				            Quaternion.Euler(90, 330, 0));
				if (Script1.monHP > totalAggro){ // do they survive?
					Script1.monHP = Script1.monHP - totalAggro; // if so, reduce damage from HP
				}
				else {
					experience += Script1.monExp; //give XP
					Script1.spinDying = true;
					killCount++; //ding
				}
			}
			//if (movement <= 0) {Script1.monTurn = true;}
		}
		spinEffect.renderer.enabled = true;
		spinMode = true;
		currentDir = player.transform.rotation;
		spinLim = 360;
		spinBtnPressed = false;

	}
}
