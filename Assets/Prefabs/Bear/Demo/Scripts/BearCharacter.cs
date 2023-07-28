using UnityEngine;
using System.Collections;

public class BearCharacter : MonoBehaviour {
	Animator bearAnimator;
	public bool jumpStart=false;
	public float groundCheckDistance = 0.6f;
	public float groundCheckOffset=0.01f;
	public bool isGrounded=true;
	public float jumpSpeed=1f;
	Rigidbody bearRigid;
	public float forwardSpeed;
	public float turnSpeed;
	public float walkMode=1f;
	public float jumpStartTime=0f;
	
	void Start () {
		bearAnimator = GetComponent<Animator> ();
		bearRigid=GetComponent<Rigidbody>();
	}
	
	void FixedUpdate(){
        if (!this.gameObject.activeSelf)
            return;
		CheckGroundStatus ();
		Move ();
		jumpStartTime+=Time.deltaTime;
	}
	
	public void Attack(){
		bearAnimator.SetTrigger("Attack");
	}
	
	public void Hit(){
		bearAnimator.SetTrigger("Hit");
	}
	
	public void Death(){
		bearAnimator.SetBool("IsLived",false);
	}
	
	public void Rebirth(){
		bearAnimator.SetBool("IsLived",true);
	}

	public void StandUp(){
		bearAnimator.SetBool("IsStanding",true);
	}

	public void StandUpEnd(){
		bearAnimator.SetBool("IsStanding",false);
	}

	public void Gallop(){
		walkMode = 2f;
	}

	public void Walk(){
		walkMode = 1f;
	}

	public void Jump(){
		if (isGrounded) {
			bearAnimator.SetTrigger ("Jump");
			jumpStart = true;
			jumpStartTime=0f;
			isGrounded=false;
			bearAnimator.SetBool("IsGrounded",false);
		}
	}
	
	void CheckGroundStatus()
	{
		RaycastHit hitInfo;
		isGrounded = Physics.Raycast (transform.position + (transform.up * groundCheckOffset), Vector3.down, out hitInfo, groundCheckDistance);
		
		if (jumpStart) {
			if(jumpStartTime>.25f){
				jumpStart=false;
				bearRigid.AddForce((transform.up+transform.forward*forwardSpeed)*jumpSpeed,ForceMode.Impulse);
				bearAnimator.applyRootMotion = false;
				bearAnimator.SetBool("IsGrounded",false);
			}
		}
		
		if (isGrounded && !jumpStart && jumpStartTime>.5f) {
			bearAnimator.applyRootMotion = true;
			bearAnimator.SetBool ("IsGrounded", true);
		} else {
			if(!jumpStart){
				bearAnimator.applyRootMotion = false;
				bearAnimator.SetBool ("IsGrounded", false);
			}
		}
	}
	
	public void Move(){
		bearAnimator.SetFloat ("Forward", forwardSpeed);
		bearAnimator.SetFloat ("Turn", turnSpeed);
	}
}
