using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LionJController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject lion;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Global.lion_mode)
            return;

        if (ETCInput.GetAxisPressedRight("Horizontal"))
        {
            lion.GetComponent<LionCharacter>().turnSpeed = -ETCInput.GetAxisSpeed("Horizontal");
        }
        if (!ETCInput.GetAxisPressedLeft("Horizontal") && !ETCInput.GetAxisPressedRight("Horizontal"))
        {
            lion.GetComponent<LionCharacter>().turnSpeed = 0f;
        }

        if (ETCInput.GetAxisPressedDown("Vertical"))
        {
            lion.GetComponent<LionCharacter>().forwardSpeed = -ETCInput.GetAxisSpeed("Vertical");
        }
        if (!ETCInput.GetAxisPressedUp("Vertical") && !ETCInput.GetAxisPressedDown("Vertical"))
        {
            lion.GetComponent<LionCharacter>().forwardSpeed = 0f;
        }

        if (ETCInput.GetAxisPressedLeft("Horizontal"))
        {
            lion.GetComponent<LionCharacter>().turnSpeed = -ETCInput.GetAxisSpeed("Horizontal");
        }
        if (!ETCInput.GetAxisPressedLeft("Horizontal") && !ETCInput.GetAxisPressedRight("Horizontal"))
        {
            lion.GetComponent<LionCharacter>().turnSpeed = 0f;
        }

        if (ETCInput.GetAxisPressedUp("Vertical"))
        {
            lion.GetComponent<LionCharacter>().forwardSpeed = ETCInput.GetAxisSpeed("Vertical");
        }
        if (!ETCInput.GetAxisPressedUp("Vertical") && !ETCInput.GetAxisPressedDown("Vertical"))
        {
            lion.GetComponent<LionCharacter>().forwardSpeed = 0f;
        }
        if (Input.GetButtonDown("Fire1"))
        {
            lion.GetComponent<LionCharacter>().Attack();
        }
    }
}
