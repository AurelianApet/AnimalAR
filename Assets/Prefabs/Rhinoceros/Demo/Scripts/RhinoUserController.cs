using UnityEngine;
using System.Collections;

public class RhinoUserController : MonoBehaviour {
	RhinoCharacter rhinoCharacter;
	
	void Start () {
		rhinoCharacter = GetComponent < RhinoCharacter> ();
	}
	
	void Update () {	
		if (Input.GetButtonDown ("Fire1")) {
			rhinoCharacter.Attack();
		}
		if (Input.GetButtonDown ("Jump")) {
			rhinoCharacter.Jump();
		}
		if (Input.GetKeyDown (KeyCode.H)) {
			rhinoCharacter.Hit();
		}
		if (Input.GetKeyDown (KeyCode.E)) {
			rhinoCharacter.EatStart();
		}
		if (Input.GetKeyUp (KeyCode.E)) {
			rhinoCharacter.EatEnd();
		}
		if (Input.GetKeyDown (KeyCode.G)) {
			rhinoCharacter.LegDig();
		}

		if (Input.GetKeyDown (KeyCode.K)) {
			rhinoCharacter.Death();
		}
		if (Input.GetKeyDown (KeyCode.L)) {
			rhinoCharacter.Rebirth();
		}		
		

		if (Input.GetKeyDown (KeyCode.G)) {
			rhinoCharacter.Gallop();
		}	
		if (Input.GetKeyUp (KeyCode.G)) {
			rhinoCharacter.Walk();
		}	

		if (Input.GetKeyDown (KeyCode.T)) {
			rhinoCharacter.Trot();
		}	
		if (Input.GetKeyUp (KeyCode.T)) {
			rhinoCharacter.Walk();
		}	




		rhinoCharacter.forwardSpeed=rhinoCharacter.walkMode*Input.GetAxis ("Vertical");
		rhinoCharacter.turnSpeed= Input.GetAxis ("Horizontal");
	}
}
