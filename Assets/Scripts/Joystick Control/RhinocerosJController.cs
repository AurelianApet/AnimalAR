using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhinocerosJController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject rhinoceros;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Global.rhino_mode)
            return;

        if (ETCInput.GetAxisPressedRight("Horizontal"))
        {
            rhinoceros.GetComponent<RhinoCharacter>().turnSpeed = -ETCInput.GetAxisSpeed("Horizontal");
        }
        if (!ETCInput.GetAxisPressedLeft("Horizontal") && !ETCInput.GetAxisPressedRight("Horizontal"))
        {
            rhinoceros.GetComponent<RhinoCharacter>().turnSpeed = 0f;
        }

        if (ETCInput.GetAxisPressedDown("Vertical"))
        {
            rhinoceros.GetComponent<RhinoCharacter>().forwardSpeed = ETCInput.GetAxisSpeed("Vertical");
        }
        if (!ETCInput.GetAxisPressedUp("Vertical") && !ETCInput.GetAxisPressedDown("Vertical"))
        {
            rhinoceros.GetComponent<RhinoCharacter>().forwardSpeed = 0f;
        }

        if (ETCInput.GetAxisPressedLeft("Horizontal"))
        {
            rhinoceros.GetComponent<RhinoCharacter>().turnSpeed = -ETCInput.GetAxisSpeed("Horizontal");
        }
        if (!ETCInput.GetAxisPressedLeft("Horizontal") && !ETCInput.GetAxisPressedRight("Horizontal"))
        {
            rhinoceros.GetComponent<RhinoCharacter>().turnSpeed = 0f;
        }

        if (ETCInput.GetAxisPressedUp("Vertical"))
        {
            rhinoceros.GetComponent<RhinoCharacter>().forwardSpeed = ETCInput.GetAxisSpeed("Vertical");
        }
        if (!ETCInput.GetAxisPressedUp("Vertical") && !ETCInput.GetAxisPressedDown("Vertical"))
        {
            rhinoceros.GetComponent<RhinoCharacter>().forwardSpeed = 0f;
        }
        if (Input.GetButtonDown("Fire1"))
        {
            rhinoceros.GetComponent<RhinoCharacter>().Attack();
        }
    }
}
