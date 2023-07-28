using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class elephant : MonoBehaviour, ITrackableEventHandler
{
    #region PUBLIC_MEMBER_VARIABLES

    public GameObject elep;
    public GameObject tree;
    int init = 0;
    public bool flag = false;
    #endregion

    #region PROTECTED_MEMBER_VARIABLES

    protected TrackableBehaviour mTrackableBehaviour;
    protected TrackableBehaviour.Status m_PreviousStatus;
    protected TrackableBehaviour.Status m_NewStatus;

    #endregion // PROTECTED_MEMBER_VARIABLES

    #region UNITY_MONOBEHAVIOUR_METHODS

    protected virtual void Start()
    {
        elep.transform.position = elep.transform.parent.position;
        var erot = elep.transform.parent.rotation;
        erot.y = erot.y + 90;
        elep.transform.rotation = erot;

        Global.elepant_mode = true;

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
        tree.SetActive(true);
        StartCoroutine(elephant_turn());
        flag = false;
    }

    IEnumerator elephant_turn()
    {
        yield return new WaitForSeconds(1.0f);
        if (elep.activeSelf && tree.activeSelf)
        {
            Vector3 relativePos = tree.transform.position - elep.transform.position;
            Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
            float temp = -1.0f;
            if (rotation.w < 0)
                temp = 1.0f;
            elep.GetComponent<ElephantCharacter>().turnSpeed = temp;
            StartCoroutine("Stop_turn");
        }
        yield return null;
    }

    IEnumerator Stop_turn()
    {
        while (true)
        {
            Vector3 fwd = elep.transform.TransformDirection(Vector3.forward);
            float distance = Vector3.Distance(elep.transform.position, tree.transform.position);
            Ray ray1 = new Ray(elep.transform.position + new Vector3(0, 0.3f, 0), elep.transform.forward);
            Debug.DrawRay(ray1.origin, ray1.direction * 1000f, Color.red);
            RaycastHit hit;
            if (Physics.Raycast(elep.transform.position, fwd, out hit, distance * 2.0f))
            {
                if (hit.collider.gameObject.tag == "banana")
                {
                    elep.GetComponent<ElephantCharacter>().turnSpeed = 0f;
                    elep.GetComponent<ElephantCharacter>().forwardSpeed = 1.0f;
                    StartCoroutine(stop_go());
                    break;
                }
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
    IEnumerator stop_go()
    {
        while (true)
        {
            float dist = Vector3.Distance(elep.transform.position, tree.transform.position);
            if (dist < 0.6f)
            {
                elep.GetComponent<ElephantCharacter>().forwardSpeed = 0f;
                StartCoroutine(start_eat());
                break;
            }
            yield return new WaitForSeconds(0.01f);
        }
    }
    IEnumerator start_eat()
    {
        if (elep.activeSelf == true && tree.activeSelf == true)
        {
            //while (true)
            //{
                elep.GetComponent<ElephantCharacter>().Eat();
            yield return new WaitForSeconds(0.5f);
            tree.SetActive(false);
            flag = true;
                //if (init == 1)
                //    break;
            //    yield return new WaitForSeconds(0.01f);
            //}
        }
        yield return null;
    }
    protected virtual void OnTrackingFoundExtended()
    {
        Debug.Log("OnTrackingFoundExtended");
    }


    protected virtual void OnTrackingLost()
    {
        tree.SetActive(false);
        flag = false;
    }
    public void Initialize()
    {
        tree.transform.position = tree.transform.parent.position;
        tree.transform.rotation = tree.transform.parent.rotation;
        elep.GetComponent<ElephantCharacter>().forwardSpeed = 0f;
        elep.GetComponent<ElephantCharacter>().turnSpeed = 0f;
        elep.transform.position = elep.transform.parent.position;
        var erot = elep.transform.parent.rotation;
        erot.y = erot.y + 90;
        elep.transform.rotation = erot;
    }
    void Update()
    {
        if ((!elep.activeSelf || !tree.activeSelf) && Global.elepant_mode && !flag)
        {
            Initialize();
            Global.elepant_mode = false;
            init = 1;
        }
        if (elep.activeSelf && tree.activeSelf && init == 1)
        {
            init = 0;
            Global.elepant_mode = true;
            StartCoroutine(elephant_turn());
        }
    }
    #endregion // PROTECTED_METHODS

}
