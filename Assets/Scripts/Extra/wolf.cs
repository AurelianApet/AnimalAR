using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class wolf : MonoBehaviour, ITrackableEventHandler
{
    #region PUBLIC_MEMBER_VARIABLES
    public GameObject animal;
    public GameObject hill;
    public GameObject col;
    #endregion

    #region PROTECTED_MEMBER_VARIABLES

    protected TrackableBehaviour mTrackableBehaviour;
    protected TrackableBehaviour.Status m_PreviousStatus;
    protected TrackableBehaviour.Status m_NewStatus;

    #endregion // PROTECTED_MEMBER_VARIABLES

    #region UNITY_MONOBEHAVIOUR_METHODS
    int init = 0;
    Quaternion rotation;

    protected virtual void Start()
    {
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
            ////Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " found , Status : " + newStatus.ToString());
            //if (newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
            //    OnTrackingFoundExtended();
            //else
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
        hill.SetActive(true);
        col.SetActive(true);

        Global.wolf_mode = true;
        animal.transform.localPosition = animal.transform.parent.localPosition;
        animal.transform.localRotation = animal.transform.parent.localRotation;
        StartCoroutine(turn_animal());
    }

    IEnumerator turn_animal()
    {
        yield return new WaitForSeconds(1.0f);
        if (animal.activeSelf && hill.activeSelf)
        {
            Vector3 relativePos = col.transform.position - animal.transform.position;
            rotation = Quaternion.LookRotation(relativePos, Vector3.up);
            float temp = -1.0f;
            if (rotation.w < 0)
                temp = 1.0f;
            animal.GetComponent<WolfCharacter>().turnSpeed = temp;
            StartCoroutine(stop_turn());
        }
        yield return null;
    }
    IEnumerator stop_turn()
    {
        while (true)
        {
            Vector3 fwd = animal.transform.TransformDirection(Vector3.forward);
            Ray ray1 = new Ray(animal.transform.position + new Vector3(0, 2f, 0), animal.transform.forward);
            RaycastHit hit;
            //Debug.DrawRay(ray1.origin, ray1.direction * 1000f, Color.red);
            if (Physics.Raycast(animal.transform.position + new Vector3(0, 2f, 0), fwd, out hit))
            {
                if (hit.collider.gameObject.tag == "hill")
                {
                    animal.GetComponent<WolfCharacter>().turnSpeed = 0f;
                    animal.GetComponent<WolfCharacter>().forwardSpeed = 3f;
                    StartCoroutine(stop_go());
                    break;
                }
            }
            yield return new WaitForSeconds(0.001f);
        }
    }
    IEnumerator stop_go()
    {
        while (true)
        {
            float distance = Vector3.Distance(animal.transform.position, col.transform.position);
            Debug.Log(distance);
            if (distance < 1.5f)
            {
                animal.GetComponent<WolfCharacter>().forwardSpeed = 0f;
                yield return new WaitForSeconds(0.3f);
                animal.GetComponent<WolfCharacter>().Hit();
                yield return new WaitForSeconds(1.0f);
                animal.GetComponent<WolfCharacter>().Roar();
                break;
            }
            yield return new WaitForSeconds(0.01f);
        }
    }

    protected virtual void OnTrackingFoundExtended()
    {
        Debug.Log("OnTrackingFoundExtended");
    }


    protected virtual void OnTrackingLost()
    {
        hill.SetActive(false);
        col.SetActive(false);
    }

    public void Initialize()
    {
        Debug.Log("initialize");
        hill.transform.localPosition = hill.transform.parent.localPosition + new Vector3(52f, 0f, -48.99f);
        col.transform.localPosition = col.transform.parent.localPosition + new Vector3(10.297f, 3.782f, 3.733f);
        animal.transform.localPosition = animal.transform.parent.localPosition;
        animal.transform.localRotation = animal.transform.parent.localRotation;
        animal.GetComponent<WolfCharacter>().forwardSpeed = 0f;
        animal.GetComponent<WolfCharacter>().turnSpeed = 0f;
    }
    void Update()
    {
        if ((!animal.activeSelf || !hill.activeSelf) && Global.wolf_mode)
        {
            Initialize();
            init = 1;
            Global.wolf_mode = false;
        }
        if (animal.activeSelf && hill.activeSelf && init == 1)
        {
            init = 0;
            Global.wolf_mode = true;
            StartCoroutine(turn_animal());
        }
    }
    #endregion // PROTECTED_METHODS

}
