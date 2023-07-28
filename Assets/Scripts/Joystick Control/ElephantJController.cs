using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElephantJController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject elephant;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Global.elepant_mode)
            return;

        if (ETCInput.GetAxisPressedRight("Horizontal"))
        {
            elephant.GetComponent<ElephantCharacter>().turnSpeed = -ETCInput.GetAxisSpeed("Horizontal");
        }
        if (!ETCInput.GetAxisPressedLeft("Horizontal") && !ETCInput.GetAxisPressedRight("Horizontal"))
        {
            elephant.GetComponent<ElephantCharacter>().turnSpeed = 0f;
        }

        if (ETCInput.GetAxisPressedDown("Vertical"))
        {
            elephant.GetComponent<ElephantCharacter>().forwardSpeed = ETCInput.GetAxisSpeed("Vertical");
        }
        if (!ETCInput.GetAxisPressedUp("Vertical") && !ETCInput.GetAxisPressedDown("Vertical"))
        {
            elephant.GetComponent<ElephantCharacter>().forwardSpeed = 0f;
        }

        if (ETCInput.GetAxisPressedLeft("Horizontal"))
        {
            elephant.GetComponent<ElephantCharacter>().turnSpeed = -ETCInput.GetAxisSpeed("Horizontal");
        }
        if (!ETCInput.GetAxisPressedLeft("Horizontal") && !ETCInput.GetAxisPressedRight("Horizontal"))
        {
            elephant.GetComponent<ElephantCharacter>().turnSpeed = 0f;
        }

        if (ETCInput.GetAxisPressedUp("Vertical"))
        {
            elephant.GetComponent<ElephantCharacter>().forwardSpeed = ETCInput.GetAxisSpeed("Vertical");
        }
        if (!ETCInput.GetAxisPressedUp("Vertical") && !ETCInput.GetAxisPressedDown("Vertical"))
        {
            elephant.GetComponent<ElephantCharacter>().forwardSpeed = 0f;
        }
        if (Input.GetButtonDown("Fire1"))
        {
            elephant.GetComponent<ElephantCharacter>().Attack();
        }
    }
}
