using UnityEngine;
using System.Collections;

public class WolfUserController : MonoBehaviour
{
    WolfCharacter wolfCharacter;

    void Start()
    {
        wolfCharacter = GetComponent<WolfCharacter>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            wolfCharacter.Attack();
        }
        if (Input.GetButtonDown("Jump"))
        {
            wolfCharacter.Jump();
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            wolfCharacter.Hit();
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            wolfCharacter.Death();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            wolfCharacter.Rebirth();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            wolfCharacter.Roar();
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            wolfCharacter.Gallop();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            wolfCharacter.Canter();
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            wolfCharacter.Trot();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            wolfCharacter.Walk();
        }

        wolfCharacter.forwardSpeed = wolfCharacter.maxWalkSpeed * Input.GetAxis("Vertical");
        wolfCharacter.turnSpeed = Input.GetAxis("Horizontal");
    }

}
