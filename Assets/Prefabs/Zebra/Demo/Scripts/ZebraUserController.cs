using UnityEngine;
using System.Collections;

public class ZebraUserController : MonoBehaviour {
	ZebraCharacter zebraCharacter;
	
	void Start () {
		zebraCharacter = GetComponent <ZebraCharacter> ();
	}
	
	void Update () {	
		if (Input.GetButtonDown ("Fire1")) {
			zebraCharacter.Attack();
		}
		if (Input.GetButtonDown ("Jump")) {
			zebraCharacter.Jump();
		}
		if (Input.GetKeyDown (KeyCode.H)) {
			zebraCharacter.Hit();
		}
		if (Input.GetKeyDown (KeyCode.K)) {
			zebraCharacter.Death();
		}
		if (Input.GetKeyDown (KeyCode.L)) {
			zebraCharacter.Rebirth();
		}		
		
		if (Input.GetKeyDown (KeyCode.E)) {
			zebraCharacter.EatStart();
		}	
		if (Input.GetKeyUp (KeyCode.E)) {
			zebraCharacter.EatEnd();
		}	
		
		if (Input.GetKeyDown (KeyCode.J)) {
			zebraCharacter.SitDown();
		}	
		if (Input.GetKeyDown (KeyCode.N)) {
			zebraCharacter.Sleep();
		}	
		if (Input.GetKeyDown (KeyCode.U)) {
			zebraCharacter.StandUp();
		}	
		
		if (Input.GetKeyDown (KeyCode.G)) {
			zebraCharacter.Gallop();
		}	
		if (Input.GetKeyDown (KeyCode.C)) {
			zebraCharacter.Canter();
		}	
		if (Input.GetKeyDown (KeyCode.T)) {
			zebraCharacter.Trot();
		}	
		if (Input.GetKeyUp (KeyCode.X)) {
			zebraCharacter.Walk();
		}	
		
		zebraCharacter.forwardSpeed=zebraCharacter.maxWalkSpeed*Input.GetAxis ("Vertical");
		zebraCharacter.turnSpeed= Input.GetAxis ("Horizontal");
	}
}
