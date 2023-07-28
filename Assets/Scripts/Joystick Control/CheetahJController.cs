using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheetahJController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject cheetah;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Global.cheetah_mode)
            return;

        if (ETCInput.GetAxisPressedRight("Horizontal"))
        {
            cheetah.GetComponent<CheetahCharacter>().turnSpeed = -ETCInput.GetAxisSpeed("Horizontal");
        }
        if (!ETCInput.GetAxisPressedLeft("Horizontal") && !ETCInput.GetAxisPressedRight("Horizontal"))
        {
            cheetah.GetComponent<CheetahCharacter>().turnSpeed = 0f;
        }

        if (ETCInput.GetAxisPressedDown("Vertical"))
        {
            cheetah.GetComponent<CheetahCharacter>().forwardSpeed = ETCInput.GetAxisSpeed("Vertical");
        }
        if (!ETCInput.GetAxisPressedUp("Vertical") && !ETCInput.GetAxisPressedDown("Vertical"))
        {
            cheetah.GetComponent<CheetahCharacter>().forwardSpeed = 0f;
        }

        if (ETCInput.GetAxisPressedLeft("Horizontal"))
        {
            cheetah.GetComponent<CheetahCharacter>().turnSpeed = -ETCInput.GetAxisSpeed("Horizontal");
        }
        if (!ETCInput.GetAxisPressedLeft("Horizontal") && !ETCInput.GetAxisPressedRight("Horizontal"))
        {
            cheetah.GetComponent<CheetahCharacter>().turnSpeed = 0f;
        }

        if (ETCInput.GetAxisPressedUp("Vertical"))
        {
            cheetah.GetComponent<CheetahCharacter>().forwardSpeed = ETCInput.GetAxisSpeed("Vertical") * 2.0f;
        }
        if (!ETCInput.GetAxisPressedUp("Vertical") && !ETCInput.GetAxisPressedDown("Vertical"))
        {
            cheetah.GetComponent<CheetahCharacter>().forwardSpeed = 0f;
        }
        if (Input.GetButtonDown("Fire1"))
        {
            cheetah.GetComponent<CheetahCharacter>().Attack();
        }
    }
}
