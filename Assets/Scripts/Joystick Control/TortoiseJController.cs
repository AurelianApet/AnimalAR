using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TortoiseJController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject tortoise;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (ETCInput.GetAxisPressedRight("Horizontal"))
        {
            tortoise.GetComponent<AfricanSpurredTortoiseCharacter>().turnSpeed = -ETCInput.GetAxisSpeed("Horizontal");
        }
        if (!ETCInput.GetAxisPressedLeft("Horizontal") && !ETCInput.GetAxisPressedRight("Horizontal"))
        {
            tortoise.GetComponent<AfricanSpurredTortoiseCharacter>().turnSpeed = 0f;
        }

        if (ETCInput.GetAxisPressedDown("Vertical"))
        {
            tortoise.GetComponent<AfricanSpurredTortoiseCharacter>().forwardSpeed = ETCInput.GetAxisSpeed("Vertical");
        }
        if (!ETCInput.GetAxisPressedUp("Vertical") && !ETCInput.GetAxisPressedDown("Vertical"))
        {
            tortoise.GetComponent<AfricanSpurredTortoiseCharacter>().forwardSpeed = 0f;
        }

        if (ETCInput.GetAxisPressedLeft("Horizontal"))
        {
            tortoise.GetComponent<AfricanSpurredTortoiseCharacter>().turnSpeed = -ETCInput.GetAxisSpeed("Horizontal");
        }
        if (!ETCInput.GetAxisPressedLeft("Horizontal") && !ETCInput.GetAxisPressedRight("Horizontal"))
        {
            tortoise.GetComponent<AfricanSpurredTortoiseCharacter>().turnSpeed = 0f;
        }

        if (ETCInput.GetAxisPressedUp("Vertical"))
        {
            tortoise.GetComponent<AfricanSpurredTortoiseCharacter>().forwardSpeed = ETCInput.GetAxisSpeed("Vertical") * 2.0f;
        }
        if (!ETCInput.GetAxisPressedUp("Vertical") && !ETCInput.GetAxisPressedDown("Vertical"))
        {
            tortoise.GetComponent<AfricanSpurredTortoiseCharacter>().forwardSpeed = 0f;
        }
        if (Input.GetButtonDown("Fire1"))
        {
            tortoise.GetComponent<AfricanSpurredTortoiseCharacter>().Attack();
        }
    }
}
