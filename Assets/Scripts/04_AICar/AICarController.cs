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
    private Transform[] waypoints;         // ウェイポイントを保持する配列

    private int currentWaypointIndex = 0; // 現在のウェイポイントインデックス

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

        // 0～個数-1までの子を順番に配列に格納
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
            steerAngle = CalculateSteering(); // ステアリングの角度を計算する関数
            brakeTorque = CalculateBrakeTorque(); // ブレーキトルクを計算
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

        UpdateWaypointIndex(); // ウェイポイントインデックスを更新
    }

    private float CalculateSteering()
    {
        Vector3 nextWaypoint = GetNextWaypoint(); // 次のウェイポイントの位置を取得
        Vector3 directionToWaypoint = nextWaypoint - transform.position;
        Vector3 forward = transform.forward;
        float steeringAngle = Vector3.SignedAngle(forward, directionToWaypoint, Vector3.up);
        return Mathf.Clamp(steeringAngle, -HandleAngle, HandleAngle);
    }

    private float CalculateBrakeTorque()
    {
        float speed = RB.velocity.magnitude * 2;
        float requiredDeceleration = speed - GetDesiredSpeed(); // 望ましいスピードと現在のスピード差

        // 速度が高いまたは急激に減速が必要な場合、ブレーキトルクを増やす
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
        // 現在のウェイポイントと次のウェイポイントの位置を取得
        Vector3 currentWaypoint = waypoints[currentWaypointIndex].position;
        Vector3 nextWaypoint = waypoints[(currentWaypointIndex + 1) % waypoints.Length].position; // ループするためのモジュロ演算

        // 2つのウェイポイント間の距離を計算
        float distance = Vector3.Distance(currentWaypoint, nextWaypoint);

        // ここでは単純化のために、距離が短いほど速度を下げる例を示します。
        if (distance < 10f)
            return 10f; // 短い距離では低速（例：5 m/s）
        else if (distance < 20f)
            return 20f; // 中距離では中速（例：10 m/s）
        else
            return 60f; // 長い距離では高速（例：15 m/s）

        // 実際には、ウェイポイント間の角度（曲率）を測定し、その曲率が高いほど速度をさらに落とすことが考えられます。
        // 角度やその他の要因を基に計算したい場合は、さらに複雑なロジックを組み込むことが可能です。
    }

    private Vector3 GetNextWaypoint()
    {
        if (waypoints.Length == 0)
            return Vector3.zero; // ウェイポイントがない場合はゼロベクトルを返す

        return waypoints[currentWaypointIndex].position; // 現在のウェイポイントの位置を返す
    }

    private void UpdateWaypointIndex()
    {
        if (Vector3.Distance(transform.position, waypoints[currentWaypointIndex].position) < 20f) // 5メートル以内に近づいたら
        {
            currentWaypointIndex++; // ウェイポイントインデックスをインクリメント
            if (currentWaypointIndex >= waypoints.Length) // 最後のウェイポイントに到達したら
                currentWaypointIndex = 0; // インデックスをリセット（ループする場合）
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