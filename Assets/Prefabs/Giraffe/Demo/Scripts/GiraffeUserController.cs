using UnityEngine;
using System.Collections;

public class GiraffeUserController : MonoBehaviour {
	GiraffeCharacter giraffeCharacter;
	
	void Start () {
		giraffeCharacter = GetComponent < GiraffeCharacter> ();
	}
	
	void Update () {	
		if (Input.GetButtonDown ("Fire1")) {
			giraffeCharacter.Attack();
		}
		if (Input.GetButtonDown ("Jump")) {
			giraffeCharacter.Jump();
		}
		if (Input.GetKeyDown (KeyCode.H)) {
			giraffeCharacter.Hit();
		}
		if (Input.GetKeyDown (KeyCode.E)) {
			giraffeCharacter.Eat();
		}		
		
		if (Input.GetKeyDown (KeyCode.K)) {
			giraffeCharacter.Death();
		}
		if (Input.GetKeyDown (KeyCode.L)) {
			giraffeCharacter.Rebirth();
		}		
		
		
		if (Input.GetKeyDown (KeyCode.U)) {
			giraffeCharacter.DrinkStart();
		}		
		if (Input.GetKeyDown (KeyCode.J)) {
			giraffeCharacter.DrinkEnd();
		}	
		
		if (Input.GetKeyDown (KeyCode.R)) {
			giraffeCharacter.Gallop();
		}	
		if (Input.GetKeyUp (KeyCode.R)) {
			giraffeCharacter.Walk();
		}	
		
		giraffeCharacter.forwardSpeed=giraffeCharacter.maxWalkSpeed*Input.GetAxis ("Vertical");
		giraffeCharacter.turnSpeed= Input.GetAxis ("Horizontal");
	}

}
