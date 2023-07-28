using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class penguine : MonoBehaviour, ITrackableEventHandler
{
    #region PUBLIC_MEMBER_VARIABLES
    public GameObject animal;
    public GameObject ice;
    public GameObject background;
    public GameObject col;

    #endregion

    #region PROTECTED_MEMBER_VARIABLES

    protected TrackableBehaviour mTrackableBehaviour;
    protected TrackableBehaviour.Status m_PreviousStatus;
    protected TrackableBehaviour.Status m_NewStatus;

    #endregion // PROTECTED_MEMBER_VARIABLES

    Transform ipos;
    Transform bpos;
    int init = 0;
    int dive = 0;
    int start = 0;


    #region UNITY_MONOBEHAVIOUR_METHODS

    protected virtual void Start()
    {
        animal.transform.position = animal.transform.parent.position;

        var arot = animal.transform.parent.rotation;
        arot.y = arot.y + 90;
        animal.transform.rotation = arot;

        Global.penguine_mode = true;

        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
        {
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
        }

        ipos = ice.transform;
        bpos = background.transform;

        //OnTrackingFound();
    }

    protected virtual void OnDestroy()
    {
        if (mTrackableBehaviour)
        {
            mTrackableBehaviour.UnregisterTrackableEventHandler(this);
        }
    }

    #endregion // UNITY_MONOBEHAVIOUR_METHODS

    #region PUBLIC_METHODS

    /// <summary>
    ///     Implementation of the ITrackableEventHandler function called when the
    ///     tracking state changes.
    /// </summary>
    public void OnTrackableStateChanged(
        TrackableBehaviour.Status previousStatus,
        TrackableBehaviour.Status newStatus)
    {
        m_PreviousStatus = previousStatus;
        m_NewStatus = newStatus;

        if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED ||
            newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            //Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " found , Status : " + newStatus.ToString());
            if (newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
                OnTrackingFoundExtended();
            else
                OnTrackingFound();
        }
        else if (previousStatus == TrackableBehaviour.Status.TRACKED &&
                 newStatus == TrackableBehaviour.Status.NO_POSE)
        {
            //Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " lost");
            OnTrackingLost();
        }
        else
        {
            // For combo of previousStatus=UNKNOWN + newStatus=UNKNOWN|NOT_FOUND
            // Vuforia is starting, but tracking has not been lost or found yet
            // Call OnTrackingLost() to hide the augmentations
            //Debug.LogError("Trackable NoLost,NoFound " + mTrackableBehaviour.TrackableName);
            OnTrackingLost();
        }
    }

    #endregion // PUBLIC_METHODS

    #region PROTECTED_METHODS

    protected virtual void OnTrackingFound()
    {
        ice.SetActive(true);
        background.SetActive(true);
        //col.SetActive(true);
        StartCoroutine(turn_penguin());
    }

    IEnumerator turn_penguin()
    {
        yield return new WaitForSeconds(1.0f);
        if (animal.activeSelf && ice.activeSelf && background.activeSelf)
        {
            animal.GetComponent<PenguinCharacter>().isTobogganing = true;
            yield return new WaitForSeconds(1.0f);
            animal.GetComponent<PenguinCharacter>().tobogganSpeed = 1f;
            StartCoroutine(stop_turn());
        }
        yield return null;
    }
    IEnumerator stop_turn()
    {
        while (true)
        {
            if (!animal.activeSelf || !ice.activeSelf || !background.activeSelf)
            {
                break;
            }
            Vector3 down = animal.transform.TransformDirection(Vector3.down);
            Ray ray1 = new Ray(animal.transform.position, Vector3.down);
            RaycastHit hit;
            if (Physics.Raycast(animal.transform.position, down, out hit))
            {
                if (hit.collider.gameObject.tag == "water")
                {
                    hit.collider.gameObject.SetActive(false);
                    animal.GetComponent<PenguinCharacter>().tobogganSpeed = 0f;
                    animal.GetComponent<PenguinCharacter>().isTobogganing = false;
                    StartCoroutine(dive_penguin());
                    break;
                }
            }
            yield return new WaitForSeconds(0.01f);
        }
    }
    IEnumerator dive_penguin()
    {
        if (animal.activeSelf && ice.activeSelf && background.activeSelf)
        {
            Debug.Log("dive");
            while (true)
            {
                if (init == 1)
                {
                    break;
                }
                animal.GetComponent<PenguinCharacter>().SwimStart();
                animal.GetComponent<PenguinCharacter>().forwardSpeed = -0.5f;
                Debug.Log(animal.transform.position.y);
                if (animal.transform.position.y <= -0.7f)
                {
                    Debug.Log("swim start");
                    animal.GetComponent<PenguinCharacter>().forwardSpeed = 0f;
                    StartCoroutine(swim_penguin());
                    break;
                }
                yield return new WaitForSeconds(0.01f);
            }
        }
    }
    IEnumerator swim_penguin()
    {
        if (animal.activeSelf && ice.activeSelf && background.activeSelf)
        {
            Debug.Log("swim");
            animal.GetComponent<PenguinCharacter>().swimSpeed = 1f;
            animal.GetComponent<PenguinCharacter>().yawVelocity = 1f;
        }
        yield return null;
    }
    protected virtual void OnTrackingFoundExtended()
    {
        Debug.Log("OnTrackingFoundExtended");
    }


    protected virtual void OnTrackingLost()
    {
        ice.SetActive(false);
        background.SetActive(false);
        //col.SetActive(false);
    }

    public void Initialize()
    {
        Debug.Log(animal.GetComponent<PenguinCharacter>().swimSpeed);
        animal.GetComponent<PenguinCharacter>().isTobogganing = false;
        animal.GetComponent<PenguinCharacter>().SwimEnd();
        //animal.GetComponent<PenguinCharacter>().StandUp();
        animal.GetComponent<PenguinCharacter>().forwardSpeed = 0f;
        animal.GetComponent<PenguinCharacter>().tobogganSpeed = 1f;
        animal.GetComponent<PenguinCharacter>().swimSpeed = 3f;
        animal.GetComponent<PenguinCharacter>().yawVelocity = 0f;
        //col.SetActive(true);
        animal.transform.position = animal.transform.parent.position;
        var rot = animal.transform.parent.rotation;
        rot.y += 90;
        animal.transform.rotation = rot;
        ice.transform.position = ipos.position;
        background.transform.position = bpos.position;
    }

    void Update()
    {
        if ((!animal.activeSelf || (!ice.activeSelf && !background.activeSelf) && init == 0) && Global.penguine_mode)
        {
            Initialize();
            init = 1;
            Global.penguine_mode = false;
            StopAllCoroutines();
        }
        if (animal.activeSelf && ice.activeSelf && background.activeSelf && init == 1)
        {
            init = 0;
            Global.penguine_mode = true;
            StartCoroutine(turn_penguin());
        }
    }

    #endregion // PROTECTED_METHODS

}