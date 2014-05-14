using UnityEngine;
using System.Collections;

public class MonControl : MonoBehaviour {

	public int monX, monZ, monPow, monEnd, monSpe, monHP, oldX, oldZ, dLX, dLZ, monMove, monExp, qad;
	public Transform monType, damageEffect;
	private GoTweenChain chain;
	private float ticker, rotAm, rotOrig;
	public bool monTurn = false;
	public bool obstacle = false;
	public bool runOnce = false;
	private GameObject gc, player, enemy;

	// Use this for initialization
	void Awake () {

		gc = GameObject.FindGameObjectWithTag("GameController");
		GCScript Script1 = gc.GetComponent<GCScript>();
		monX = Script1.newMonX;
		monZ = Script1.newMonZ;
		monType = Script1.newMonType;
		MonStats Script2 = this.GetComponent<MonStats>();
		monPow = Script2.monPow;
		monEnd = Script2.monEnd;
		monSpe = Script2.monSpe;
		monHP = Script2.monHP;
		monExp = Script2.expYield;
		monMove = monSpe;
		rotAm = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (monTurn)
		{

			if (runOnce) {
				monMove = monSpe; 
				runOnce = false; 
				chain = new GoTweenChain(); 
				ticker = 0.1f; 
				rotOrig = transform.rotation.eulerAngles.y;
			}
			dLX = 0;
			dLZ = 0;

			if (monMove > 0){
				if (ticker > 0) {ticker -= Time.deltaTime;}
				else {
					dLX = 0;
					dLZ = 0;



					obstacle = false;
					gc = GameObject.FindGameObjectWithTag("GameController"); // call gamecontroller script to determine direction to player
					GCScript Script1 = gc.GetComponent<GCScript>();
					if (Script1.pLocX > monX) {dLX += 1;}
					if (Script1.pLocX == monX) {dLX = 0;}
					if (Script1.pLocX < monX) {dLX -= 1;}
					if (Script1.pLocZ > monZ) {dLZ += 1;}
					if (Script1.pLocZ == monZ) {dLZ = 0;}
					if (Script1.pLocZ < monZ) {dLZ -= 1;}
					if (dLX == 0 && dLZ < 0) {rotAm = 0;}
					if (dLX < 0 && dLZ < 0) {rotAm = 45;}
					if (dLX < 0 && dLZ == 0) {rotAm = 90;}
					if (dLX < 0 && dLZ > 0) {rotAm = 135;}
					if (dLX == 0 && dLZ > 0) {rotAm = 180;}
					if (dLX > 0 && dLZ > 0) {rotAm = 225;}
					if (dLX > 0 && dLZ == 0) {rotAm = 270;}
					if (dLX > 0 && dLZ < 0) {rotAm = 315;}
					//transform.rotation = Quaternion.Euler(0,rotAm,0);
					float rotDelta = rotAm - rotOrig;
					rotOrig = rotAm;
					if (rotDelta < -180) {rotDelta += 360;}
					if (rotDelta > 180) {rotDelta -= 360;}
					if (monX + dLX == Script1.pLocX && monZ + dLZ == Script1.pLocZ) 
					{
						dLX = 0;
						dLZ = 0;
						qad = Random.Range (monPow, monPow * 2) - Script1.endurance;
						if (qad > 0) { Script1.playHP = Script1.playHP - qad; }
						if (Script1.playHP <= 0) {Script1.gameOver = true; Script1.HPNum.GetComponent<TextMesh>().text = Script1.playHP.ToString();}
						obstacle = true;
						if (qad > 0)
						{
							Transform damEff = Instantiate(damageEffect, new Vector3(
								(this.transform.position.x + Script1.player.transform.position.x) / 2 * 0.95f, 5f, 
								(this.transform.position.z + Script1.player.transform.position.z) / 2 * 0.95f), 
							            Quaternion.Euler(90, 30, 0)) as Transform;
							damEff.gameObject.GetComponent<TextMesh>().text = qad.ToString();

						}

					}
					// make sure no enemies are in the way
					foreach(GameObject enemy in GameObject.FindGameObjectsWithTag("foe"))
					{
						
						MonControl Script2 = enemy.GetComponent<MonControl>();
						if (Script2.monX == this.monX+dLX && Script2.monZ == this.monZ+dLZ){dLX = 0; dLZ = 0; ticker = 0.2f;}

					}

					monX = monX + dLX;
					monZ = monZ + dLZ;
					GoTween monMoveTween = new GoTween(this.transform, 0.3f/monSpe, new GoTweenConfig().position (new Vector3( dLX * 5, 0, dLZ * 5 ), true));
					GoTween monRotTween = new GoTween(this.transform, 0.15f/monSpe, new GoTweenConfig().eulerAngles (new Vector3( 0, rotDelta, 0 ), true));
					chain.append(monRotTween);
					chain.append(monMoveTween);

					monMove--;
					ticker = 0.2f;
				}
			}
			else {chain.play();
				monTurn = false;}

		}

	}

}
