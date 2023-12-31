﻿using UnityEngine;
using System.Collections;

public class SharkCharacter : MonoBehaviour {
	Animator sharkAnimator;
	Rigidbody sharkRigid;
	
	public bool isLived=true;
	public float forwardAccerelation=0f;
	public float turnAccerelation=0f;
	public float upDownAccerelation=0f;
	public float rollAccerelation = 0f;
	public GameObject sharkPrefab;
	public GameObject sharkBody;

	void Start () {
		sharkAnimator = GetComponent<Animator> ();
		sharkRigid=GetComponent<Rigidbody>();
	}
	
	void FixedUpdate(){
        if (!this.gameObject.activeSelf)
            return;

        Move();
	}	
	
	public void Hit(){
		sharkAnimator.SetTrigger("Hit");
	}
	
	public void Bite(){
		sharkAnimator.SetTrigger("Bite");
	}		

	public void BiteRight(){
		sharkAnimator.SetTrigger("BiteRight");
	}	

	public void BiteLeft(){
		sharkAnimator.SetTrigger("BiteLeft");
	}	
	public void BiteDown(){
		sharkAnimator.SetTrigger("BiteDown");
	}	
	
	public void BiteUp(){
		sharkAnimator.SetTrigger("BiteUp");
	}	

	public void TwistBiteRight(){
		sharkAnimator.SetTrigger("TwistBiteRight");
	}	
	
	public void TwistBiteLeft(){
		sharkAnimator.SetTrigger("TwistBiteLeft");
	}	

	public void Death(){
		if (isLived) {
			sharkAnimator.SetTrigger ("Death");
			isLived = false;
			sharkBody= (GameObject)GameObject.Instantiate (sharkPrefab, transform.position, transform.rotation);
			SkinnedMeshRenderer[] skins = GetComponentsInChildren<SkinnedMeshRenderer> ();
			foreach (SkinnedMeshRenderer skin in skins) {
				skin.enabled = false;
			}

			CapsuleCollider[] capsels=GetComponentsInParent<CapsuleCollider>();
			foreach(CapsuleCollider capsel in capsels){
				capsel.enabled=false;
			}

			sharkRigid.constraints=RigidbodyConstraints.FreezeAll;

		}

	}
	
	public void Rebirth(){
		if(!isLived){
			sharkAnimator.SetTrigger("Rebirth");
			SkinnedMeshRenderer[] skins = GetComponentsInChildren<SkinnedMeshRenderer> ();
			foreach (SkinnedMeshRenderer skin in skins) {
				skin.enabled = true;
			}
			
			CapsuleCollider[] capsels=GetComponentsInParent<CapsuleCollider>();
			foreach(CapsuleCollider capsel in capsels){
				capsel.enabled=true;
			}
			Destroy(sharkBody);
			sharkRigid.constraints=RigidbodyConstraints.None;

			isLived = true;
		}
	}
	
	public void Move(){
		if (isLived) {
			sharkRigid.AddForce(transform.forward*forwardAccerelation*2000f);
            sharkRigid.AddTorque(-transform.right * upDownAccerelation * 500f);
            sharkRigid.AddTorque(transform.up * turnAccerelation * 300f);
            sharkRigid.AddTorque(transform.forward * rollAccerelation * 10f);
            sharkAnimator.SetFloat ("Forward", forwardAccerelation);
            sharkAnimator.SetFloat("Turn", turnAccerelation);
            sharkAnimator.SetFloat("UpDown", upDownAccerelation);
            sharkAnimator.SetFloat("Roll", rollAccerelation);
        }
	}
}
