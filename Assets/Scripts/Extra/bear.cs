using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class bear : MonoBehaviour, ITrackableEventHandler
{
    #region PUBLIC_MEMBER_VARIABLES
    public GameObject PolarBear;
    public GameObject BrownBear;
    #endregion

    #region PROTECTED_MEMBER_VARIABLES

    protected TrackableBehaviour mTrackableBehaviour;
    protected TrackableBehaviour.Status m_PreviousStatus;
    protected TrackableBehaviour.Status m_NewStatus;

    #endregion // PROTECTED_MEMBER_VARIABLES

    #region UNITY_MONOBEHAVIOUR_METHODS

    int init = 0;
    Transform bpos;
    protected virtual void Start()
    {
        PolarBear.transform.position = PolarBear.transform.parent.position;
        Global.bear_mode = true;
        var prot = PolarBear.transform.parent.rotation;
        prot.y = prot.y + 90f;
        PolarBear.transform.rotation = prot;

        bpos = BrownBear.transform;
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
        BrownBear.SetActive(true);
        StartCoroutine(bear_turn());
    }

    IEnumerator bear_turn()
    {
        yield return new WaitForSeconds(1.0f);
        if (PolarBear.activeSelf && BrownBear.activeSelf)
        {
            Vector3 relativePos = BrownBear.transform.position - PolarBear.transform.position;
            Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
            float temp = -1.0f;
            if (rotation.w < 0)
                temp = 1.0f;
            relativePos.Normalize();
            PolarBear.GetComponent<BearCharacter>().turnSpeed = Vector3.Dot(relativePos, transform.right);
            StartCoroutine(stop_turn());
            BrownBear.GetComponent<BearCharacter>().turnSpeed = temp;
            StartCoroutine(stop_turn1());
        }
        yield return null;
    }
    IEnumerator stop_turn()
    {
        while (true)
        {
            Vector3 fwd = PolarBear.transform.TransformDirection(Vector3.forward);
            float distance = Vector3.Distance(PolarBear.transform.position, BrownBear.transform.position);
            Ray ray1 = new Ray(PolarBear.transform.position + new Vector3(0, 0.5f, 0), PolarBear.transform.forward);
            RaycastHit hit;
            if (Physics.Raycast(PolarBear.transform.position + new Vector3(0, 0.5f, 0), fwd, out hit, distance * 2.0f))
            {
                if (hit.collider.transform.tag == "bear")
                {
                    PolarBear.GetComponent<BearCharacter>().turnSpeed = 0f;
                    PolarBear.GetComponent<BearCharacter>().forwardSpeed = 1.0f;
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
            Vector3 pfwd = BrownBear.transform.TransformDirection(Vector3.forward);
            float pdistance = Vector3.Distance(BrownBear.transform.position, PolarBear.transform.position);
            Ray pray2 = new Ray(BrownBear.transform.position + new Vector3(0, 0.5f, 0), BrownBear.transform.forward);
            //Debug.DrawRay(pray2.origin, BrownBear.transform.forward * 1000f, Color.red);
            RaycastHit phit;
            if (Physics.Raycast(BrownBear.transform.position + new Vector3(0, 0.5f, 0), pfwd, out phit, pdistance * 2.0f))
            {
                if (phit.collider.transform.tag == "pbear")
                {
                    BrownBear.GetComponent<BearCharacter>().turnSpeed = 0f;
                    BrownBear.GetComponent<BearCharacter>().forwardSpeed = 1.0f;
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
            float distance = Vector3.Distance(PolarBear.transform.position, BrownBear.transform.position);
            if (distance < 1.4f)
            {
                PolarBear.GetComponent<BearCharacter>().forwardSpeed = 0f;
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
            float distance = Vector3.Distance(PolarBear.transform.position, BrownBear.transform.position);
            if (distance < 1.4f)
            {
                BrownBear.GetComponent<BearCharacter>().forwardSpeed = 0f;
                yield return new WaitForSeconds(0.5f);
                StartCoroutine(bear_fight());
                break;
            }
            yield return new WaitForSeconds(0.01f);
        }
    }
    IEnumerator bear_fight()
    {
        if (PolarBear.activeSelf && BrownBear.activeSelf)
        {
            BrownBear.GetComponent<BearCharacter>().StandUp();
            yield return new WaitForSeconds(0.01f);
            while (true)
            {
                BrownBear.GetComponent<BearCharacter>().Attack();
                if (init == 1)
                {
                    break;
                }
                yield return new WaitForSeconds(0.01f);
            }
        }
        yield return null;
    }

    IEnumerator pbear_fight()
    {
        if (PolarBear.activeSelf && BrownBear.activeSelf)
        {
            PolarBear.GetComponent<BearCharacter>().StandUp();
            yield return new WaitForSeconds(0.01f);
            while (true)
            {
                PolarBear.GetComponent<BearCharacter>().Attack();
                if (init == 1)
                {
                    break;
                }
                yield return new WaitForSeconds(0.01f);
            }
        }
        yield return null;
    }
    public void Initialize()
    {
        BrownBear.GetComponent<BearCharacter>().StandUpEnd();
        BrownBear.GetComponent<BearCharacter>().forwardSpeed = 0f;
        BrownBear.GetComponent<BearCharacter>().turnSpeed = 0f;
        BrownBear.transform.position = BrownBear.transform.parent.position;
        BrownBear.transform.rotation = bpos.rotation;

        PolarBear.GetComponent<BearCharacter>().StandUpEnd();
        PolarBear.GetComponent<BearCharacter>().forwardSpeed = 0f;
        PolarBear.GetComponent<BearCharacter>().turnSpeed = 0f;
        PolarBear.transform.position = PolarBear.transform.parent.position;
        var prot = PolarBear.transform.parent.rotation;
        prot.y = prot.y + 90f;
        PolarBear.transform.rotation = prot;
    }
    protected virtual void OnTrackingFoundExtended()
    {
        Debug.Log("OnTrackingFoundExtended");
    }


    protected virtual void OnTrackingLost()
    {
        BrownBear.SetActive(false);
    }
    void Update()
    {
        if ((!PolarBear.activeSelf || !BrownBear.activeSelf) && Global.bear_mode)
        {
            Initialize();
            init = 1;
            Global.bear_mode = false;
        }
        else if (PolarBear.activeSelf && BrownBear.activeSelf && init == 1)
        {
            init = 0;
            Global.bear_mode = true;
            StartCoroutine(bear_turn());
        }
    }
    #endregion // PROTECTED_METHODS

}
