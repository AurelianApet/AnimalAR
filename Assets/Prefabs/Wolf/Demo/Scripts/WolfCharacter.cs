using UnityEngine;
using System.Collections;

public class WolfCharacter : MonoBehaviour
{
    Animator wolfAnimator;
    public bool jumpStart = false;
    public float groundCheckDistance = 0.6f;
    public float groundCheckOffset = 0.01f;
    public bool isGrounded = true;
    public float jumpSpeed = 1f;
    Rigidbody wolfRigid;
    public float forwardSpeed;
    public float turnSpeed;
    public float walkMode = 1f;
    public float jumpStartTime = 0f;
    public float maxWalkSpeed = 1f;

    void Start()
    {
        wolfAnimator = GetComponent<Animator>();
        wolfRigid = GetComponent<Rigidbody>();
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
        wolfAnimator.SetTrigger("Attack");
    }

    public void Hit()
    {
        wolfAnimator.SetTrigger("Hit");
    }

    public void Death()
    {
        wolfAnimator.SetBool("IsLived", false);
    }

    public void Rebirth()
    {
        wolfAnimator.SetBool("IsLived", true);
    }


    public void Roar()
    {
        wolfAnimator.SetTrigger("Roar");
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
            wolfAnimator.SetTrigger("Jump");
            jumpStart = true;
            jumpStartTime = 0f;
            isGrounded = false;
            wolfAnimator.SetBool("IsGrounded", false);
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
                wolfRigid.AddForce((transform.up + transform.forward * forwardSpeed) * jumpSpeed, ForceMode.Impulse);
                wolfAnimator.applyRootMotion = false;
                wolfAnimator.SetBool("IsGrounded", false);
            }
        }

        if (isGrounded && !jumpStart && jumpStartTime > .5f)
        {
            wolfAnimator.applyRootMotion = true;
            wolfAnimator.SetBool("IsGrounded", true);
        }
        else
        {
            if (!jumpStart)
            {
                wolfAnimator.applyRootMotion = false;
                wolfAnimator.SetBool("IsGrounded", false);
            }
        }
    }

    public void Move()
    {
        wolfAnimator.SetFloat("Forward", forwardSpeed);
        wolfAnimator.SetFloat("Turn", turnSpeed);
    }
}
