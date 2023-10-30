using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HandleOscillator : MonoBehaviour
{
    [SerializeField] Transform oscillatorHost;
    [SerializeField] protected OscillStateMachine oscillator;
    public OscillStateMachine Oscillator => oscillator;

    [SerializeField] bool BeginOnStart = true;
    Vector3 initailPos = Vector3.zero;
    Vector3 oldPosition = Vector3.zero;
    Vector3 oldDelta = Vector3.zero;
    [SerializeField] Vector3 distance = Vector3.one;
    public Vector3 Distance => distance;
    [SerializeField] Vector3 speed = Vector3.one;
    public Vector3 Speed => speed;

    [Serializable]
    public class OscillatorEvents
    {
        [SerializeField] public UnityEvent OnPosXEnd;
        [SerializeField] public UnityEvent OnNegXEnd;
        [SerializeField] public UnityEvent OnPosYEnd;
        [SerializeField] public UnityEvent OnNegYEnd;
        [SerializeField] public UnityEvent OnPosZEnd;
        [SerializeField] public UnityEvent OnNegZEnd;
    }
    public OscillatorEvents oscEvents;


    private void OnEnable()
    {
        Init();
    }
    private void Start()
    {
        Init();
        initailPos = oscillatorHost == null ? Vector3.zero : oscillatorHost.position;
        if (BeginOnStart)
            oscillator.Begin();
    }
    public void Update()
    {
        oscillator.SetSpeedAndDist(speed, distance);
        oldPosition = oscillatorHost.position;
        oscillator.Update();
        CheckEvents();
    }
    public void ResetToInitial() => oscillatorHost.transform.position = initailPos;
    private void CheckEvents()
    {
        float deltaX = oscillatorHost.position.x - oldPosition.x;
        float deltaY = oscillatorHost.position.y - oldPosition.y;
        float deltaZ = oscillatorHost.position.z - oldPosition.z;
        if(oldDelta == Vector3.zero)
            oldDelta = new Vector3(deltaX, deltaY, deltaZ);

        if (deltaX / oldDelta.x < 0)
        {
            if (deltaX > 0)
                oscEvents.OnNegXEnd?.Invoke();
            else if (deltaX < 0)
                oscEvents.OnPosXEnd?.Invoke();
        }
        if (deltaY / oldDelta.y < 0)
        {
            if (deltaY > 0)
                oscEvents.OnNegYEnd?.Invoke();
            else if (deltaY < 0)
                oscEvents.OnPosYEnd?.Invoke();
        }
        if (deltaZ / oldDelta.z < 0)
        {
            if (deltaZ > 0)
                oscEvents.OnNegZEnd?.Invoke();
            else if (deltaZ < 0)
                oscEvents.OnPosZEnd?.Invoke();
        }
        oldDelta.x = deltaX;
        oldDelta.y = deltaY;
        oldDelta.z = deltaZ;
    }

    protected void Init()
    {
        oscillator = new OscillStateMachine(oscillatorHost == null ? transform : oscillatorHost);
        oscillator.SetSpeedAndDist(speed, distance);
        oscillator.Init();
    }
    public float Value(int dimension) => Oscillator.Value(dimension);
    public void SetHost(Transform host) => this.oscillator.SetOscillator(host);
    public bool[] AxisActive => new bool[3] { distance.x != 0 && speed.x != 0, distance.y != 0 && speed.y != 0, distance.z != 0 && speed.z != 0 };
    public void StartNewOscillationFromCurrentPos(Vector3 _speed, Vector3 _distance)
    {
        oscillator.Pause();
        SetSpeedAndDistance(_speed, _distance);
        oscillator.Init();
        oscillator.Begin();
    }
    public void StartOscillation(Vector3 _speed, Vector3 _distance)
    {
        oscillator.Pause();
        SetSpeedAndDistance(_speed, _distance);
        oscillator.StartAtTime(0);
    }
    public void StartOscillationFromClosestPoint(Transform tran) => StartOscillationFromClosestPoint(tran.position);
    public void StartOscillationFromClosestPoint(Vector3 point)
    {
        oscillator.Pause();
        bool[] axisAvailable = oscillator.AxisActive;
        float[] time = { 0, 0, 0 };
        Vector3[] directions = { Vector3.right, Vector3.up, Vector3.forward };
        float timeIncraments = 0.01f;
        int count = 0;
        for (int i = 0; i < axisAvailable.Length; i++)
        {
            if (!axisAvailable[i])
                continue;
            count++;

            if (count > 1)
                Debug.LogWarning("Closest Point can be buggy with multiple oscillations active!");

            float closestDistance = Mathf.Infinity;
            float timeWithShortestDistance = 0f;
            Vector3 currOscPos;
            do
            {
                time[i] += timeIncraments;
                currOscPos = oscillator.StartPos + directions[i] * oscillator.Oscillate(time[i], speed[i], distance[i]);
                float currDist = Mathf.Abs(point[i] - currOscPos[i]);
                if (currDist < closestDistance)
                {
                    closestDistance = currDist;
                    timeWithShortestDistance = time[i];
                }
            } while (time[i] < oscillator.Period(speed[i]));
            time[i] = timeWithShortestDistance;
        }
        float startTime = (axisAvailable[0] ? time[0] : 1) * (axisAvailable[1] ? time[1] : 1) * (axisAvailable[2] ? time[2] : 1);
        oscillator.StartAtTime(startTime);
    }
    public void SetSpeedAndDistance(Vector3 _speed, Vector3 _distance)
    {
        speed = _speed;
        distance = _distance;
        oscillator.SetSpeedAndDist(_speed, _distance);
    }
    public Vector3 StartPos()
    {
        if (Application.isPlaying)
            return oscillator.StartPos;
        return transform.position;
    }
    public Vector3 StartWorldPos()
    {
        return oscillator.WorldPos;
    }
    public void DebugLogThing(string thing) => Debug.Log(thing);
}
