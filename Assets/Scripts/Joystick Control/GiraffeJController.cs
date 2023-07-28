using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiraffeJController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject giraffe;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Global.giraffe_mode)
            return;

        if (ETCInput.GetAxisPressedRight("Horizontal"))
        {
            giraffe.GetComponent<GiraffeCharacter>().turnSpeed = -ETCInput.GetAxisSpeed("Horizontal");
        }
        if (!ETCInput.GetAxisPressedLeft("Horizontal") && !ETCInput.GetAxisPressedRight("Horizontal"))
        {
            giraffe.GetComponent<GiraffeCharacter>().turnSpeed = 0f;
        }

        if (ETCInput.GetAxisPressedDown("Vertical"))
        {
            giraffe.GetComponent<GiraffeCharacter>().forwardSpeed = ETCInput.GetAxisSpeed("Vertical");
        }
        if (!ETCInput.GetAxisPressedUp("Vertical") && !ETCInput.GetAxisPressedDown("Vertical"))
        {
            giraffe.GetComponent<GiraffeCharacter>().forwardSpeed = 0f;
        }

        if (ETCInput.GetAxisPressedLeft("Horizontal"))
        {
            giraffe.GetComponent<GiraffeCharacter>().turnSpeed = -ETCInput.GetAxisSpeed("Horizontal");
        }
        if (!ETCInput.GetAxisPressedLeft("Horizontal") && !ETCInput.GetAxisPressedRight("Horizontal"))
        {
            giraffe.GetComponent<GiraffeCharacter>().turnSpeed = 0f;
        }

        if (ETCInput.GetAxisPressedUp("Vertical"))
        {
            giraffe.GetComponent<GiraffeCharacter>().forwardSpeed = ETCInput.GetAxisSpeed("Vertical");
        }
        if (!ETCInput.GetAxisPressedUp("Vertical") && !ETCInput.GetAxisPressedDown("Vertical"))
        {
            giraffe.GetComponent<GiraffeCharacter>().forwardSpeed = 0f;
        }
        if (Input.GetButtonDown("Fire1"))
        {
            giraffe.GetComponent<GiraffeCharacter>().Attack();
        }
    }
}
