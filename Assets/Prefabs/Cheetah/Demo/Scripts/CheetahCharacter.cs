using UnityEngine;
using System.Collections;

public class CheetahCharacter : MonoBehaviour
{
    Animator cheetahAnimator;
    public bool jumpStart = false;
    public float groundCheckDistance = 0.6f;
    public float groundCheckOffset = 0.01f;
    public bool isGrounded = true;
    public float jumpSpeed = 1f;
    Rigidbody cheetahRigid;
    public float forwardSpeed;
    public float turnSpeed;
    public float walkMode = 1f;
    public float jumpStartTime = 0f;
    public float maxWalkSpeed = 1f;

    void Start()
    {
        cheetahAnimator = GetComponent<Animator>();
        cheetahRigid = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (!this.gameObject.activeSelf)
            return;

        CheckGroundStatus();
        Move();
        jumpStartTime += Time.deltaTime;
        maxWalkSpeed = Mathf.Lerp(maxWalkSpeed, walkMode, Time.deltaTime);
    }

    public void Attack()
    {
        cheetahAnimator.SetTrigger("Attack");
    }

    public void Hit()
    {
        cheetahAnimator.SetTrigger("Hit");
    }

    public void Death()
    {
        cheetahAnimator.SetBool("IsLived", false);
    }

    public void Rebirth()
    {
        cheetahAnimator.SetBool("IsLived", true);
    }
        
    public void Roar()
    {
        cheetahAnimator.SetTrigger("Roar");
    }

    public void GallopFast()
    {
        walkMode = 6f;
    }


    public void Gallop()
    {
        walkMode = 4f;
    }

    public void Canter()
    {
        walkMode = 3f;
    }

    public void Trot()
    {
        walkMode = 2f;
    }

    public void Walk()
    {
        walkMode = 1f;
    }

    public void Jump()
    {
        if (isGrounded)
        {
            cheetahAnimator.SetBool("JumpStart", true);
            cheetahRigid.AddForce((transform.up + transform.forward * forwardSpeed*0.5f) * jumpSpeed, ForceMode.Impulse);
            cheetahAnimator.applyRootMotion = false;
            jumpStart = true;
            jumpStartTime = 0f;
            isGrounded = false;
            cheetahAnimator.SetBool("IsGrounded", false);
        }
    }

    void CheckGroundStatus()
    {
        RaycastHit hitInfo;
        isGrounded = Physics.Raycast(transform.position + (transform.up * groundCheckOffset), Vector3.down, out hitInfo, groundCheckDistance);

        if (jumpStart)
        {
            if (jumpStartTime > .25f)
            {
                jumpStart = false;
                cheetahAnimator.SetBool("IsGrounded", false);
                cheetahAnimator.SetBool("JumpStart", false);
            }
        }

        if (isGrounded && !jumpStart && jumpStartTime > .5f)
        {
            cheetahAnimator.applyRootMotion = true;
            cheetahAnimator.SetBool("IsGrounded", true);
        }
        else
        {
            if (!jumpStart)
            {
                cheetahAnimator.applyRootMotion = false;
                cheetahAnimator.SetBool("IsGrounded", false);
            }
        }
    }

    public void Move()
    {
        cheetahAnimator.SetFloat("Forward", forwardSpeed);
        cheetahAnimator.SetFloat("Turn", turnSpeed);
    }
}
