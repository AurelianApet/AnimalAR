using UnityEngine;
using System.Collections;

public class HarpyEagleUserControllerScript : MonoBehaviour {
	public HarpyEagleCharacterScript harpyEagleCharacter;
	public float upDownInputSpeed=3f;
	
	void Start () {
		harpyEagleCharacter = GetComponent<HarpyEagleCharacterScript> ();
	}
	
	void Update(){
		if (Input.GetButtonDown ("Jump")) {
			harpyEagleCharacter.Soar ();
            Debug.Log("1");
		}
		
		if (Input.GetKeyDown (KeyCode.J)) {
			harpyEagleCharacter.Jump ();
            Debug.Log("2");
        }

        if (Input.GetKeyDown (KeyCode.H)) {
			harpyEagleCharacter.Hit ();
            Debug.Log("3");
        }
        if (Input.GetKeyDown (KeyCode.N)) {
			harpyEagleCharacter.SitDown ();
            Debug.Log("4");
        }

        if (Input.GetKeyDown (KeyCode.U)) {
			harpyEagleCharacter.StandUp ();
            Debug.Log("5");
        }

        if (Input.GetKeyDown (KeyCode.K)) {
			harpyEagleCharacter.Down ();
            Debug.Log("6");
        }

        if (Input.GetKeyDown (KeyCode.R)) {
			harpyEagleCharacter.Rebirth ();
            Debug.Log("7");
        }
        if (Input.GetKeyDown (KeyCode.G)) {
			harpyEagleCharacter.Grooming ();
            Debug.Log("8");
        }

        if (Input.GetButtonDown ("Fire2")) {
			harpyEagleCharacter.Attack2 ();
            Debug.Log("9");
        }

        if (Input.GetKeyDown (KeyCode.V)) {
			harpyEagleCharacter.Call ();
            Debug.Log("10");
        }

        if (Input.GetKeyDown (KeyCode.E)) {
			harpyEagleCharacter.Eat ();
            Debug.Log("11");
        }
        if (Input.GetKeyDown (KeyCode.C)) {
			harpyEagleCharacter.CrouchStart ();
            Debug.Log("12");
        }

        if (Input.GetKeyUp (KeyCode.C)) {
			harpyEagleCharacter.CrouchEnd ();
            Debug.Log("13");
        }

        if (Input.GetKeyDown (KeyCode.R)) {
			harpyEagleCharacter.RunStart ();
            Debug.Log("14");
        }

        if (Input.GetKeyUp (KeyCode.R)) {
			harpyEagleCharacter.RunEnd ();
            Debug.Log("15");
        }

        if (Input.GetButtonDown ("Fire1")) {
			harpyEagleCharacter.Attack ();
            Debug.Log("16");
        }
        if (Input.GetKey (KeyCode.N)) {
			harpyEagleCharacter.upDown=Mathf.Clamp(harpyEagleCharacter.upDown-Time.deltaTime*upDownInputSpeed,-1f,1f);
            Debug.Log("17");
        }
        if (Input.GetKey (KeyCode.U)) {
			harpyEagleCharacter.upDown=Mathf.Clamp(harpyEagleCharacter.upDown+Time.deltaTime*upDownInputSpeed,-1f,1f);
            Debug.Log("18");
        }
    }
	
	void FixedUpdate(){
		float v = Input.GetAxis ("Vertical");
		float h = Input.GetAxis ("Horizontal");	
		
		harpyEagleCharacter.forwardAcceleration = v;
		harpyEagleCharacter.yawVelocity = h;
	}
}

