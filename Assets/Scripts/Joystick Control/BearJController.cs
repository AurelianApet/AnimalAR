using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class BearJController : MonoBehaviour
{
    public GameObject animal;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Global.bear_mode)
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
            animal.GetComponent<BearCharacter>().turnSpeed = -ETCInput.GetAxisSpeed("Horizontal");
        }
        if (!ETCInput.GetAxisPressedLeft("Horizontal") && !ETCInput.GetAxisPressedRight("Horizontal"))
        {
            animal.GetComponent<BearCharacter>().turnSpeed = 0f;
        }

        if (ETCInput.GetAxisPressedDown("Vertical"))
        {
            animal.GetComponent<BearCharacter>().forwardSpeed = ETCInput.GetAxisSpeed("Vertical");
        }
        if (!ETCInput.GetAxisPressedUp("Vertical") && !ETCInput.GetAxisPressedDown("Vertical"))
        {
            animal.GetComponent<BearCharacter>().forwardSpeed = 0f;
        }

        if (ETCInput.GetAxisPressedLeft("Horizontal"))
        {
            animal.GetComponent<BearCharacter>().turnSpeed = -ETCInput.GetAxisSpeed("Horizontal");
        }
        if (!ETCInput.GetAxisPressedLeft("Horizontal") && !ETCInput.GetAxisPressedRight("Horizontal"))
        {
            animal.GetComponent<BearCharacter>().turnSpeed = 0f;
        }

        if (ETCInput.GetAxisPressedUp("Vertical"))
        {
            animal.GetComponent<BearCharacter>().forwardSpeed = ETCInput.GetAxisSpeed("Vertical");
        }
        if(!ETCInput.GetAxisPressedUp("Vertical") && !ETCInput.GetAxisPressedDown("Vertical"))
        {
            animal.GetComponent<BearCharacter>().forwardSpeed = 0f;
        }
        if (Input.GetButtonDown("Fire1"))
        {
            animal.GetComponent<BearCharacter>().Attack();
        }
    }
}
