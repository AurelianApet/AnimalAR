using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class cheetah : MonoBehaviour, ITrackableEventHandler
{
    #region PUBLIC_MEMBER_VARIABLES
    public GameObject cheet1;
    public GameObject cheet2;

    float angle360(Vector3 from, Vector3 to, Vector3 right)
    {
        float angle = Vector3.Angle(from, to);
        return (Vector3.Angle(right, to) > 90f) ? 360f - angle : angle;
    }
    #endregion

    #region PROTECTED_MEMBER_VARIABLES

    protected TrackableBehaviour mTrackableBehaviour;
    protected TrackableBehaviour.Status m_PreviousStatus;
    protected TrackableBehaviour.Status m_NewStatus;

    #endregion // PROTECTED_MEMBER_VARIABLES

    int init = 0;
    int cnt = 0;
    Transform cpos;

    #region UNITY_MONOBEHAVIOUR_METHODS

    protected virtual void Start()
    {
        cheet1.transform.position = cheet1.transform.parent.position;
        var crot = cheet1.transform.parent.rotation;
        crot.y = crot.y + 90;
        cheet1.transform.rotation = crot;
        cpos = cheet2.transform;

        Global.cheetah_mode = true;

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
        cheet2.SetActive(true);
        StartCoroutine(bear_turn());
    }

    IEnumerator bear_turn()
    {
        yield return new WaitForSeconds(1.0f);
        if (cheet1.activeSelf && cheet2.activeSelf)
        {
            float pangle = angle360(cheet1.transform.position, cheet2.transform.position, cheet1.transform.right);
            Vector3 relativePos = cheet2.transform.position - cheet1.transform.position;
            Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
            float temp = -1.0f;
            if (rotation.w < 0)
                temp = 1.0f;
            Debug.Log("Turn!");
            cheet2.GetComponent<CheetahCharacter>().turnSpeed = temp;
            StartCoroutine(stop_turn1());
            cheet1.GetComponent<CheetahCharacter>().turnSpeed = temp;
            StartCoroutine(stop_turn());
        }
        yield return null;
    }
    IEnumerator stop_turn()
    {
        while (true)
        {
            Vector3 fwd = cheet1.transform.TransformDirection(Vector3.forward);
            float distance = Vector3.Distance(cheet1.transform.position, cheet2.transform.position);
            Ray ray1 = new Ray(cheet1.transform.position + new Vector3(0, 0.2f, 0), cheet1.transform.forward);
            RaycastHit hit;
            if (Physics.Raycast(cheet1.transform.position + new Vector3(0, 0.2f, 0), fwd, out hit, distance * 2.0f))
            {
                if (hit.collider.transform.tag == "cheet2")
                {
                    Debug.Log(hit.collider.transform.tag + " " + cnt);
                    cnt++;
                    cheet1.GetComponent<CheetahCharacter>().turnSpeed = 0f;
                    Debug.Log(cheet1.GetComponent<CheetahCharacter>().turnSpeed);
                    cheet1.GetComponent<CheetahCharacter>().forwardSpeed = 1.0f;
                    StartCoroutine(stop_go());
                    break;
                }
            }
            yield return new WaitForSeconds(0.01f);
        }
    }
    IEnumerator stop_turn1()
    {
        while (true)
        {
            Vector3 pfwd = cheet2.transform.TransformDirection(Vector3.forward);
            float pdistance = Vector3.Distance(cheet2.transform.position, cheet1.transform.position);
            Ray pray2 = new Ray(cheet2.transform.position + new Vector3(0, 0.2f, 0), cheet2.transform.forward);
            RaycastHit phit;
            if (Physics.Raycast(cheet2.transform.position + new Vector3(0, 0.2f, 0), pfwd, out phit, pdistance * 2.0f))
            {
                if (phit.collider.transform.tag == "cheet1")
                {
                    Debug.Log(phit.collider.transform.tag);
                    cheet2.GetComponent<CheetahCharacter>().turnSpeed = 0f;
                    cheet2.GetComponent<CheetahCharacter>().forwardSpeed = 1.0f;
                    StartCoroutine(stop_go1());
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
            float distance = Vector3.Distance(cheet1.transform.position, cheet2.transform.position);
            if (distance < 1.6f)
            {
                cheet1.GetComponent<CheetahCharacter>().forwardSpeed = 0f;
                StartCoroutine(pbear_fight());
                break;
            }
            yield return new WaitForSeconds(0.01f);
        }
    }
    IEnumerator stop_go1()
    {
        while (true)
        {
            float distance = Vector3.Distance(cheet1.transform.position, cheet2.transform.position);
            if (distance < 1.6f)
            {
                cheet2.GetComponent<CheetahCharacter>().forwardSpeed = 0f;
                yield return new WaitForSeconds(0.5f);
                StartCoroutine(bear_fight());
                break;
            }
            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator bear_fight()
    {
        if (cheet2.activeSelf && cheet1.activeSelf)
        {
            while (true)
            {
                cheet2.GetComponent<CheetahCharacter>().Attack();
                if (init == 1)
                {
                    break;
                }
                yield return new WaitForSeconds(0.01f);
            }
        }
    }

    IEnumerator pbear_fight()
    {
        if (cheet1.activeSelf && cheet2.activeSelf)
        {
            while (true)
            {
                cheet1.GetComponent<CheetahCharacter>().Attack();
                if (init == 1)
                {
                    break;
                }
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
        cheet2.SetActive(false);
    }
    public void Initialize()
    {
        cheet2.GetComponent<CheetahCharacter>().forwardSpeed = 0f;
        cheet2.GetComponent<CheetahCharacter>().turnSpeed = 0f;
        cheet2.transform.position = cheet2.transform.parent.position;
        cheet2.transform.rotation = cpos.rotation;

        cheet1.GetComponent<CheetahCharacter>().forwardSpeed = 0f;
        cheet1.GetComponent<CheetahCharacter>().turnSpeed = 0f;
        cheet1.transform.position = cheet1.transform.parent.position;
        var crot = cheet1.transform.parent.rotation;
        crot.y = crot.y + 90;
        cheet1.transform.rotation = crot;
    }
    void Update()
    {
        if ((!cheet1.activeSelf || !cheet2.activeSelf) && Global.cheetah_mode)
        {
            Initialize();
            init = 1;
            Global.cheetah_mode = false;
        }
        if (cheet1.activeSelf && cheet2.activeSelf && init == 1)
        {
            init = 0;
            Global.cheetah_mode = true;
            StartCoroutine(bear_turn());
        }
        Debug.Log("TurnSpeed = " + cheet1.GetComponent<CheetahCharacter>().turnSpeed);
    }
    #endregion // PROTECTED_METHODS

}
