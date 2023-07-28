using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenJControll : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject cock;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Global.chicken_mode)
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
            cock.GetComponent<ChickenCharacter>().yawVelocity = -ETCInput.GetAxisSpeed("Horizontal");
        }
        if (!ETCInput.GetAxisPressedLeft("Horizontal") && !ETCInput.GetAxisPressedRight("Horizontal"))
        {
            cock.GetComponent<ChickenCharacter>().yawVelocity = 0f;
        }

        if (ETCInput.GetAxisPressedDown("Vertical"))
        {
            cock.GetComponent<ChickenCharacter>().forwardAcceleration = ETCInput.GetAxisSpeed("Vertical");
        }
        if (!ETCInput.GetAxisPressedUp("Vertical") && !ETCInput.GetAxisPressedDown("Vertical"))
        {
            cock.GetComponent<ChickenCharacter>().forwardAcceleration = 0f;
        }

        if (ETCInput.GetAxisPressedLeft("Horizontal"))
        {
            cock.GetComponent<ChickenCharacter>().yawVelocity = -ETCInput.GetAxisSpeed("Horizontal");
        }
        if (!ETCInput.GetAxisPressedLeft("Horizontal") && !ETCInput.GetAxisPressedRight("Horizontal"))
        {
            cock.GetComponent<ChickenCharacter>().yawVelocity = 0f;
        }

        if (ETCInput.GetAxisPressedUp("Vertical"))
        {
            cock.GetComponent<ChickenCharacter>().forwardAcceleration = ETCInput.GetAxisSpeed("Vertical");
        }
        if (!ETCInput.GetAxisPressedUp("Vertical") && !ETCInput.GetAxisPressedDown("Vertical"))
        {
        cock.GetComponent<ChickenCharacter>().forwardAcceleration = 0f;
        }
        if (Input.GetButtonDown("Fire1"))
        {
            cock.GetComponent<ChickenCharacter>().Attack();
        }
    }
}
