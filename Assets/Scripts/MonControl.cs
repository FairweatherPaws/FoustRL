using UnityEngine;
using System.Collections;

public class MonControl : MonoBehaviour {

	public int monX, monZ, monPow, monEnd, monSpe, monHP, oldX, oldZ, dLX, dLZ, monMove, monExp;
	public Transform monType;
	private GoTweenChain chain;
	public bool monTurn = false;
	public bool obstacle = false;
	private GameObject gc, player, enemy;

	// Use this for initialization
	void Start () {

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
			while (monMove > 0){
				// call gamecontroller script to determine direction to player
				monMove--;
				obstacle = false;
				gc = GameObject.FindGameObjectWithTag("GameController");
				GCScript Script1 = gc.GetComponent<GCScript>();
				if (Script1.pLocX > monX) {dLX = 1;}
				if (Script1.pLocX == monX) {dLX = 0;}
				if (Script1.pLocX < monX) {dLX = -1;}
				if (Script1.pLocZ > monZ) {dLZ = 1;}
				if (Script1.pLocZ == monZ) {dLZ = 0;}
				if (Script1.pLocZ < monZ) {dLZ = -1;}
				if (monX + dLX == Script1.pLocX && monZ + dLZ == Script1.pLocZ) 
				{
					dLX = 0;
					dLZ = 0;
					Script1.playHP = Script1.playHP - monPow;
					if (Script1.playHP <= 0) {Script1.gameOver = true;}
					obstacle = true;
				}
				// make sure no enemies are in the way
				foreach(GameObject enemy in GameObject.FindGameObjectsWithTag("foe"))
				{
					
					MonControl Script2 = enemy.GetComponent<MonControl>();
					if (Script2.monX == monX+dLX && Script2.monZ == monZ+dLZ){obstacle = true;}
					
				}
				// move
				if (obstacle) {}
				else {
					monX = monX + dLX;
					monZ = monZ + dLZ;
					GoTween monMoveTween = new GoTween(this.transform, 0.2f, new GoTweenConfig().position (new Vector3( dLX * 5, 0, dLZ * 5 ), true));
					chain = new GoTweenChain();
					chain.append(monMoveTween);
					chain.play();
				}
				if (monMove <= 0) {monTurn = false;}
			}

		}
	
	}
}
