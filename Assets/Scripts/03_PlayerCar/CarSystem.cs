using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarSystem : MonoBehaviour
{
    [SerializeField]
    private GameObject manager;

    private CountDown countDown;

    private BatterySystem batterySystem;

    private SpeedCheck speedCheck;

    private BoostModeManager boostManager;


    public Rigidbody RB { get; private set; }
    [SerializeField] Transform CenterOfMass;
    [SerializeField] WheelCollider[] Wheel;
    [SerializeField] Transform[] Obj;

    [SerializeField] string XAxisName = "Horizontal";
    [SerializeField] string YAxisName = "Vertical";
    [SerializeField] KeyCode BrakeKey = KeyCode.Space;

    [SerializeField] Vector2 InputVector;
    [SerializeField] float Brake = 0;

    [SerializeField] float AccelPower = 1000f;
    [SerializeField] float HandleAngle = 45f;
    [SerializeField] float BrakePower = 1000f;

    [SerializeField] float[] DriveWheels = new float[] { 1.0f, 1.0f, 1.0f, 1.0f };
    [SerializeField] float[] SteerWheels = new float[] { 0f, 0f, 1.0f, 1.0f };
    [SerializeField] float SpeedHandleLimit = 1.0f;

    private float addAcceleAmount = 1;
    private int remainAddAccele = 3;
    private float restrictor;


    // Start is called before the first frame update
    void Awake()
    {
        countDown = manager.GetComponent<CountDown>();

        batterySystem = GetComponent<BatterySystem>();

        speedCheck = GetComponent<SpeedCheck>();

        boostManager = GetComponent<BoostModeManager>();

        Wheel = GetComponentsInChildren<WheelCollider>();
        RB = GetComponent<Rigidbody>();
        RB.centerOfMass = CenterOfMass.localPosition;

        Obj = new Transform[Wheel.Length];
        for (int i = 0; i < Wheel.Length; i++)
        {
            Obj[i] = Wheel[i].transform.GetChild(0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (countDown.isPlay)
        {
            InputVector = new Vector2(Input.GetAxis(XAxisName), Input.GetAxis(YAxisName));
            Brake = Input.GetKey(BrakeKey) ? BrakePower : 0f;
        }
        float _sh = RB.velocity.magnitude * SpeedHandleLimit;
        restrictor = batterySystem.restrictor;

        for (int i = 0; i < Wheel.Length; i++)
        {
            if (GetComponent<BatterySystem>().remainBattery > 0)
            {
                Wheel[i].steerAngle = InputVector.x * SteerWheels[i] * HandleAngle;
                Wheel[i].motorTorque = InputVector.y * DriveWheels[i] * AccelPower * boostManager.addBoostPower * (restrictor / 4);
                Wheel[i].brakeTorque = Brake;
            }

            Vector3 _pos;
            Quaternion _dir;
            Wheel[i].GetWorldPose(out _pos, out _dir);
            Obj[i].position = _pos;
            Obj[i].rotation = _dir;
        }

        if (InputVector.y > 0)
        {
            batterySystem.remainBattery -= 1.0f * restrictor * Time.deltaTime;
        }

        if (InputVector.y <= 0)
        {
            if (speedCheck.speed > 0)
            {
                if (speedCheck.speed >= 10)
                {
                    batterySystem.remainBattery += 2f * restrictor * Time.deltaTime;
                }
            }
        }
    }
}