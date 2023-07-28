using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class rhinoceros : MonoBehaviour, ITrackableEventHandler
{
    #region PUBLIC_MEMBER_VARIABLES
    public GameObject rhino;
    public GameObject ball;
    #endregion

    #region PROTECTED_MEMBER_VARIABLES

    protected TrackableBehaviour mTrackableBehaviour;
    protected TrackableBehaviour.Status m_PreviousStatus;
    protected TrackableBehaviour.Status m_NewStatus;

    #endregion // PROTECTED_MEMBER_VARIABLES

    #region UNITY_MONOBEHAVIOUR_METHODS

    int init = 0;
    Rigidbody ballRigid;
    bool fd = false;
    float pdist;
    int isStart = 0;
    protected virtual void Start()
    {
        rhino.transform.position = rhino.transform.parent.position;
        var rot = rhino.transform.parent.rotation;
        rot.y = rot.y + 90;
        rhino.transform.rotation = rot;

        Global.rhino_mode = true;

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

    float angle360(Vector3 from, Vector3 to, Vector3 right)
    {
        float angle = Vector3.Angle(from, to);
        return (Vector3.Angle(right, to) > 90f) ? 360f - angle : angle;
    }

    #region PROTECTED_METHODS

    protected virtual void OnTrackingFound()
    {
        ball.SetActive(true);
        ballRigid = ball.GetComponent<Rigidbody>();
        pdist = Vector3.Distance(rhino.transform.position, ball.transform.position);
        if (ball.activeSelf && rhino.activeSelf)
        {
            StartCoroutine(elephant_turn());
        }
    }

    IEnumerator elephant_turn()
    {
        Debug.Log("turn");
        isStart++;
        yield return new WaitForSeconds(1.0f);
        if (rhino.activeSelf && ball.activeSelf)
        {
            float pangle = angle360(ball.transform.position, rhino.transform.position, ball.transform.right);
            Vector3 relativePos = ball.transform.position - rhino.transform.position;
            Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
            float temp = -1.0f;
            if (rotation.w < 0)
                temp = 1.0f;
            rhino.GetComponent<RhinoCharacter>().turnSpeed = temp;
            StartCoroutine("Stop_turn");
        }
        yield return null;
    }

    IEnumerator Stop_turn()
    {
        while (true)
        {
            Vector3 fwd = rhino.transform.TransformDirection(Vector3.forward);
            float distance = Vector3.Distance(rhino.transform.position, ball.transform.position);
            Ray ray1 = new Ray(rhino.transform.position + new Vector3(0, 0.2f, 0), rhino.transform.forward);
            Debug.DrawRay(ray1.origin, ray1.direction * 1000f, Color.red);
            RaycastHit hit;
            if (Physics.Raycast(rhino.transform.position + new Vector3(0, 0.2f, 0), fwd, out hit))
            {
                Debug.Log(hit.collider.gameObject.tag);
                if (hit.collider.gameObject.tag == "ball")
                {
                    rhino.GetComponent<RhinoCharacter>().turnSpeed = 0f;
                    rhino.GetComponent<RhinoCharacter>().forwardSpeed = 1.0f;
                    StartCoroutine(go_start());
                    break;
                }
            }
            yield return new WaitForSeconds(0.01f);
        }
    }
    IEnumerator go_start()
    {
        while (true)
        {
            float dist = Vector3.Distance(rhino.transform.position, ball.transform.position);
            if (dist < 1.4f)
            {
                rhino.GetComponent<RhinoCharacter>().Hit();
                rhino.GetComponent<RhinoCharacter>().forwardSpeed = 0f;
                StartCoroutine(elephant_kick());
                break;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
    IEnumerator elephant_kick()
    {

        if (rhino.activeSelf)
        {
            if (pdist >= 2.68f && isStart == 1)
            {
                yield return new WaitForSeconds(0.5f);
            }
            if (pdist < 2.68f && isStart == 1)
            {
                float diff = pdist - 2.024f;
                float temp = 0.656f - diff;
                yield return new WaitForSeconds(temp / 0.656f + 0.5f);
            }
            if (isStart > 1)
            {
                yield return new WaitForSeconds(0.5f);
            }
            ballRigid.AddForce((rhino.transform.forward), ForceMode.Impulse);
        }
        yield return null;
    }

    protected virtual void OnTrackingFoundExtended()
    {
        Debug.Log("OnTrackingFoundExtended");
    }


    protected virtual void OnTrackingLost()
    {
        ball.SetActive(false);
    }

    public void Initialize()
    {
        ball.transform.position = ball.transform.parent.position + new Vector3(0, 0.1f, 0);
        //ballRigid.velocity = Vector3.zero;
        rhino.GetComponent<RhinoCharacter>().forwardSpeed = 0f;
        rhino.GetComponent<RhinoCharacter>().turnSpeed = 0f;
        rhino.transform.position = rhino.transform.parent.position;
        var rto = rhino.transform.rotation;
        rto.y = 90;
        rhino.transform.rotation = rto;
    }
    void Update()
    {
        if ((!rhino.activeSelf || !ball.activeSelf) && Global.rhino_mode && isStart != 0)
        {
            Initialize();
            init = 1;
            Global.rhino_mode = false;
        }
        if (rhino.activeSelf && ball.activeSelf && init == 1)
        {
            init = 0;
            Global.rhino_mode = true;
            StartCoroutine(elephant_turn());
        }
    }
    #endregion // PROTECTED_METHODS

}
