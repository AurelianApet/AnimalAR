using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GorillaJController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject gorilla;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Global.gorilla_mode)
            return;

        if (ETCInput.GetAxisPressedRight("Horizontal"))
        {
            gorilla.GetComponent<GorillaCharacter>().turnSpeed = -ETCInput.GetAxisSpeed("Horizontal");
        }
        if (!ETCInput.GetAxisPressedLeft("Horizontal") && !ETCInput.GetAxisPressedRight("Horizontal"))
        {
            gorilla.GetComponent<GorillaCharacter>().turnSpeed = 0f;
        }

        if (ETCInput.GetAxisPressedDown("Vertical"))
        {
            gorilla.GetComponent<GorillaCharacter>().forwardSpeed = ETCInput.GetAxisSpeed("Vertical");
        }
        if (!ETCInput.GetAxisPressedUp("Vertical") && !ETCInput.GetAxisPressedDown("Vertical"))
        {
            gorilla.GetComponent<GorillaCharacter>().forwardSpeed = 0f;
        }

        if (ETCInput.GetAxisPressedLeft("Horizontal"))
        {
            gorilla.GetComponent<GorillaCharacter>().turnSpeed = -ETCInput.GetAxisSpeed("Horizontal");
        }
        if (!ETCInput.GetAxisPressedLeft("Horizontal") && !ETCInput.GetAxisPressedRight("Horizontal"))
        {
            gorilla.GetComponent<GorillaCharacter>().turnSpeed = 0f;
        }

        if (ETCInput.GetAxisPressedUp("Vertical"))
        {
            gorilla.GetComponent<GorillaCharacter>().forwardSpeed = ETCInput.GetAxisSpeed("Vertical");
        }
        if (!ETCInput.GetAxisPressedUp("Vertical") && !ETCInput.GetAxisPressedDown("Vertical"))
        {
            gorilla.GetComponent<GorillaCharacter>().forwardSpeed = 0f;
        }
        if (Input.GetButtonDown("Fire1"))
        {
            gorilla.GetComponent<GorillaCharacter>().Attack();
        }
    }
}
