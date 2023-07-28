using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class harpy : MonoBehaviour, ITrackableEventHandler
{
    #region PUBLIC_MEMBER_VARIABLES
    public GameObject eagle;
    public GameObject fish;
    public GameObject background;

    #endregion

    #region PROTECTED_MEMBER_VARIABLES

    protected TrackableBehaviour mTrackableBehaviour;
    protected TrackableBehaviour.Status m_PreviousStatus;
    protected TrackableBehaviour.Status m_NewStatus;

    #endregion // PROTECTED_MEMBER_VARIABLES

    Transform fpos;
    Transform bpos;
    int init = 0;
    int cnt = 0;
    bool isFounded = false;

    #region UNITY_MONOBEHAVIOUR_METHODS

    protected virtual void Start()
    {
        eagle.transform.position = eagle.transform.parent.position;
        var erot  = eagle.transform.parent.rotation;
        erot.y = 90;
        eagle.transform.rotation = erot;

        Global.eagle_mode = true;

        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
        {
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
        }
        fpos = fish.transform;
        bpos = background.transform;

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
        fish.SetActive(true);
        background.SetActive(true);
        eagle.GetComponent<HarpyEagleCharacterScript>().controlMode = 1;
        StartCoroutine(turn_eagle());
        isFounded = true;
    }

    IEnumerator turn_eagle()
    {
        yield return new WaitForSeconds(1.0f);
        if (eagle.activeSelf && fish.activeSelf)
        {
            Vector3 relativePos = fish.transform.position - eagle.transform.position;
            Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
            float temp = -1.0f;
            if (rotation.w < 0)
                temp = 1.0f;
            Debug.Log("temp" + temp);
            Vector3 fwd = eagle.transform.TransformDirection(Vector3.forward);
            //Ray ray1 = new Ray(eagle.transform.position + new Vector3(0f, 1f, 0f), fwd);
            //Debug.DrawRay(ray1.origin, ray1.direction * 1000f, Color.red);
            RaycastHit hit;
            if (Physics.Raycast(eagle.transform.position + new Vector3(0f, 1f, 0f), fwd, out hit))
            {
                if (hit.collider.transform.tag == "fish")
                {
                    eagle.GetComponent<HarpyEagleCharacterScript>().yawVelocity = 0f;
                    StartCoroutine(fly_eagle());
                }
                else
                {
                    eagle.GetComponent<HarpyEagleCharacterScript>().yawVelocity = temp;
                    StartCoroutine(stop_turn());
                }
            }
            else
            {
                eagle.GetComponent<HarpyEagleCharacterScript>().yawVelocity = temp;
                StartCoroutine(stop_turn());
            }
        }
        yield return null;
    }
    IEnumerator stop_turn()
    {
        while (true)
        {
            float dis = Vector3.Distance(eagle.transform.position, fish.transform.position);
            Vector3 fwd = eagle.transform.TransformDirection(Vector3.forward);
            Ray ray1 = new Ray(eagle.transform.position, eagle.transform.forward);
            //Debug.DrawRay(ray1.origin + new Vector3(0f, 0f, 0f), ray1.direction * 1000f, Color.red);
            RaycastHit hit;
            if (Physics.Raycast(eagle.transform.position + new Vector3(0f, 0.08f, 0f), fwd, out hit))
            {
                if (hit.collider.transform.tag == "fish")
                {
                    eagle.GetComponent<HarpyEagleCharacterScript>().yawVelocity = 0f;
                    StartCoroutine(fly_eagle());
                    break;
                }
            }
            yield return new WaitForSeconds(0.01f);
        }
    }
    IEnumerator fly_eagle()
    {
        while (true)
        {
            if (!eagle.activeSelf || !fish.activeSelf)
                break;
            eagle.GetComponent<HarpyEagleCharacterScript>().isGrounded = true;
            eagle.GetComponent<HarpyEagleCharacterScript>().Soar();
            if (eagle.transform.position.y > 0.1f)
            {
                StartCoroutine(go_eagle());
                break;
            }
            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator go_eagle()

    {
        if (eagle.activeSelf && fish.activeSelf)
        {
            eagle.GetComponent<HarpyEagleCharacterScript>().forwardAcceleration = 1.0f;
            eagle.GetComponent<HarpyEagleCharacterScript>().forwardSpeed = 1.0f;
            StartCoroutine(stop_go());
        }
        yield return null;
    }
    IEnumerator stop_go()
    {
        while (true)
        {
            //float dis = Vector3.Distance(eagle.transform.position, fish.transform.position);
            //Debug.Log(dis);
            //if (dis < 1.25f)
            //{
            //    eagle.GetComponent<HarpyEagleCharacterScript>().forwardAcceleration = 0f;
            //    eagle.GetComponent<HarpyEagleCharacterScript>().forwardSpeed = 0f;
            //    StartCoroutine(land_eagle());
            //    break;
            //}
            Vector3 down = eagle.transform.TransformDirection(Vector3.down);
            //Ray ray1 = new Ray(eagle.transform.position, Vector3.down);
            //Debug.DrawRay(ray1.origin + new Vector3(0f, 0f, 0f), ray1.direction * 1000f, Color.red);
            RaycastHit hit;
            if (Physics.Raycast(eagle.transform.position + new Vector3(0f, 0f, -0.2f), down, out hit))
            {
                if (hit.collider.transform.tag == "fish")
                {
                    eagle.GetComponent<HarpyEagleCharacterScript>().forwardAcceleration = 0f;
                    eagle.GetComponent<HarpyEagleCharacterScript>().forwardSpeed = 0f;
                    StartCoroutine(land_eagle());
                    break;
                }
            }
            yield return new WaitForSeconds(0.01f);
        }
    }
    IEnumerator land_eagle()
    {
        if (eagle.activeSelf && fish.activeSelf)
        {
            eagle.GetComponent<HarpyEagleCharacterScript>().upDown = -0.1f;
            eagle.GetComponent<HarpyEagleCharacterScript>().forwardAcceleration = 0f;
            eagle.GetComponent<HarpyEagleCharacterScript>().forwardSpeed = 0f;
            eagle.GetComponent<HarpyEagleCharacterScript>().Attack();
            yield return new WaitForSeconds(1.0f);
            eagle.GetComponent<HarpyEagleCharacterScript>().upDown = 0f;
            eagle.GetComponent<HarpyEagleCharacterScript>().Landing();
            yield return new WaitForSeconds(1.0f);
            eagle.GetComponent<HarpyEagleCharacterScript>().Eat();
            StartCoroutine(AfterEat());
        }
        yield return null;
    }
    IEnumerator AfterEat()
    {
        yield return new WaitForSeconds(1.0f);
        if (eagle.activeSelf && background.activeSelf && fish.activeSelf)
            fish.SetActive(false);
        StartCoroutine("SFly");
    }
    protected virtual void OnTrackingFoundExtended()
    {
        Debug.Log("OnTrackingFoundExtended");
    }

    IEnumerator SFly()
    {
        while (true)
        {
            if (!eagle.activeSelf)
                break;
            eagle.GetComponent<HarpyEagleCharacterScript>().isGrounded = true;
            eagle.GetComponent<HarpyEagleCharacterScript>().Soar();
            if (eagle.transform.position.y > 0.1f)
            {
                StartCoroutine(sgo_eagle());
                break;
            }
            yield return new WaitForSeconds(0.01f);
        }
    }
    IEnumerator sgo_eagle()
    {
        eagle.GetComponent<HarpyEagleCharacterScript>().upDown = 1.0f;
        eagle.GetComponent<HarpyEagleCharacterScript>().forwardSpeed = 1.0f;
        eagle.GetComponent<HarpyEagleCharacterScript>().forwardAcceleration = 0.1f;
        yield return new WaitForSeconds(2.0f);
        eagle.GetComponent<HarpyEagleCharacterScript>().yawVelocity = 0.6f;
        eagle.GetComponent<HarpyEagleCharacterScript>().upDown = 0f;
        yield return null;
    }

    protected virtual void OnTrackingLost()
    {
        fish.SetActive(false);
        background.SetActive(false);
        isFounded = false;
    }

    public void Initialize()
    {
        StopAllCoroutines();
        eagle.GetComponent<HarpyEagleCharacterScript>().forwardSpeed = 0;
        eagle.GetComponent<HarpyEagleCharacterScript>().yawVelocity = 0;
        eagle.GetComponent<HarpyEagleCharacterScript>().upDown = 0f;
        eagle.GetComponent<HarpyEagleCharacterScript>().forwardAcceleration = 0f;
        eagle.GetComponent<HarpyEagleCharacterScript>().Landing();

        eagle.transform.position = eagle.transform.parent.position;
        var erot = eagle.transform.parent.rotation;
        erot.y = 90;
        eagle.transform.rotation = erot;
        fish.transform.position = fpos.position;
        fish.transform.rotation = fpos.rotation;
        background.transform.position = bpos.position;
        background.transform.rotation = bpos.rotation;
        if (isFounded && Global.eagle_mode)
        {
            fish.SetActive(true);
        }
    }
    protected virtual void Update()
    {
        if ((!eagle.activeSelf || (!fish.activeSelf && !background.activeSelf)) && Global.eagle_mode)
        {
            Initialize();
            init = 1;
            Global.eagle_mode = false;
        }
        if (eagle.activeSelf && fish.activeSelf && background.activeSelf && init == 1)
        {
            init = 0;
            Global.eagle_mode = true;
            StartCoroutine(turn_eagle());
        }
        Vector3 fwd = eagle.transform.TransformDirection(Vector3.forward);
        Ray ray1 = new Ray(eagle.transform.position + new Vector3(0f, 0.1f, 0f), fwd);
        //Debug.DrawRay(ray1.origin, ray1.direction * 1000f, Color.red);
    }

    #endregion // PROTECTED_METHODS

}
