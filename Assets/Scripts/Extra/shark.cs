using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class shark : MonoBehaviour, ITrackableEventHandler
{
    #region PUBLIC_MEMBER_VARIABLES
    public GameObject shark1;
    public GameObject shark2;

    #endregion

    #region PROTECTED_MEMBER_VARIABLES

    protected TrackableBehaviour mTrackableBehaviour;
    protected TrackableBehaviour.Status m_PreviousStatus;
    protected TrackableBehaviour.Status m_NewStatus;

    #endregion // PROTECTED_MEMBER_VARIABLES

    #region UNITY_MONOBEHAVIOUR_METHODS

    bool init = false;
    protected virtual void Start()
    {
        shark1.transform.position = shark1.transform.parent.position;
        var srot = shark1.transform.parent.rotation;
        srot.y = srot.y + 90;
        shark1.transform.rotation = srot;


        shark2.transform.position = shark2.transform.parent.position;
        var sr = shark2.transform.parent.rotation;
        sr.y = sr.y - 0.7f;
        shark2.transform.rotation = sr;

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
        Global.shark_mode = true;
        shark2.SetActive(true);
        FoundScript();
    }

    protected virtual void OnTrackingFoundExtended()
    {
        Debug.Log("OnTrackingFoundExtended");
    }

    public void Initialize()
    {
        shark1.transform.position = shark1.transform.parent.position;
        var srot = shark1.transform.parent.rotation;
        srot.y = srot.y + 90;
        shark1.transform.rotation = srot;

        shark2.transform.position = shark2.transform.parent.position;
        var sr = shark2.transform.parent.rotation;
        sr.y = sr.y - 0.7f;
        shark2.transform.rotation = sr;

        shark1.GetComponent<SharkCharacter>().turnAccerelation = 0f;
        shark1.GetComponent<SharkCharacter>().upDownAccerelation = 0f;
        shark1.GetComponent<SharkCharacter>().rollAccerelation = 0f;
        shark1.GetComponent<SharkCharacter>().forwardAccerelation = 0f;
        shark2.GetComponent<SharkCharacter>().turnAccerelation = 0f;
        shark2.GetComponent<SharkCharacter>().upDownAccerelation = 0f;
        shark2.GetComponent<SharkCharacter>().rollAccerelation = 0f;
        shark2.GetComponent<SharkCharacter>().forwardAccerelation = 0f;

        LostScript();
    }

    private void LostScript()
    {
        shark2.GetComponent<SharkJController>().enabled = true;
        shark2.GetComponent<SharkChaseAttackScript>().enabled = false;
        shark1.GetComponent<SharkJController>().enabled = true;
        shark1.GetComponent<SharkChaseAttackScript>().enabled = false;
    }
    private void FoundScript()
    {
        shark2.GetComponent<SharkJController>().enabled = false;
        shark2.GetComponent<SharkChaseAttackScript>().enabled = true;
        shark1.GetComponent<SharkJController>().enabled = false;
        shark1.GetComponent<SharkChaseAttackScript>().enabled = true;
    }
    protected virtual void OnTrackingLost()
    {
        shark2.SetActive(false);
        Global.shark_mode = false;
        LostScript();
    }

    public void Update()
    {
        if ((!shark1.activeSelf || !shark2.activeSelf) && Global.shark_mode)
        {
            Initialize();
            Global.shark_mode = false;
            init = true;
        }

        if (shark1.activeSelf && shark2.activeSelf && init)
        {
            init = false;
            Global.shark_mode = true;
            FoundScript();
        }

    }
    #endregion // PROTECTED_METHODS

}
