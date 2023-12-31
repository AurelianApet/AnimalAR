﻿using UnityEngine;
using System.Collections;

public class AfricanSpurredTortoiseCharacter : MonoBehaviour {
	Animator africanSpurredTortoiseAnimator;

    public float forwardSpeed = 0.0f;
    public float turnSpeed = 0.0f;
	void Start () {
		africanSpurredTortoiseAnimator = GetComponent<Animator> ();
	}
	void Update()
    {
        Move();
    }
	public void Attack(){
		africanSpurredTortoiseAnimator.SetTrigger("Attack");
	}

	public void Hit(){
		africanSpurredTortoiseAnimator.SetTrigger("Hit");
	}

	public void Eat(){
		africanSpurredTortoiseAnimator.SetTrigger("Eat");
	}

	public void SitDown(){
		africanSpurredTortoiseAnimator.SetBool("SitDown",true);
	}
	
	public void StandUp(){
		africanSpurredTortoiseAnimator.SetBool("SitDown",false);
	}

	public void Death1(){
		africanSpurredTortoiseAnimator.SetTrigger("Death1");
	}

	public void Death2(){
		africanSpurredTortoiseAnimator.SetTrigger("Death2");
	}

	public void Death3(){
		africanSpurredTortoiseAnimator.SetTrigger("Death3");
	}

	public void Rebirth(){
		africanSpurredTortoiseAnimator.SetTrigger("Rebirth");
	}

	public void Move(){
		africanSpurredTortoiseAnimator.SetFloat ("Forward", forwardSpeed);
		africanSpurredTortoiseAnimator.SetFloat ("Turn", turnSpeed);
	}
}
