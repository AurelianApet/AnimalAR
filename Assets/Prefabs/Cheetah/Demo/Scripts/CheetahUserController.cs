using UnityEngine;
using System.Collections;

public class CheetahUserController : MonoBehaviour
{
    CheetahCharacter cheetahCharacter;

    void Start()
    {
        cheetahCharacter = GetComponent<CheetahCharacter>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            cheetahCharacter.Attack();
        }
        if (Input.GetButtonDown("Jump"))
        {
            cheetahCharacter.Jump();
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            cheetahCharacter.Hit();
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            cheetahCharacter.Death();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            cheetahCharacter.Rebirth();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            cheetahCharacter.Roar();
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            cheetahCharacter.GallopFast();
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            cheetahCharacter.Gallop();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            cheetahCharacter.Canter();
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            cheetahCharacter.Trot();
        }
        if (Input.GetKeyUp(KeyCode.X))
        {
            cheetahCharacter.Walk();
        }

        cheetahCharacter.forwardSpeed = cheetahCharacter.maxWalkSpeed * Input.GetAxis("Vertical");
        cheetahCharacter.turnSpeed = Input.GetAxis("Horizontal");
    }

}
