using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenguinJController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject penguin;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Global.penguine_mode)
            return;
        //penguin.GetComponent<PenguinCharacter>().isTobogganing = false;
        //penguin.GetComponent<PenguinCharacter>().SwimEnd();
        //penguin.GetComponent<PenguinCharacter>().isSwimming = false;
        penguin.GetComponent<PenguinCharacter>().StandUp();
        if (ETCInput.GetAxisPressedRight("Horizontal"))
        {
            penguin.GetComponent<PenguinCharacter>().yawVelocity = -ETCInput.GetAxisSpeed("Horizontal");
        }
        if (!ETCInput.GetAxisPressedLeft("Horizontal") && !ETCInput.GetAxisPressedRight("Horizontal"))
        {
            penguin.GetComponent<PenguinCharacter>().yawVelocity = 0f;
        }

        if (ETCInput.GetAxisPressedDown("Vertical"))
        {
            penguin.GetComponent<PenguinCharacter>().forwardSpeed = ETCInput.GetAxisSpeed("Vertical");
        }
        if (!ETCInput.GetAxisPressedUp("Vertical") && !ETCInput.GetAxisPressedDown("Vertical"))
        {
            penguin.GetComponent<PenguinCharacter>().forwardSpeed = 0f;
        }

        if (ETCInput.GetAxisPressedLeft("Horizontal"))
        {
            penguin.GetComponent<PenguinCharacter>().yawVelocity = -ETCInput.GetAxisSpeed("Horizontal");
        }
        if (!ETCInput.GetAxisPressedLeft("Horizontal") && !ETCInput.GetAxisPressedRight("Horizontal"))
        {
            penguin.GetComponent<PenguinCharacter>().yawVelocity = 0f;
        }

        if (ETCInput.GetAxisPressedUp("Vertical"))
        {
            penguin.GetComponent<PenguinCharacter>().forwardSpeed = ETCInput.GetAxisSpeed("Vertical");
        }
        if (!ETCInput.GetAxisPressedUp("Vertical") && !ETCInput.GetAxisPressedDown("Vertical"))
        {
            penguin.GetComponent<PenguinCharacter>().forwardSpeed = 0f;
        }
        if (Input.GetButtonDown("Fire1"))
        {
            penguin.GetComponent<PenguinCharacter>().Attack();
        }
    }
}
