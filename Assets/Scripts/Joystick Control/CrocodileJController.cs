using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrocodileJController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject croc;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Global.crocodile_mode)
            return;

        //if (ETCInput.GetAxisDownRight("Horizontal"))
        //{
        //    animal.GetComponent<BearCharacter>().turnSpeed = 1.0f;
        //}

        //if (ETCInput.GetAxisDownDown("Vertical"))
        //{
        //    animal.GetComponent<BearCharacter>().forwardSpeed = -1.0f;
        //}

        //if (ETCInput.GetAxisDownLeft("Horizontal"))
        //{
        //    animal.GetComponent<BearCharacter>().turnSpeed = -1.0f;
        //}

        //if (ETCInput.GetAxisDownUp("Vertical"))
        //{
        //    animal.GetComponent<BearCharacter>().forwardSpeed = 1.0f;
        //}



        if (ETCInput.GetAxisPressedRight("Horizontal"))
        {
            croc.GetComponent<CrocodileCharacter>().turnSpeed = -ETCInput.GetAxisSpeed("Horizontal");
        }
        if (!ETCInput.GetAxisPressedLeft("Horizontal") && !ETCInput.GetAxisPressedRight("Horizontal"))
        {
            croc.GetComponent<CrocodileCharacter>().turnSpeed = 0f;
        }

        if (ETCInput.GetAxisPressedDown("Vertical"))
        {
            croc.GetComponent<CrocodileCharacter>().forwardSpeed = ETCInput.GetAxisSpeed("Vertical");
        }
        if (!ETCInput.GetAxisPressedUp("Vertical") && !ETCInput.GetAxisPressedDown("Vertical"))
        {
            croc.GetComponent<CrocodileCharacter>().forwardSpeed = 0f;
        }

        if (ETCInput.GetAxisPressedLeft("Horizontal"))
        {
            croc.GetComponent<CrocodileCharacter>().turnSpeed = -ETCInput.GetAxisSpeed("Horizontal");
        }
        if (!ETCInput.GetAxisPressedLeft("Horizontal") && !ETCInput.GetAxisPressedRight("Horizontal"))
        {
            croc.GetComponent<CrocodileCharacter>().turnSpeed = 0f;
        }

        if (ETCInput.GetAxisPressedUp("Vertical"))
        {
            croc.GetComponent<CrocodileCharacter>().forwardSpeed = ETCInput.GetAxisSpeed("Vertical");
        }
        if (!ETCInput.GetAxisPressedUp("Vertical") && !ETCInput.GetAxisPressedDown("Vertical"))
        {
            croc.GetComponent<CrocodileCharacter>().forwardSpeed = 0f;
        }
        if (Input.GetButtonDown("Fire1"))
        {
            croc.GetComponent<CrocodileCharacter>().Attack();
        }
    }
}
