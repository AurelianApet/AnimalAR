using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class chicken : MonoBehaviour, ITrackableEventHandler
{
    #region PUBLIC_MEMBER_VARIABLES

    public GameObject cock;
    public GameObject[] seeds;

    #endregion

    #region PROTECTED_MEMBER_VARIABLES

    protected TrackableBehaviour mTrackableBehaviour;
    protected TrackableBehaviour.Status m_PreviousStatus;
    protected TrackableBehaviour.Status m_NewStatus;

    #endregion // PROTECTED_MEMBER_VARIABLES

    int count;
    int eatOrder = 0;
    bool init = false;

    #region UNITY_MONOBEHAVIOUR_METHODS

    protected virtual void Start()
    {
        cock.transform.position = cock.transform.parent.position;
        var crot = cock.transform.parent.rotation;
        crot.y += 90;
        cock.transform.rotation = crot;
        Global.chicken_mode = true;
        count = seeds.Length;
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

    void ShowSeeds(bool fg)
    {
        for (int i = 0; i < count; i++)
        {
            seeds[i].SetActive(fg);
        }
    }
    bool isHideSeeds()
    {
        for(int i = 0; i < count; i++)
        {
            if (seeds[i].activeSelf)
                return false;
        }
        return true;
    }

    bool isShowSeeds()
    {
        for (int i = 0; i < count; i++)
        {
            if (!seeds[i].activeSelf)
                return false;
        }
        return true;
    }
    protected virtual void OnTrackingFound()
    {
        ShowSeeds(true);
        StartCoroutine(turn_chicken());
    }
    IEnumerator turn_chicken()
    {
        yield return new WaitForSeconds(1.0f);
        if (cock.activeSelf)
        {
            Vector3 relativePos = seeds[eatOrder].transform.position - cock.transform.position;
            Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
            if(rotation.w < cock.transform.rotation.w)
                cock.GetComponent<ChickenCharacter>().yawVelocity = 1f;
            else
                cock.GetComponent<ChickenCharacter>().yawVelocity = -1f;
            StartCoroutine(stop_turn());
            seeds[eatOrder].gameObject.tag = "seed1";
        }
        yield return null;
    }
    IEnumerator stop_turn()
    {
        while (true)
        {
            Vector3 fwd = cock.transform.TransformDirection(Vector3.forward);
            Ray ray1 = new Ray(cock.transform.position + new Vector3(0, 0f, 0), cock.transform.forward);
            Debug.DrawRay(ray1.origin, ray1.direction * 1000f, Color.red);
            RaycastHit hit;
            if (Physics.Raycast(cock.transform.position + new Vector3(0, 0f, 0), fwd, out hit))
            {
                if (hit.collider.gameObject.tag == ("seed1"))
                {
                    cock.GetComponent<ChickenCharacter>().yawVelocity = 0f;
                    cock.GetComponent<ChickenCharacter>().forwardAcceleration = 1.0f;
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
            float distance = Vector3.Distance(cock.transform.position, seeds[eatOrder].transform.position);
            if (distance < 0.4f)
            {
                cock.GetComponent<ChickenCharacter>().forwardAcceleration = 0f;
                cock.GetComponent<ChickenCharacter>().Eat();
                yield return new WaitForSeconds(0.5f);
                seeds[eatOrder].SetActive(false);
                eatOrder++;
                if (eatOrder <= 29)
                {
                    StartCoroutine(turn_chicken());
                    break;
                }
                if (eatOrder > 29)
                {
                    break;
                }
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
        ShowSeeds(false);
    }
    void InitSeedsTag()
    {
        for(int i = 0; i < count; i++)
        {
            seeds[i].gameObject.tag = "seed";
        }
    }
    public void Initialize()
    {
        StopAllCoroutines();
        ShowSeeds(true);
        InitSeedsTag();
        eatOrder = 0;
        cock.transform.position = cock.transform.parent.position;
        var crot = cock.transform.parent.rotation;
        crot.y += 90;
        cock.transform.rotation = crot;
        cock.GetComponent<ChickenCharacter>().forwardAcceleration = 0f;
        cock.GetComponent<ChickenCharacter>().yawVelocity = 0f;
    }

    protected virtual void Update()
    {
        if (!cock.activeSelf && isHideSeeds())
        {
            return;
        }
        if (!cock.activeSelf && Global.chicken_mode)
        {
            Initialize();
            init = true;
            Global.chicken_mode = false;
        }
        if (cock.activeSelf && init)
        {
            init = false;
            Global.chicken_mode = true;
            StartCoroutine(turn_chicken());
        }
    }
    #endregion // PROTECTED_METHODS

}
