using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfJController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject wolf;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Global.wolf_mode)
            return;

        if (ETCInput.GetAxisPressedRight("Horizontal"))
        {
            wolf.GetComponent<WolfCharacter>().turnSpeed = -ETCInput.GetAxisSpeed("Horizontal");
        }
        if (!ETCInput.GetAxisPressedLeft("Horizontal") && !ETCInput.GetAxisPressedRight("Horizontal"))
        {
            wolf.GetComponent<WolfCharacter>().turnSpeed = 0f;
        }

        if (ETCInput.GetAxisPressedDown("Vertical"))
        {
            wolf.GetComponent<WolfCharacter>().forwardSpeed = ETCInput.GetAxisSpeed("Vertical");
        }
        if (!ETCInput.GetAxisPressedUp("Vertical") && !ETCInput.GetAxisPressedDown("Vertical"))
        {
            wolf.GetComponent<WolfCharacter>().forwardSpeed = 0f;
        }

        if (ETCInput.GetAxisPressedLeft("Horizontal"))
        {
            wolf.GetComponent<WolfCharacter>().turnSpeed = -ETCInput.GetAxisSpeed("Horizontal");
        }
        if (!ETCInput.GetAxisPressedLeft("Horizontal") && !ETCInput.GetAxisPressedRight("Horizontal"))
        {
            wolf.GetComponent<WolfCharacter>().turnSpeed = 0f;
        }

        if (ETCInput.GetAxisPressedUp("Vertical"))
        {
            wolf.GetComponent<WolfCharacter>().forwardSpeed = ETCInput.GetAxisSpeed("Vertical") * 2.0f;
        }
        if (!ETCInput.GetAxisPressedUp("Vertical") && !ETCInput.GetAxisPressedDown("Vertical"))
        {
            wolf.GetComponent<WolfCharacter>().forwardSpeed = 0f;
        }
        if (Input.GetButtonDown("Fire1"))
        {
            wolf.GetComponent<WolfCharacter>().Attack();
        }
    }
}
