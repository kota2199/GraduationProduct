using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AICarController : MonoBehaviour
{
    [SerializeField]
    private GameObject manager;

    private CountDown countDown;

    private BatterySystem batterySystem;
    public Rigidbody RB { get; private set; }
    [SerializeField] Transform CenterOfMass;
    [SerializeField] WheelCollider[] Wheel;
    [SerializeField] Transform[] Obj;

    [SerializeField] float AccelPower = 1000f;
    [SerializeField] float HandleAngle = 45f;
    [SerializeField] float BrakePower = 1000f;
    [SerializeField] float throttle = 0.0f;

    private float steerAngle, brakeTorque;

    [SerializeField] float[] DriveWheels = new float[] { 1.0f, 1.0f, 1.0f, 1.0f };
    [SerializeField] float[] SteerWheels = new float[] { 0f, 0f, 1.0f, 1.0f };
    [SerializeField] float SpeedHandleLimit = 1.0f;

    [SerializeField]
    private Transform wayPointsParent;
    
    [SerializeField]
    private Transform[] waypoints;         // ?E?F?C?|?C???g???????????z??

    private int currentWaypointIndex = 0; // ???????E?F?C?|?C???g?C???f?b?N?X

    private float addAcceleAmount = 1;
    private int remainAddAccele = 3;
    private float restrictor;

    // Start is called before the first frame update
    void Awake()
    {
        countDown = manager.GetComponent<CountDown>();

        batterySystem = GetComponent<BatterySystem>();

        Wheel = GetComponentsInChildren<WheelCollider>();
        RB = GetComponent<Rigidbody>();
        RB.centerOfMass = CenterOfMass.localPosition;

        Obj = new Transform[Wheel.Length];
        for (int i = 0; i < Wheel.Length; i++)
        {
            Obj[i] = Wheel[i].transform.GetChild(0);
        }

        waypoints = new Transform[wayPointsParent.childCount];

        // 0?`????-1???????q?????????z?????i?[
        for (int i = 0; i < waypoints.Length; ++i)
        {
            waypoints[i] = wayPointsParent.GetChild(i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        float _sh = RB.velocity.magnitude * SpeedHandleLimit;
        restrictor = batterySystem.restrictor;
        if (countDown.isPlay)
        {
            steerAngle = CalculateSteering(); // ?X?e?A?????O???p?x???v?Z????????
            brakeTorque = CalculateBrakeTorque(); // ?u???[?L?g???N???v?Z
        }

        for (int i = 0; i < Wheel.Length; i++)
        {
            if (GetComponent<BatterySystem>().remainBattery > 0)
            {
                Wheel[i].steerAngle = steerAngle * SteerWheels[i];
                Wheel[i].motorTorque = throttle * DriveWheels[i] * AccelPower * addAcceleAmount * (restrictor / 4);
                Wheel[i].brakeTorque = brakeTorque;
            }

            Vector3 _pos;
            Quaternion _dir;
            Wheel[i].GetWorldPose(out _pos, out _dir);
            Obj[i].position = _pos;
            Obj[i].rotation = _dir;
        }


        if (throttle > 0)
        {
            GetComponent<BatterySystem>().remainBattery -= 0.8f * restrictor * Time.deltaTime;
        }

        if (throttle <= 0)
        {
            if (RB.velocity.magnitude > 0)
            {
                if (RB.velocity.magnitude * 2 >= 10)
                {
                    GetComponent<BatterySystem>().remainBattery += 2f * restrictor * Time.deltaTime;
                }
            }
        }

        UpdateWaypointIndex(); // ?E?F?C?|?C???g?C???f?b?N?X???X?V
    }

    private float CalculateSteering()
    {
        Vector3 nextWaypoint = GetNextWaypoint(); // ?????E?F?C?|?C???g?????u??????
        Vector3 directionToWaypoint = nextWaypoint - transform.position;
        Vector3 forward = transform.forward;
        float steeringAngle = Vector3.SignedAngle(forward, directionToWaypoint, Vector3.up);
        return Mathf.Clamp(steeringAngle, -HandleAngle, HandleAngle);
    }

    private float CalculateBrakeTorque()
    {
        float speed = RB.velocity.magnitude * 2;
        float requiredDeceleration = speed - GetDesiredSpeed(); // ?]???????X?s?[?h?????????X?s?[?h??

        // ???x?????????????}???????????K?v???????A?u???[?L?g???N????????
        if (requiredDeceleration > 0)
        {
            throttle = 0.0f;
            return Mathf.Clamp(requiredDeceleration / speed, 0, 1) * BrakePower;
        }
        else
        {
            throttle = 1.0f;
            return 0;
        }
    }

    private float GetDesiredSpeed()
    {
        // ???????E?F?C?|?C???g???????E?F?C?|?C???g?????u??????
        Vector3 currentWaypoint = waypoints[currentWaypointIndex].position;
        Vector3 nextWaypoint = waypoints[(currentWaypointIndex + 1) % waypoints.Length].position; // ???[?v?????????????W???????Z

        // 2?????E?F?C?|?C???g???????????v?Z
        float distance = Vector3.Distance(currentWaypoint, nextWaypoint);

        // ?????????P?????????????A???????Z?????????x?????????????????????B
        if (distance < 10f)
            return 10f; // ?Z???????????????i???F5 m/s?j
        else if (distance < 20f)
            return 20f; // ???????????????i???F10 m/s?j
        else
            return 60f; // ?????????????????i???F15 m/s?j

        // ?????????A?E?F?C?|?C???g?????p?x?i?????j?????????A?????????????????????x?????????????????????l???????????B
        // ?p?x???????????v?????????v?Z?????????????A?????????G?????W?b?N???g???????????????\?????B
    }

    private Vector3 GetNextWaypoint()
    {
        if (waypoints.Length == 0)
            return Vector3.zero; // ?E?F?C?|?C???g?????????????[???x?N?g????????

        return waypoints[currentWaypointIndex].position; // ???????E?F?C?|?C???g?????u??????
    }

    private void UpdateWaypointIndex()
    {
        if (Vector3.Distance(transform.position, waypoints[currentWaypointIndex].position) < 20f) // 5???[?g??????????????????
        {
            currentWaypointIndex++; // ?E?F?C?|?C???g?C???f?b?N?X???C???N???????g
            if (currentWaypointIndex >= waypoints.Length) // ???????E?F?C?|?C???g?????B??????
                currentWaypointIndex = 0; // ?C???f?b?N?X?????Z?b?g?i???[?v?????????j
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "AcceleArea")
        {
            if (remainAddAccele > 0)
            {
                addAcceleAmount = 1.2f;
                remainAddAccele--;
                Invoke("ReturnAcceleAmount", 5);
            }
        }
    }
    public void ReturnAcceleAmount()
    {
        addAcceleAmount = 1;
    }
}