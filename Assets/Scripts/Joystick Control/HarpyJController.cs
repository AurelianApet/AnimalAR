using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarpyJController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject harpy;
    public int cnt = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Global.eagle_mode)
            return;
        if (ETCInput.GetAxisPressedRight("Horizontal"))
        {
            harpy.GetComponent<HarpyEagleCharacterScript>().controlMode = 1;
            harpy.GetComponent<HarpyEagleCharacterScript>().isGrounded = true;
            harpy.GetComponent<HarpyEagleCharacterScript>().soaring = true;
            harpy.GetComponent<HarpyEagleCharacterScript>().Soar();
            harpy.GetComponent<HarpyEagleCharacterScript>().yawVelocity = -ETCInput.GetAxisSpeed("Horizontal");
        }
        if (!ETCInput.GetAxisPressedLeft("Horizontal") && !ETCInput.GetAxisPressedRight("Horizontal"))
        {
            harpy.GetComponent<HarpyEagleCharacterScript>().yawVelocity = 0f;
        }

        if (ETCInput.GetAxisPressedDown("Vertical"))
        {
            harpy.GetComponent<HarpyEagleCharacterScript>().controlMode = 1;
            harpy.GetComponent<HarpyEagleCharacterScript>().isGrounded = true;
            harpy.GetComponent<HarpyEagleCharacterScript>().soaring = true;
            harpy.GetComponent<HarpyEagleCharacterScript>().Soar();
            harpy.GetComponent<HarpyEagleCharacterScript>().forwardAcceleration = ETCInput.GetAxisSpeed("Vertical");
        }
        if (!ETCInput.GetAxisPressedUp("Vertical") && !ETCInput.GetAxisPressedDown("Vertical"))
        {
            harpy.GetComponent<HarpyEagleCharacterScript>().forwardAcceleration = 0f;
        }

        if (ETCInput.GetAxisPressedLeft("Horizontal"))
        {
            harpy.GetComponent<HarpyEagleCharacterScript>().controlMode = 1;
            harpy.GetComponent<HarpyEagleCharacterScript>().isGrounded = true;
            harpy.GetComponent<HarpyEagleCharacterScript>().soaring = true;
            harpy.GetComponent<HarpyEagleCharacterScript>().Soar();
            harpy.GetComponent<HarpyEagleCharacterScript>().yawVelocity = -ETCInput.GetAxisSpeed("Horizontal");
        }
        if (!ETCInput.GetAxisPressedLeft("Horizontal") && !ETCInput.GetAxisPressedRight("Horizontal"))
        {
            harpy.GetComponent<HarpyEagleCharacterScript>().yawVelocity = 0f;
        }

        if (ETCInput.GetAxisPressedUp("Vertical"))
        {
            harpy.GetComponent<HarpyEagleCharacterScript>().controlMode = 1;
            harpy.GetComponent<HarpyEagleCharacterScript>().isGrounded = true;
            harpy.GetComponent<HarpyEagleCharacterScript>().soaring = true;
            harpy.GetComponent<HarpyEagleCharacterScript>().upDown = 0.5f;
            harpy.GetComponent<HarpyEagleCharacterScript>().Soar();
            harpy.GetComponent<HarpyEagleCharacterScript>().forwardAcceleration = ETCInput.GetAxisSpeed("Vertical");
        }
        if (!ETCInput.GetAxisPressedUp("Vertical") && !ETCInput.GetAxisPressedDown("Vertical"))
        {
            harpy.GetComponent<HarpyEagleCharacterScript>().forwardAcceleration = 0f;
        }
        if (Input.GetButtonDown("Fire1"))
        {
            harpy.GetComponent<HarpyEagleCharacterScript>().Attack();
        }
        if(!ETCInput.GetAxisPressedUp("Vertical") && !ETCInput.GetAxisPressedDown("Vertical") && !ETCInput.GetAxisPressedLeft("Horizontal") && !ETCInput.GetAxisPressedRight("Horizontal"))
        {
            harpy.GetComponent<HarpyEagleCharacterScript>().upDown = -5f;
            if (harpy.transform.position.y < 3f)
            {
                harpy.GetComponent<HarpyEagleCharacterScript>().Landing();
                harpy.GetComponent<HarpyEagleCharacterScript>().upDown = 0f;
            }
        }
        Debug.Log(harpy.transform.position.y);

    }
}
