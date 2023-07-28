using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZebraJController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject zebra;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Global.zebra_mode)
            return;

        if (ETCInput.GetAxisPressedRight("Horizontal"))
        {
            zebra.GetComponent<ZebraCharacter>().turnSpeed = -ETCInput.GetAxisSpeed("Horizontal");
        }
        if (!ETCInput.GetAxisPressedLeft("Horizontal") && !ETCInput.GetAxisPressedRight("Horizontal"))
        {
            zebra.GetComponent<ZebraCharacter>().turnSpeed = 0f;
        }

        if (ETCInput.GetAxisPressedDown("Vertical"))
        {
            zebra.GetComponent<ZebraCharacter>().forwardSpeed = ETCInput.GetAxisSpeed("Vertical");
        }
        if (!ETCInput.GetAxisPressedUp("Vertical") && !ETCInput.GetAxisPressedDown("Vertical"))
        {
            zebra.GetComponent<ZebraCharacter>().forwardSpeed = 0f;
        }

        if (ETCInput.GetAxisPressedLeft("Horizontal"))
        {
            zebra.GetComponent<ZebraCharacter>().turnSpeed = -ETCInput.GetAxisSpeed("Horizontal");
        }
        if (!ETCInput.GetAxisPressedLeft("Horizontal") && !ETCInput.GetAxisPressedRight("Horizontal"))
        {
            zebra.GetComponent<ZebraCharacter>().turnSpeed = 0f;
        }

        if (ETCInput.GetAxisPressedUp("Vertical"))
        {
            zebra.GetComponent<ZebraCharacter>().forwardSpeed = ETCInput.GetAxisSpeed("Vertical") * 2.0f;
        }
        if (!ETCInput.GetAxisPressedUp("Vertical") && !ETCInput.GetAxisPressedDown("Vertical"))
        {
            zebra.GetComponent<ZebraCharacter>().forwardSpeed = 0f;
        }
        if (Input.GetButtonDown("Fire1"))
        {
            zebra.GetComponent<ZebraCharacter>().Attack();
        }
    }
}
