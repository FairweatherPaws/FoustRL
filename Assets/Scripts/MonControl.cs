using UnityEngine;
using System.Collections;

public class MonControl : MonoBehaviour {

	public int monX, monZ, monPow, monEnd, monSpe, monHP, oldX, oldZ, dLX, dLZ, monMove, monExp, qad;
	public Transform monType;
	private GoTweenChain chain;
	private float ticker;
	public bool monTurn = false;
	public bool obstacle = false;
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
	}
	
	// Update is called once per frame
	void Update () {
		if (monTurn)
		{
			ticker = 0.2f;
			monMove = monSpe;
			dLX = 0;
			dLZ = 0;
			chain = new GoTweenChain();
			while (monMove > 0){
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
					if (monX + dLX == Script1.pLocX && monZ + dLZ == Script1.pLocZ) 
					{
						dLX = 0;
						dLZ = 0;
						qad = monPow - Script1.endurance;
						if (qad > 0) { Script1.playHP = Script1.playHP - qad; }
						if (Script1.playHP <= 0) {Script1.gameOver = true; Script1.HPNum.GetComponent<TextMesh>().text = Script1.playHP.ToString();}
						obstacle = true;

					}
					// make sure no enemies are in the way
					foreach(GameObject enemy in GameObject.FindGameObjectsWithTag("foe"))
					{
						
						MonControl Script2 = enemy.GetComponent<MonControl>();
						if (Script2.monX == this.monX+dLX && Script2.monZ == this.monZ+dLZ){dLX = 0; dLZ = 0; ticker = 0.2f;}

					}

						monX = monX + dLX;
						monZ = monZ + dLZ;
						GoTween monMoveTween = new GoTween(this.transform, 0.2f, new GoTweenConfig().position (new Vector3( dLX * 5, 0, dLZ * 5 ), true));
						
						chain.append(monMoveTween);

					monMove--;
				}
			}
			monTurn = false;
			chain.play();
		}

	}

}
