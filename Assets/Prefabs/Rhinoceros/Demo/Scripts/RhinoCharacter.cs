using UnityEngine;
using System.Collections;

public class RhinoCharacter : MonoBehaviour {
	Animator rhinoAnimator;
	public bool jumpStart=false;
	public float groundCheckDistance = 0.6f;
	public float groundCheckOffset=0.01f;
	public bool isGrounded=true;
	public float jumpSpeed=1f;
	Rigidbody rhinoRigid;
	public float forwardSpeed;
	public float turnSpeed;
	public float walkMode=1f;
	public float jumpStartTime=0f;

	void Start () {
		rhinoAnimator = GetComponent<Animator> ();
		rhinoRigid=GetComponent<Rigidbody>();
	}
	
	void FixedUpdate(){
		CheckGroundStatus ();
		Move ();
		jumpStartTime+=Time.deltaTime;
	}
	
	public void Attack(){
		rhinoAnimator.SetTrigger("Attack");
	}
	
	public void Hit(){
		rhinoAnimator.SetTrigger("Hit");
	}

	public void EatStart(){
		rhinoAnimator.SetBool("Eat",true);
	}

	public void EatEnd(){
		rhinoAnimator.SetBool("Eat",false);
	}

	public void LegDig(){
		rhinoAnimator.SetTrigger("LegDig");
	}

	public void Death(){
		rhinoAnimator.SetBool("IsLived",false);
	}
	
	public void Rebirth(){
		rhinoAnimator.SetBool("IsLived",true);
	}

	
	public void Gallop(){
		walkMode = 3f;
	}

	public void Trot(){
		walkMode = 2f;
	}

	public void Walk(){
		walkMode = 1f;
	}
	
	public void Jump(){
		if (isGrounded) {
			rhinoAnimator.SetTrigger ("Jump");
			jumpStart = true;
			jumpStartTime=0f;
			isGrounded=false;
			rhinoAnimator.SetBool("IsGrounded",false);
		}
	}
	
	void CheckGroundStatus()
	{
		RaycastHit hitInfo;
		isGrounded = Physics.Raycast (transform.position + (transform.up * groundCheckOffset), Vector3.down, out hitInfo, groundCheckDistance);
		
		if (jumpStart) {
			if(jumpStartTime>.25f){
				jumpStart=false;
				rhinoRigid.AddForce((transform.up+transform.forward*forwardSpeed)*jumpSpeed,ForceMode.Impulse);
				rhinoAnimator.applyRootMotion = false;
				rhinoAnimator.SetBool("IsGrounded",false);
			}
		}
		
		if (isGrounded && !jumpStart && jumpStartTime>.5f) {
			rhinoAnimator.applyRootMotion = true;
			rhinoAnimator.SetBool ("IsGrounded", true);
		} else {
			if(!jumpStart){
				rhinoAnimator.applyRootMotion = false;
				rhinoAnimator.SetBool ("IsGrounded", false);
			}
		}
	}
	
	public void Move(){
		rhinoAnimator.SetFloat ("Forward", forwardSpeed);
		rhinoAnimator.SetFloat ("Turn", turnSpeed);
	}
}
