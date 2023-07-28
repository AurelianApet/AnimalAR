using UnityEngine;
using System.Collections;

public class ElephantUserController : MonoBehaviour {
	ElephantCharacter elephantCharacter;
	
	void Start () {
		elephantCharacter = GetComponent <ElephantCharacter> ();
	}
	
	void Update () {	
		if (Input.GetButtonDown ("Fire1")) {
			elephantCharacter.Attack();
		}
		if (Input.GetButtonDown ("Jump")) {
			elephantCharacter.Jump();
		}
		if (Input.GetKeyDown (KeyCode.H)) {
			elephantCharacter.Hit();
		}
		
		if (Input.GetKeyDown (KeyCode.K)) {
			elephantCharacter.Death();
		}
		if (Input.GetKeyDown (KeyCode.L)) {
			elephantCharacter.Rebirth();
		}		
	
		if (Input.GetKeyDown (KeyCode.E)) {
			elephantCharacter.Eat();
		}		
		
		if (Input.GetKeyDown (KeyCode.T)) {
			elephantCharacter.Trot();
		}	
		if (Input.GetKeyUp (KeyCode.T)) {
			elephantCharacter.Walk();
		}	
		
		elephantCharacter.forwardSpeed=elephantCharacter.walkMode*Input.GetAxis ("Vertical");
		elephantCharacter.turnSpeed= Input.GetAxis ("Horizontal");
	}
}
