using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

[Serializable]
public class OscillStateMachine : Oscillation
{
    [SerializeField] bool startFromCenter = true;

    Vector3 startPosition;
    public Vector3 StartPos => startPosition;
    Vector3 Offset = Vector3.zero;
    public Vector3 WorldPos { get; private set; }
    public bool UseGlobal => useGlobal;
    public Action onHalfPeriodFinished;
    private bool directionBounce = false;
    float periodTimer = 0;

    Vector3 oldPosition = Vector3.zero;
    Vector3 oldDelta = Vector3.zero;

    public OscillStateMachine(Transform t) : base(t)
    {
    }
    public override void Init()
    {
        startPosition = oscillator.localPosition;
        WorldPos = oscillator.position;
        Debug.Log(startPosition + ", over " + distance);
        time = 0; 
        periodTimer = 0;
    }
    public void StartAtTime(float t)
    {
        time = t;
        updateTime = true;
    }
    public override void Update()
    {
        if (updateTime)
        {
            time += Time.deltaTime;
        }
        SetPosition();
    }
    public void Begin() => updateTime = true;
    public void Pause()
    {
        updateTime = false;
        SetPosition();
    }
    public override void Release()
    {
        Pause();
        oscillator = null;
    }
    public float Value(int d)
    {
        float v = 0;
        switch(d)
        {
            case 0:
                v = (xAxis ? (Oscillate(time, speed.x, distance.x) + distance.x) / (distance.x * 2) : 0);
                break;
            case 1:
                v = (yAxis ? (Oscillate(time, speed.y, distance.y) + distance.y) / (distance.y * 2) : 0);
                break;
            case 2: 
                v = (zAxis ? (Oscillate(time, speed.z, distance.z) + distance.z) / (distance.z * 2) : 0);
                break;
            default:
                return -1;
        }
        return v;
    }
    private void SetPosition()
    {
        float x = (xAxis ? Oscillate(time, speed.x, distance.x) : 0);
        float y = (yAxis ? Oscillate(time, speed.y, distance.y) : 0);
        float z = (zAxis ? Oscillate(time, speed.z, distance.z) : 0);

        oscillator.localPosition = startPosition + new Vector3(x, y, z);
    }
    public static bool[] ReadAxis(OscillationAxis flags)
    {
        bool[] rv = new bool[3];
        if(flags.HasFlag(OscillationAxis.x))
            rv[0] = true;
        if (flags.HasFlag(OscillationAxis.y))
            rv[1] = true;
        if (flags.HasFlag(OscillationAxis.z))
            rv[2] = true;
        return rv;
    }
    public void SetOscillator(Transform t) => oscillator = t;
    public void SetOtherPosCloseToAxis(Transform other, OscillationAxis attatchToClosestPointOnAxis, Transform host = null) //bitwise 000 reads as xyz 1 meanding ON and 0 meaning OFF
    {
        bool[] axisToLinkTo = ReadAxis(attatchToClosestPointOnAxis);// {x,y,z}
        int axisCount = axisToLinkTo.Where(p => p == true).Count();
        if (host)
            oscillator = host;

        switch (axisCount)
        {
            case 0:
                break;
            case 1:
                for (int i = 0; i < axisToLinkTo.Count(); i++)
                    if (axisToLinkTo[i])
                    {
                        time = 0;
                        float distance = Mathf.Infinity;
                        float timeWithShortestDistance = 0f;
                        float timeIncraments = 0.1f;
                        Vector3 inc = startPosition;
                        do
                        {
                            time += timeIncraments;
                            inc = UpdateOscillationAxis(i, distance, inc);
                            float currDist = Vector3.Distance(inc, other.position);
                            if (currDist < distance)
                            {
                                distance = currDist;
                                timeWithShortestDistance = time;
                            }
                        } while (time / distance / 2 < distance * Mathf.PI);
                        time = timeWithShortestDistance;
                        inc = UpdateOscillationAxis(i, distance, inc);
                        other.transform.position = inc;
                        break;
                    }
                break;
            case 2:
                throw new System.NotImplementedException();
                break;
            case 3:
                throw new System.NotImplementedException();
                break;
            default:
                Debug.LogError("AxisCount out of range: " + axisCount);
                break;
        }
        Vector3 UpdateOscillationAxis(int i, float distance, Vector3 inc)
        {
            if (i == 0)
                inc.x = Oscillate(time, speed[i], distance);
            else if (i == 1)
                inc.y = Oscillate(time, speed[i], distance);
            else if (i == 2)
                inc.z = Oscillate(time, speed[i], distance);
            return inc;
        }
    }

    public float Period(float _speed = 1) => (2f / _speed) * (Mathf.PI * Mathf.PI);
    public float Oscillate(float time, float _speed = 1, float _dist = 1) => startFromCenter ? Mathf.Sin(time * _speed / Mathf.PI) * _dist : Mathf.Cos(time * _speed / Mathf.PI) * _dist;
    public float OscillateAbsolute(float time, float _speed = 1, float _dist = 1)
    {
        if(startFromCenter)
        {
            float fixedTime = time / (Period(_speed));
            
            //if within first slope of Sine Wave
            if(fixedTime < (Period(_speed)/4))
            {
                return (2 * _dist / (Mathf.PI * Mathf.PI)) * _speed * fixedTime;
            }
            // if within secondary falling slope
            else if( fixedTime < (Period(_speed)*3/4))
            {
                return -(2 * _dist / (Mathf.PI * Mathf.PI)) * _speed * fixedTime + 2 * _dist;
            }
            //if on final slope
            else
            {
                return (2 * _dist / (Mathf.PI * Mathf.PI)) * _speed * fixedTime - 4 * _dist;
            }
        }
        else
        {
            Debug.LogWarning("OscillateAbsolute not setup to start from the edges!!!");
            return 0;
        }
    }
    internal void SetSpeedAndDist(Vector3 speed, Vector3 distance)
    {
        this.speed = speed;
        this.distance = distance;
        xAxis = !(this.speed.x == 0 || this.distance.x == 0);
        yAxis = !(this.speed.y == 0 || this.distance.y == 0);
        zAxis = !(this.speed.z == 0 || this.distance.z == 0);
    }
}

[Serializable]
public abstract class Oscillation
{
    [Flags]
    public enum OscillationAxis
    {
        x = 1 << 0,
        y = 1 << 1,
        z = 1 << 2
    }
    [SerializeField]
    protected float time = 0;
    protected bool xAxis = true;
    protected bool yAxis = true;
    protected bool zAxis = true;
    public bool[] AxisActive => new bool[] { xAxis, yAxis, zAxis };
    public Vector3 Distance => new Vector3(distance.x, distance.y, distance.z);
    protected bool useGlobal = false;
    protected bool updateTime = false;
    protected Transform oscillator = null;
    protected Vector3 speed = Vector3.one;
    protected Vector3 distance = Vector3.one;
    public abstract void Init();
    public abstract void Update();
    public abstract void Release();
    public Oscillation(Transform t) => oscillator = t;
}