using UnityEngine;
using System.Collections;

public class ElephantCharacter : MonoBehaviour {
	Animator elephantAnimator;
	public bool jumpStart=false;
	public float groundCheckDistance = 0.6f;
	public float groundCheckOffset=0.01f;
	public bool isGrounded=true;
	public float jumpSpeed=1f;
	Rigidbody elephantRigid;
	public float forwardSpeed;
	public float turnSpeed;
	public float walkMode=1f;
	public float jumpStartTime=0f;
	
	void Start () {
		elephantAnimator = GetComponent<Animator> ();
		elephantRigid=GetComponent<Rigidbody>();
	}
	
	void FixedUpdate(){
		CheckGroundStatus ();
		Move ();
		jumpStartTime+=Time.deltaTime;
	}
	
	public void Attack(){
		elephantAnimator.SetTrigger("Attack");
	}
	
	public void Hit(){
		elephantAnimator.SetTrigger("Hit");
	}
	
	public void Death(){
		elephantAnimator.SetBool("IsLived",false);
	}
	
	public void Rebirth(){
		elephantAnimator.SetBool("IsLived",true);
	}
	
	public void Eat(){
		elephantAnimator.SetTrigger("Eat");
	}
	
	public void Trot(){
		walkMode = 2f;
	}
	
	public void Walk(){
		walkMode = 1f;
	}
	
	public void Jump(){
		if (isGrounded) {
			elephantAnimator.SetTrigger ("Jump");
			jumpStart = true;
			jumpStartTime=0f;
			isGrounded=false;
			elephantAnimator.SetBool("IsGrounded",false);
		}
	}
	
	void CheckGroundStatus()
	{
		RaycastHit hitInfo;
		isGrounded = Physics.Raycast (transform.position + (transform.up * groundCheckOffset), Vector3.down, out hitInfo, groundCheckDistance);
		
		if (jumpStart) {
			if(jumpStartTime>.25f){
				jumpStart=false;
				elephantRigid.AddForce((transform.up+transform.forward*forwardSpeed)*jumpSpeed,ForceMode.Impulse);
				elephantAnimator.applyRootMotion = false;
				elephantAnimator.SetBool("IsGrounded",false);
			}
		}
		
		if (isGrounded && !jumpStart && jumpStartTime>.5f) {
			elephantAnimator.applyRootMotion = true;
			elephantAnimator.SetBool ("IsGrounded", true);
		} else {
			if(!jumpStart){
				elephantAnimator.applyRootMotion = false;
				elephantAnimator.SetBool ("IsGrounded", false);
			}
		}
	}
	
	public void Move(){
		elephantAnimator.SetFloat ("Forward", forwardSpeed);
		elephantAnimator.SetFloat ("Turn", turnSpeed);
	}
}
