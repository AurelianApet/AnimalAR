using UnityEngine;
using System.Collections;

public class GiraffeCharacter : MonoBehaviour {
	Animator giraffeAnimator;
	public bool jumpStart=false;
	public float groundCheckDistance = 0.6f;
	public float groundCheckOffset=0.01f;
	public bool isGrounded=true;
	public float jumpSpeed=1f;
	Rigidbody giraffeRigid;
	public float forwardSpeed;
	public float turnSpeed;
	public float jumpStartTime=0f;

	public bool isGallp=false;	
	public float maxWalkSpeed=1f;

	void Start () {
		giraffeAnimator = GetComponent<Animator> ();
		giraffeRigid=GetComponent<Rigidbody>();
	}
	
	void FixedUpdate(){
		CheckGroundStatus ();
		Move ();
		jumpStartTime+=Time.deltaTime;
		if (isGallp) {
			maxWalkSpeed=Mathf.Clamp(maxWalkSpeed+Time.deltaTime,1f,2f);
		} else {
			maxWalkSpeed=Mathf.Clamp(maxWalkSpeed-Time.deltaTime,1f,2f);
		}

	}
	
	public void Attack(){
		giraffeAnimator.SetTrigger("Attack");
	}
	
	public void Hit(){
		giraffeAnimator.SetTrigger("Hit");
	}

	public void Eat(){
		giraffeAnimator.SetTrigger("Eat");
	}

	public void Death(){
		giraffeAnimator.SetBool("IsLived",false);
	}
	
	public void Rebirth(){
		giraffeAnimator.SetBool("IsLived",true);
	}

	public void DrinkEnd(){
		giraffeAnimator.SetBool("IsDrinking",false);
	}
	
	public void DrinkStart(){
		giraffeAnimator.SetBool("IsDrinking",true);
	}



	
	public void Gallop(){
		isGallp = true;
	}
	
	public void Walk(){
		isGallp = false;
	}
	
	public void Jump(){
		if (isGrounded) {
			giraffeAnimator.SetTrigger ("Jump");
			jumpStart = true;
			jumpStartTime=0f;
			isGrounded=false;
			giraffeAnimator.SetBool("IsGrounded",false);
		}
	}
	
	void CheckGroundStatus()
	{
		RaycastHit hitInfo;
		isGrounded = Physics.Raycast (transform.position + (transform.up * groundCheckOffset), Vector3.down, out hitInfo, groundCheckDistance);
		
		if (jumpStart) {
			if(jumpStartTime>.25f){
				jumpStart=false;
				giraffeRigid.AddForce((transform.up+transform.forward*forwardSpeed)*jumpSpeed,ForceMode.Impulse);
				giraffeAnimator.applyRootMotion = false;
				giraffeAnimator.SetBool("IsGrounded",false);
			}
		}
		
		if (isGrounded && !jumpStart && jumpStartTime>.5f) {
			giraffeAnimator.applyRootMotion = true;
			giraffeAnimator.SetBool ("IsGrounded", true);
		} else {
			if(!jumpStart){
				giraffeAnimator.applyRootMotion = false;
				giraffeAnimator.SetBool ("IsGrounded", false);
			}
		}
	}
	
	public void Move(){
		giraffeAnimator.SetFloat ("Forward", forwardSpeed);
		giraffeAnimator.SetFloat ("Turn", turnSpeed);
	}
}
