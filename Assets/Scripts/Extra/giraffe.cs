using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class giraffe : MonoBehaviour, ITrackableEventHandler
{
    #region PUBLIC_MEMBER_VARIABLES
    public GameObject gira;
    public GameObject tree;

    #endregion

    #region PROTECTED_MEMBER_VARIABLES

    protected TrackableBehaviour mTrackableBehaviour;
    protected TrackableBehaviour.Status m_PreviousStatus;
    protected TrackableBehaviour.Status m_NewStatus;

    #endregion // PROTECTED_MEMBER_VARIABLES

    #region UNITY_MONOBEHAVIOUR_METHODS

    int init = 0;

    protected virtual void Start()
    {
        gira.transform.position = gira.transform.parent.position;
        var grot = gira.transform.parent.rotation;
        grot.y = grot.y + 90;
        gira.transform.rotation = grot;

        Global.giraffe_mode = true;

        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
        {
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
        }
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
        tree.SetActive(true);
        StartCoroutine(giraffe_turn());
    }
    IEnumerator giraffe_turn()
    {
        yield return new WaitForSeconds(1.0f);
        if (gira.activeSelf && tree.activeSelf)
        {
            Vector3 relativePos = tree.transform.position - gira.transform.position;
            Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
            float temp = -1.0f;
            if (rotation.w < 0)
                temp = 1.0f;
            gira.GetComponent<GiraffeCharacter>().turnSpeed = temp;
            StartCoroutine(stop_turn());
        }
        yield return null;
    }
    IEnumerator stop_turn()
    {
        while (true)
        {
            Vector3 fwd = gira.transform.TransformDirection(Vector3.forward);
            float distance = Vector3.Distance(gira.transform.position, tree.transform.position);
            Ray ray1 = new Ray(gira.transform.position + new Vector3(0, 0.3f, 0), gira.transform.forward);
            RaycastHit hit;
            if (Physics.Raycast(gira.transform.position, fwd, out hit, distance * 2.0f))
            {
                if (hit.collider.gameObject.tag == "Tree")
                {
                    gira.GetComponent<GiraffeCharacter>().turnSpeed = 0f;
                    gira.GetComponent<GiraffeCharacter>().forwardSpeed = 1.5f;
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
            float dist = Vector3.Distance(gira.transform.position, tree.transform.position);

            if (dist < 1f)
            {
                gira.GetComponent<GiraffeCharacter>().forwardSpeed = 0f;
                StartCoroutine(start_eat());
                break;
            }
            yield return new WaitForSeconds(0.01f);
        }
    }
    IEnumerator start_eat()
    {
        if (gira.activeSelf == true && tree.activeSelf == true)
        {
            while (true)
            {
                gira.GetComponent<GiraffeCharacter>().Eat();
                if (init == 1)
                    break;
                yield return new WaitForSeconds(0.01f);
            }
        }
    }
    protected virtual void OnTrackingFoundExtended()
    {
        Debug.Log("OnTrackingFoundExtended");
    }


    protected virtual void OnTrackingLost()
    {
        tree.SetActive(false);
    }

    public void Initialize()
    {
        tree.transform.position = tree.transform.parent.position;
        gira.GetComponent<GiraffeCharacter>().forwardSpeed = 0f;
        gira.GetComponent<GiraffeCharacter>().turnSpeed = 0f;
        gira.transform.position = gira.transform.parent.position;
        var grot = gira.transform.parent.rotation;
        grot.y = grot.y + 90;
        gira.transform.rotation = grot;

    }
    void Update()
    {
        if ((!gira.activeSelf || !tree.activeSelf) && Global.giraffe_mode)
        {
            Initialize();
            init = 1;
            Global.giraffe_mode = false;
        }
        if (gira.activeSelf && tree.activeSelf && init == 1)
        {
            init = 0;
            Global.giraffe_mode = true;
            StartCoroutine(giraffe_turn());
        }
    }
    #endregion // PROTECTED_METHODS

}
