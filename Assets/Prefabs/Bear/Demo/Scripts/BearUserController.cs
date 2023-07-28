using UnityEngine;
using System.Collections;

public class BearUserController : MonoBehaviour {
	BearCharacter bearCharacter;
	
	void Start () {
		bearCharacter = GetComponent < BearCharacter> ();
	}
	
	void Update () {
        if (Input.GetButtonDown("Fire1"))
        {
            bearCharacter.Attack();
        }
        if (Input.GetButtonDown("Jump"))
        {
            bearCharacter.Jump();
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            bearCharacter.Hit();
        }


        if (Input.GetKeyDown(KeyCode.K))
        {
            bearCharacter.Death();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            bearCharacter.Rebirth();
        }


        if (Input.GetKeyDown(KeyCode.U))
        {
            bearCharacter.StandUp();
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            bearCharacter.StandUpEnd();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            bearCharacter.Gallop();
        }
        if (Input.GetKeyUp(KeyCode.R))
        {
            bearCharacter.Walk();
        }

        bearCharacter.forwardSpeed=bearCharacter.walkMode*Input.GetAxis ("Vertical");
        bearCharacter.turnSpeed= Input.GetAxis ("Horizontal");
    }

}
