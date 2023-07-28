using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcaJController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject orca;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (ETCInput.GetAxisPressedRight("Horizontal"))
        {
            orca.GetComponent<OrcaCharacter>().turnSpeed = -ETCInput.GetAxisSpeed("Horizontal")/2f;
        }
        if (!ETCInput.GetAxisPressedLeft("Horizontal") && !ETCInput.GetAxisPressedRight("Horizontal"))
        {
            orca.GetComponent<OrcaCharacter>().turnSpeed = 0f;
        }

        if (ETCInput.GetAxisPressedDown("Vertical"))
        {
            orca.GetComponent<OrcaCharacter>().forwardSpeed = ETCInput.GetAxisSpeed("Vertical")/2f;
        }
        if (!ETCInput.GetAxisPressedUp("Vertical") && !ETCInput.GetAxisPressedDown("Vertical"))
        {
            orca.GetComponent<OrcaCharacter>().forwardSpeed = 0f;
        }

        if (ETCInput.GetAxisPressedLeft("Horizontal"))
        {
            orca.GetComponent<OrcaCharacter>().turnSpeed = -ETCInput.GetAxisSpeed("Horizontal")/2f;
        }
        if (!ETCInput.GetAxisPressedLeft("Horizontal") && !ETCInput.GetAxisPressedRight("Horizontal"))
        {
            orca.GetComponent<OrcaCharacter>().turnSpeed = 0f;
        }

        if (ETCInput.GetAxisPressedUp("Vertical"))
        {
            orca.GetComponent<OrcaCharacter>().forwardSpeed = ETCInput.GetAxisSpeed("Vertical")/2f;
        }
        if (!ETCInput.GetAxisPressedUp("Vertical") && !ETCInput.GetAxisPressedDown("Vertical"))
        {
            orca.GetComponent<OrcaCharacter>().forwardSpeed = 0f;
        }
        if (Input.GetButtonDown("Fire1"))
        {
            orca.GetComponent<OrcaCharacter>().Hit();
        }
    }
}
