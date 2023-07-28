using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class zebra : MonoBehaviour, ITrackableEventHandler
{
    #region PUBLIC_MEMBER_VARIABLES
    public GameObject zeb;
    public GameObject grass;
    public GameObject col;

    #endregion

    #region PROTECTED_MEMBER_VARIABLES

    protected TrackableBehaviour mTrackableBehaviour;
    protected TrackableBehaviour.Status m_PreviousStatus;
    protected TrackableBehaviour.Status m_NewStatus;

    #endregion // PROTECTED_MEMBER_VARIABLES
    Transform bpos;
    int init = 0;

    #region UNITY_MONOBEHAVIOUR_METHODS

    protected virtual void Start()
    {
        zeb.transform.position = zeb.transform.parent.position;
        var zrot = zeb.transform.parent.rotation;
        zrot.y = zrot.y + 90;
        zeb.transform.rotation = zrot;

        Global.zebra_mode = true;

        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
        {
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
        }
        bpos = grass.transform;
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
        grass.SetActive(true);
        col.SetActive(true);
        StartCoroutine(zebra_turn());
    }

    IEnumerator zebra_turn()
    {
        yield return new WaitForSeconds(1.0f);
        if (zeb.activeSelf && grass.activeSelf)
        {
            Vector3 relativePos = grass.transform.position - zeb.transform.position;
            Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
            float temp = -1.0f;
            if (rotation.w < 0)
                temp = 1.0f;
            zeb.GetComponent<ZebraCharacter>().turnSpeed = temp;
            StartCoroutine(stop_turn());
        }
        yield return null;
    }

    IEnumerator stop_turn()
    {
        while (true)
        {
            Vector3 fwd = zeb.transform.TransformDirection(Vector3.forward);
            float distance = Vector3.Distance(zeb.transform.position, grass.transform.position);
            Ray ray1 = new Ray(zeb.transform.position + new Vector3(0, 5f, 0), zeb.transform.forward);
            Debug.DrawRay(ray1.origin, ray1.direction * 1000f, Color.red);
            RaycastHit hit;
            if (Physics.Raycast(zeb.transform.position + new Vector3(0, 5f, 0), fwd, out hit, distance * 2.0f))
            {
                if (hit.collider.gameObject.tag == "waterroll")
                {
                    zeb.GetComponent<ZebraCharacter>().turnSpeed = 0f;
                    zeb.GetComponent<ZebraCharacter>().forwardSpeed = 1.5f;
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
            float dist = Vector3.Distance(zeb.transform.position, grass.transform.position);
            Debug.Log(dist);
            if (dist < 67f)
            {
                zeb.GetComponent<ZebraCharacter>().forwardSpeed = 0f;
                StartCoroutine(zeb_eat());
                break;
            }
            yield return new WaitForSeconds(0.01f);
        }
    }
    IEnumerator zeb_eat()
    {
        zeb.GetComponent<ZebraCharacter>().EatStart();
        yield return null;
    }
    protected virtual void OnTrackingFoundExtended()
    {
        Debug.Log("OnTrackingFoundExtended");
    }


    protected virtual void OnTrackingLost()
    {
        grass.SetActive(false);
        col.SetActive(false);
    }
    public void Initialize()
    {
        zeb.GetComponent<ZebraCharacter>().EatEnd();
        zeb.GetComponent<ZebraCharacter>().forwardSpeed = 0f;
        zeb.GetComponent<ZebraCharacter>().turnSpeed = 0f;
        zeb.transform.position = zeb.transform.parent.position;
        var zrot = zeb.transform.parent.rotation;
        zrot.y = zrot.y + 90;
        zeb.transform.rotation = zrot;
        grass.transform.position = bpos.position;
    }

    void Update()
    {
        if ((!zeb.activeSelf || !grass.activeSelf) && Global.zebra_mode)
        {
            Initialize();
            init = 1;
            Global.zebra_mode = false;
        }
        if (zeb.activeSelf && grass.activeSelf && init == 1)
        {
            init = 0;
            Global.zebra_mode = true;
            StartCoroutine(zebra_turn());
        }
    }

    #endregion // PROTECTED_METHODS

}
