using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class crocodile : MonoBehaviour, ITrackableEventHandler
{
    #region PUBLIC_MEMBER_VARIABLES
    public GameObject croc;
    public GameObject water;
    public GameObject cube;
    public GameObject col;

    int init = 0;
    #endregion

    #region PROTECTED_MEMBER_VARIABLES

    protected TrackableBehaviour mTrackableBehaviour;
    protected TrackableBehaviour.Status m_PreviousStatus;
    protected TrackableBehaviour.Status m_NewStatus;

    #endregion // PROTECTED_MEMBER_VARIABLES

    #region UNITY_MONOBEHAVIOUR_METHODS

    protected virtual void Start()
    {
        croc.transform.position = croc.transform.parent.position;
        var crot = croc.transform.parent.rotation;
        crot.y = crot.y + 90f;
        croc.transform.rotation = crot;
        Global.crocodile_mode = true;

        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
        {
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
        }
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
        water.SetActive(true);
        cube.SetActive(true);
        col.SetActive(true);
        StartCoroutine(croc_turn());
    }

    IEnumerator croc_turn()
    {
        yield return new WaitForSeconds(1.0f);
        if (croc.activeSelf && water.activeSelf)
        {
            Vector3 relativePos = water.transform.position - croc.transform.position;
            Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
            float temp = -1.0f;
            if (rotation.w < 0)
                temp = 1.0f;
            croc.GetComponent<CrocodileCharacter>().turnSpeed = temp;
            StartCoroutine(stop_turn());
        }
        yield return null;
    }
    IEnumerator stop_turn()
    {
        while (true)
        {
            Vector3 fwd = croc.transform.TransformDirection(Vector3.forward);
            float distance = Vector3.Distance(croc.transform.position, water.transform.position);
            Ray ray1 = new Ray(croc.transform.position + new Vector3(0, 6f, 0), croc.transform.forward);
            RaycastHit hit;
            if (Physics.Raycast(croc.transform.position + new Vector3(0, 6f, 0), fwd, out hit, distance * 2.0f))
            {
                if (hit.collider.gameObject.tag == "waterroll")
                {
                    croc.GetComponent<CrocodileCharacter>().turnSpeed = 0f;
                    croc.GetComponent<CrocodileCharacter>().forwardSpeed = 1.5f;
                    StartCoroutine(stop_go());
                    break;
                }
            }
            yield return new WaitForSeconds(0.01f);
        }
    }
    IEnumerator stop_go()
    {
        while (true)
        {
            float dist = Vector3.Distance(croc.transform.position, water.transform.position);
            Debug.Log(dist);
            if (dist < 7f)
            {
                croc.GetComponent<CrocodileCharacter>().forwardSpeed = 0f;
                StartCoroutine(croc_dive());
                break;
            }
            yield return new WaitForSeconds(0.01f);
        }
    }
    IEnumerator croc_dive()
    {
        if (croc.activeSelf)
        {
            croc.GetComponent<CrocodileCharacter>().SwimStart();
            croc.GetComponent<CrocodileCharacter>().upDownAccerelation = -10f;
            croc.GetComponent<CrocodileCharacter>().forwardAccerelation = 10f;
            croc.GetComponent<CrocodileCharacter>().forwardSpeed = 2f;
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(croc_swim());
        }
        yield return null;
    }
    IEnumerator croc_swim()
    {
        croc.GetComponent<CrocodileCharacter>().upDownAccerelation = 5f;
        croc.GetComponent<CrocodileCharacter>().forwardAccerelation = 10f;
        yield return new WaitForSeconds(1f);
        croc.GetComponent<CrocodileCharacter>().upDownAccerelation = 0f;
        yield return new WaitForSeconds(8.0f);
        croc.GetComponent<CrocodileCharacter>().turnAccerelation = -3f;
    }
    protected virtual void OnTrackingFoundExtended()
    {
        Debug.Log("OnTrackingFoundExtended");
    }


    protected virtual void OnTrackingLost()
    {
        water.SetActive(false);
        cube.SetActive(false);
        col.SetActive(false);
    }
    public void Initialize()
    {
        water.transform.localPosition = water.transform.parent.localPosition + new Vector3(5.52f, -0.04f, 1.04f);
        cube.transform.localPosition = cube.transform.parent.localPosition + new Vector3(25.47f, -5.11f, -25.67f);
        col.transform.localPosition = col.transform.parent.localPosition + new Vector3(5.92f, 0f, 0.773f);
        croc.GetComponent<CrocodileCharacter>().forwardSpeed = 0f;
        croc.GetComponent<CrocodileCharacter>().turnSpeed = 0f;
        croc.GetComponent<CrocodileCharacter>().upDownAccerelation = 0f;
        croc.GetComponent<CrocodileCharacter>().forwardAccerelation = 0f;
        croc.GetComponent<CrocodileCharacter>().turnAccerelation = 0f;
        croc.transform.position = croc.transform.parent.position;
        var crot = croc.transform.parent.rotation;
        crot.y = crot.y + 90f;
        croc.transform.rotation = crot;
    }
    void Update()
    {
        if ((!croc.activeSelf || !water.activeSelf) && Global.crocodile_mode)
        {
            Initialize();
            init = 1;
            Global.crocodile_mode = false;
        }
        if (croc.activeSelf && water.activeSelf && init == 1)
        {
            init = 0;
            Global.crocodile_mode = true;
            StartCoroutine(croc_turn());
        }
        if (croc.activeSelf && water.activeSelf)
        {
            if (croc.transform.parent.position.z > water.transform.parent.position.z)
            {
                water.transform.localPosition = water.transform.parent.localPosition + new Vector3(5.52f, -0.04f, 1.04f);
                cube.transform.localPosition = cube.transform.parent.localPosition + new Vector3(25.47f, -5.11f, -25.67f);
                col.transform.localPosition = col.transform.parent.localPosition + new Vector3(5.92f, 0f, 0.773f);
            }
            if (croc.transform.parent.position.z < water.transform.parent.position.z)
            {
                water.transform.localPosition = water.transform.parent.localPosition + new Vector3(5.52f, -0.04f, 1.04f);
                cube.transform.localPosition = cube.transform.parent.localPosition + new Vector3(25.47f, -5.11f, -25.67f);
                col.transform.localPosition = col.transform.parent.localPosition + new Vector3(5.92f, 0f, 0.773f);
            }
        }
    }
    #endregion // PROTECTED_METHODS

}
