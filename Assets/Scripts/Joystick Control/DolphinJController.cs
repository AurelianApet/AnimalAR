using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DolphinJController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject dolphin;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (ETCInput.GetAxisPressedRight("Horizontal"))
        {
            dolphin.GetComponent<DolphinCharacter>().turnSpeed = -ETCInput.GetAxisSpeed("Horizontal");
        }
        if (!ETCInput.GetAxisPressedLeft("Horizontal") && !ETCInput.GetAxisPressedRight("Horizontal"))
        {
            dolphin.GetComponent<DolphinCharacter>().turnSpeed = 0f;
        }

        if (ETCInput.GetAxisPressedDown("Vertical"))
        {
            dolphin.GetComponent<DolphinCharacter>().forwardSpeed = ETCInput.GetAxisSpeed("Vertical");
        }
        if (!ETCInput.GetAxisPressedUp("Vertical") && !ETCInput.GetAxisPressedDown("Vertical"))
        {
            dolphin.GetComponent<DolphinCharacter>().forwardSpeed = 0f;
        }

        if (ETCInput.GetAxisPressedLeft("Horizontal"))
        {
            dolphin.GetComponent<DolphinCharacter>().turnSpeed = -ETCInput.GetAxisSpeed("Horizontal");
        }
        if (!ETCInput.GetAxisPressedLeft("Horizontal") && !ETCInput.GetAxisPressedRight("Horizontal"))
        {
            dolphin.GetComponent<DolphinCharacter>().turnSpeed = 0f;
        }

        if (ETCInput.GetAxisPressedUp("Vertical"))
        {
            dolphin.GetComponent<DolphinCharacter>().forwardSpeed = ETCInput.GetAxisSpeed("Vertical");
        }
        if (!ETCInput.GetAxisPressedUp("Vertical") && !ETCInput.GetAxisPressedDown("Vertical"))
        {
            dolphin.GetComponent<DolphinCharacter>().forwardSpeed = 0f;
        }
        if (Input.GetButtonDown("Fire1"))
        {
            dolphin.GetComponent<DolphinCharacter>().Hit();
        }
    }
}
