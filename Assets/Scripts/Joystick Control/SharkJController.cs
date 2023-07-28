using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkJController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject shark;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Global.shark_mode)
            return;

        if (ETCInput.GetAxisPressedRight("Horizontal"))
        {
            shark.GetComponent<SharkCharacter>().turnAccerelation = -ETCInput.GetAxisSpeed("Horizontal");
        }
        if (!ETCInput.GetAxisPressedLeft("Horizontal") && !ETCInput.GetAxisPressedRight("Horizontal"))
        {
            shark.GetComponent<SharkCharacter>().turnAccerelation = 0f;
        }

        if (ETCInput.GetAxisPressedDown("Vertical"))
        {
            shark.GetComponent<SharkCharacter>().forwardAccerelation = ETCInput.GetAxisSpeed("Vertical");
        }
        if (!ETCInput.GetAxisPressedUp("Vertical") && !ETCInput.GetAxisPressedDown("Vertical"))
        {
            shark.GetComponent<SharkCharacter>().forwardAccerelation = 0f;
        }

        if (ETCInput.GetAxisPressedLeft("Horizontal"))
        {
            shark.GetComponent<SharkCharacter>().turnAccerelation = -ETCInput.GetAxisSpeed("Horizontal");
        }
        if (!ETCInput.GetAxisPressedLeft("Horizontal") && !ETCInput.GetAxisPressedRight("Horizontal"))
        {
            shark.GetComponent<SharkCharacter>().turnAccerelation = 0f;
        }

        if (ETCInput.GetAxisPressedUp("Vertical"))
        {
            shark.GetComponent<SharkCharacter>().forwardAccerelation = ETCInput.GetAxisSpeed("Vertical");
        }
        if (!ETCInput.GetAxisPressedUp("Vertical") && !ETCInput.GetAxisPressedDown("Vertical"))
        {
            shark.GetComponent<SharkCharacter>().forwardAccerelation = 0f;
        }
        if (Input.GetButtonDown("Fire1"))
        {
            shark.GetComponent<SharkCharacter>().Bite();
        }
    }
}
