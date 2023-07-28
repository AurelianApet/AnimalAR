using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class lion : MonoBehaviour, ITrackableEventHandler
{
    #region PUBLIC_MEMBER_VARIABLES
    public GameObject male;
    public GameObject female;
    #endregion

    #region PROTECTED_MEMBER_VARIABLES

    protected TrackableBehaviour mTrackableBehaviour;
    protected TrackableBehaviour.Status m_PreviousStatus;
    protected TrackableBehaviour.Status m_NewStatus;

    #endregion // PROTECTED_MEMBER_VARIABLES

    #region UNITY_MONOBEHAVIOUR_METHODS

    static Transform frot;
    int init = 0;
    int fmeet = 0, mmeet = 0;
    protected virtual void Start()
    {
        male.transform.position = male.transform.parent.position;
        var mrot = male.transform.parent.rotation;
        mrot.y = mrot.y + 90;
        male.transform.rotation = mrot;

        frot = female.transform;
        Global.lion_mode = true;

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
        female.SetActive(true);
        StartCoroutine(turn_lion());
    }

    float angle360(Vector3 from, Vector3 to, Vector3 right)
    {
        float angle = Vector3.Angle(from, to);
        return (Vector3.Angle(right, to) > 90f) ? 360f - angle : angle;
    }

    IEnumerator turn_lion()
    {
        yield return new WaitForSeconds(1.0f);
        if (male.activeSelf && female.activeSelf)
        {
            Vector3 relativePos = female.transform.position - male.transform.position;
            Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
            float temp = -1.0f;
            if (rotation.w < 0)
                temp = 1.0f;
            male.GetComponent<LionCharacter>().turnSpeed = temp;
            StartCoroutine(stop_turn());
            female.GetComponent<LionCharacter>().turnSpeed = -temp;
            StartCoroutine(stop_turn1());
        }
        yield return null;
    }
    IEnumerator stop_turn()
    {
        while (true)
        {
            Vector3 fwd = male.transform.TransformDirection(Vector3.forward);
            float distance = Vector3.Distance(male.transform.position, female.transform.position);
            Ray ray1 = new Ray(male.transform.position + new Vector3(0, 0.28f, 0), male.transform.forward);
            Debug.DrawRay(ray1.origin, ray1.direction * 200000f, Color.red);
            RaycastHit hit;
            if (Physics.Raycast(male.transform.position + new Vector3(0, 0.28f, 0), fwd, out hit, distance))
            {
                if (hit.collider.gameObject.tag == "lion1")
                {
                    male.GetComponent<LionCharacter>().turnSpeed = 0f;
                    male.GetComponent<LionCharacter>().forwardSpeed = 1f;
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
            Vector3 pfwd = female.transform.TransformDirection(Vector3.forward);
            float pdistance = Vector3.Distance(female.transform.position, male.transform.position);
            Ray pray1 = new Ray(female.transform.position + new Vector3(0, 0.28f, 0), female.transform.forward);
            Debug.DrawRay(pray1.origin, pray1.direction * 200000f, Color.red);
            RaycastHit phit;
            if (Physics.Raycast(female.transform.position + new Vector3(0, 0.28f, 0), pfwd, out phit, pdistance))
            {
                if (phit.collider.gameObject.tag == "lion")
                {
                    female.GetComponent<LionCharacter>().turnSpeed = 0f;
                    female.GetComponent<LionCharacter>().forwardSpeed = 1f;
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
            float distance = Vector3.Distance(male.transform.position, female.transform.position);
            if (distance < 1.4f)
            {
                fmeet = 1;
                male.GetComponent<LionCharacter>().forwardSpeed = 0f;
                if (fmeet == 1 && mmeet == 1)
                {
                    fmeet = 0;
                    mmeet = 0;
                    StartCoroutine(walk_mlion());
                    StartCoroutine(walk_flion());
                }
                break;
            }
            yield return new WaitForSeconds(0.01f);
        }
    }
    IEnumerator stop_go1()
    {
        while (true)
        {
            float distance = Vector3.Distance(male.transform.position, female.transform.position);
            if (distance < 1.4f)
            {
                mmeet = 1;
                female.GetComponent<LionCharacter>().forwardSpeed = 0f;
                if (fmeet == 1 && mmeet == 1)
                {
                    fmeet = 0;
                    mmeet = 0;
                    StartCoroutine(walk_mlion());
                    StartCoroutine(walk_flion());
                }
                break;
            }
            yield return new WaitForSeconds(0.01f);
        }
    }
    IEnumerator walk_mlion()
    {
        if (male.activeSelf && female.activeSelf)
        {
            male.GetComponent<LionCharacter>().turnSpeed = 1.0f;
            male.GetComponent<LionCharacter>().forwardSpeed = 1.0f;
            yield return new WaitForSeconds(1.0f);
            male.GetComponent<LionCharacter>().turnSpeed = 0f;
            yield return new WaitForSeconds(1.0f);
            male.GetComponent<LionCharacter>().forwardSpeed = 0f;
            male.GetComponent<LionCharacter>().SitDown();
            yield return new WaitForSeconds(1.0f);
            StartCoroutine(lion_smmove());
        }
        yield return null;
    }

    IEnumerator walk_flion()
    {
        if (male.activeSelf && female.activeSelf)
        {
            female.GetComponent<LionCharacter>().turnSpeed = -1.0f;
            female.GetComponent<LionCharacter>().forwardSpeed = 1.0f;
            yield return new WaitForSeconds(1.0f);
            female.GetComponent<LionCharacter>().turnSpeed = 0f;
            yield return new WaitForSeconds(1.0f);
            female.GetComponent<LionCharacter>().forwardSpeed = 0f;
            female.GetComponent<LionCharacter>().SitDown();
            yield return new WaitForSeconds(1.0f);
            StartCoroutine(lion_sfmove());
        }
        yield return null;
    }

    IEnumerator lion_sfmove()
    {
        if (male.activeSelf && female.activeSelf)
        {
            yield return new WaitForSeconds(1.0f);
            female.GetComponent<LionCharacter>().LieDown();
            yield return new WaitForSeconds(1.0f);
        }
        yield return null;
    }
    IEnumerator lion_smmove()
    {
        if (male.activeSelf && female.activeSelf)
        {
            male.GetComponent<LionCharacter>().StandUp();
            yield return new WaitForSeconds(1.0f);
            male.GetComponent<LionCharacter>().Roar();
            yield return new WaitForSeconds(1.0f);
            male.GetComponent<LionCharacter>().forwardSpeed = 1.0f;
            male.GetComponent<LionCharacter>().turnSpeed = -0.5f;
        }
        yield return null;
    }

    IEnumerator lion_move()
    {
        if (male.activeSelf && female.activeSelf)
        {
            male.GetComponent<LionCharacter>().SitDown();
            female.GetComponent<LionCharacter>().SitDown();
        }
        yield return null;
    }

    protected virtual void OnTrackingFoundExtended()
    {
        Debug.Log("OnTrackingFoundExtended");
    }


    protected virtual void OnTrackingLost()
    {
        female.SetActive(false);
    }
    public void Initialize()
    {

        female.GetComponent<LionCharacter>().forwardSpeed = 0f;
        female.GetComponent<LionCharacter>().turnSpeed = 0f;
        female.transform.position = female.transform.parent.position;
        //female.transform.rotation = frot.rotation;

        var fmrot = female.transform.parent.rotation;
        fmrot.y = fmrot.y - 0.7f;
        female.transform.rotation = fmrot;

        Debug.Log("female rotation = " + female.transform.parent.rotation);
        Debug.Log("frot = " + frot.rotation);


        male.GetComponent<LionCharacter>().forwardSpeed = 0f;
        male.GetComponent<LionCharacter>().turnSpeed = 0f;
        male.transform.position = male.transform.parent.position;
        var mrot = male.transform.parent.rotation;
        mrot.y = mrot.y + 90;
        male.transform.rotation = mrot;
        if (male.activeSelf)
        {
            male.GetComponent<LionCharacter>().StandUp();
        }
        if (female.activeSelf)
        {
            female.GetComponent<LionCharacter>().StandUp();
        }
    }
    void Update()
    {
        if ((!male.activeSelf || !female.activeSelf) && Global.lion_mode)
        {
            Initialize();
            Global.lion_mode = false;
            init = 1;
        }
        if (male.activeSelf && female.activeSelf && init == 1)
        {
            init = 0;
            Global.lion_mode = true;
            StartCoroutine(turn_lion());
        }
    }
    #endregion // PROTECTED_METHODS

}
