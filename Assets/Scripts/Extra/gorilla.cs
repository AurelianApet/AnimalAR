using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class gorilla : MonoBehaviour, ITrackableEventHandler
{
    #region PUBLIC_MEMBER_VARIABLES
    public GameObject animal;
    public GameObject animaltemp;
    #endregion

    #region PROTECTED_MEMBER_VARIABLES

    protected TrackableBehaviour mTrackableBehaviour;
    protected TrackableBehaviour.Status m_PreviousStatus;
    protected TrackableBehaviour.Status m_NewStatus;

    #endregion // PROTECTED_MEMBER_VARIABLES

    #region UNITY_MONOBEHAVIOUR_METHODS

    Transform grot;
    int init = 0;
    protected virtual void Start()
    {
        animal.transform.position = animal.transform.parent.position;
        var arot = animal.transform.parent.rotation;
        arot.y = arot.y + 90;
        animal.transform.rotation = arot;

        grot = animaltemp.transform;

        Global.gorilla_mode = true;

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
        animaltemp.SetActive(true);
        StartCoroutine(bear_turn());
    }

    float angle360(Vector3 from, Vector3 to, Vector3 right)
    {
        float angle = Vector3.Angle(from, to);
        return (Vector3.Angle(right, to) > 90f) ? 360f - angle : angle;
    }

    IEnumerator bear_turn()
    {
        yield return new WaitForSeconds(1.0f);
        if (animal.activeSelf && animaltemp.activeSelf)
        {
            float pangle = angle360(animal.transform.position, animaltemp.transform.position, animal.transform.right);
            Vector3 relativePos = animaltemp.transform.position - animal.transform.position;
            Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
            float temp = -1.0f;
            if (rotation.w < 0)
                temp = 1.0f;
            animal.GetComponent<GorillaCharacter>().turnSpeed = temp;
            StartCoroutine(stop_turn());
            animaltemp.GetComponent<GorillaCharacter>().turnSpeed = temp;
            StartCoroutine(stop_turn1());
        }
        yield return null;
    }
    IEnumerator stop_turn()
    {
        while (true)
        {
            Vector3 fwd = animal.transform.TransformDirection(Vector3.forward);
            float distance = Vector3.Distance(animal.transform.position, animaltemp.transform.position);
            Ray ray1 = new Ray(animal.transform.position + new Vector3(0, 0.15f, 0), animal.transform.forward);
            RaycastHit hit;
            if (Physics.Raycast(animal.transform.position + new Vector3(0, 0.15f, 0), fwd, out hit, distance * 2.0f))
            {
                if (hit.collider.transform.tag == "goril2")
                {
                    animal.GetComponent<GorillaCharacter>().turnSpeed = 0f;
                    animal.GetComponent<GorillaCharacter>().forwardSpeed = 1.0f;
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
            Vector3 pfwd = animaltemp.transform.TransformDirection(Vector3.forward);
            float pdistance = Vector3.Distance(animaltemp.transform.position, animal.transform.position);
            Ray pray2 = new Ray(animaltemp.transform.position + new Vector3(0, 0.15f, 0), animaltemp.transform.forward);
            RaycastHit phit;
            if (Physics.Raycast(animaltemp.transform.position + new Vector3(0, 0.15f, 0), pfwd, out phit, pdistance * 2.0f))
            {
                if (phit.collider.transform.tag == "goril1")
                {
                    animaltemp.GetComponent<GorillaCharacter>().turnSpeed = 0f;
                    animaltemp.GetComponent<GorillaCharacter>().forwardSpeed = 1.0f;
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
            float distance = Vector3.Distance(animal.transform.position, animaltemp.transform.position);
            if (distance < 1f)
            {
                animal.GetComponent<GorillaCharacter>().forwardSpeed = 0f;
                StartCoroutine(bear_fight());
                break;
            }
            yield return new WaitForSeconds(0.01f);
        }
    }
    IEnumerator stop_go1()
    {
        while (true)
        {
            float distance = Vector3.Distance(animal.transform.position, animaltemp.transform.position);
            if (distance < 1f)
            {
                animaltemp.GetComponent<GorillaCharacter>().forwardSpeed = 0f;
                yield return new WaitForSeconds(0.5f);
                StartCoroutine(pbear_fight());
                break;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator bear_fight()
    {
        if (animal.activeSelf && animaltemp.activeSelf)
        {
            while (true)
            {
                animal.GetComponent<GorillaCharacter>().Attack();
                if (init == 1)
                {
                    break;
                }
                yield return new WaitForSeconds(0.1f);
            }
        }
        yield return null;
    }

    IEnumerator pbear_fight()
    {
        if (animal.activeSelf && animaltemp.activeSelf)
        {
            while (true)
            {
                animaltemp.GetComponent<GorillaCharacter>().Attack();
                if (init == 1)
                {
                    break;
                }
                yield return new WaitForSeconds(0.01f);
            }
        }
        yield return null;
    }

    protected virtual void OnTrackingFoundExtended()
    {
        Debug.Log("OnTrackingFoundExtended");
    }


    protected virtual void OnTrackingLost()
    {
        animaltemp.SetActive(false);
    }
    public void Initialize()
    {
        animaltemp.GetComponent<GorillaCharacter>().forwardSpeed = 0f;
        animaltemp.GetComponent<GorillaCharacter>().turnSpeed = 0f;
        animaltemp.transform.position = animaltemp.transform.parent.position;
        animaltemp.transform.rotation = grot.rotation;
        animal.GetComponent<GorillaCharacter>().forwardSpeed = 0f;
        animal.GetComponent<GorillaCharacter>().turnSpeed = 0f;
        animal.transform.position = animal.transform.parent.position;
        var arot = animal.transform.parent.rotation;
        arot.y = arot.y + 90;
        animal.transform.rotation = arot;
    }
    void Update()
    {
        if ( (!animal.activeSelf || !animaltemp.activeSelf ) && Global.gorilla_mode)
        {
            Initialize();
            Global.gorilla_mode = false;
            init = 1;
        }
        if (animal.activeSelf && animaltemp.activeSelf && init == 1)
        {
            init = 0;
            Global.gorilla_mode = true;
            StartCoroutine(bear_turn());
        }
    }

    #endregion // PROTECTED_METHODS

}
