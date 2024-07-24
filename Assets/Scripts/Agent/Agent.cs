using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    public Transform seekTarget;
    public Transform fleeTarget;
    public float maxSpeed;

    [Range(0, 0.1f)]
    public float maxForce;
    private Vector3 _velocity;
    public float viewRadius;

    //Weights
    [Range(0f, 3f)]
    public float seekWeight;
    [Range(0f, 1f)]
    public float fleeWeight;

    //Pursuit
    public Boid pursuitTarget;
    public bool isPursuing;


    // Start is called before the first frame update
    void Start()
    {
        //  int currentHP = 80;
        //  int maxHP = 100;
        //  int hpPotion = 30;
        //
        //  currentHP += hpPotion;
        //  if (currentHP > maxHP)
        //      currentHP = maxHP;
        //
        //  //  currentHP = (currentHP + hpPotion) > maxHP ? maxHP : currentHP + hpPotion;
        //  currentHP = Mathf.Clamp(currentHP, 0, maxHP);

    }

    // Update is called once per frame
    void Update()
    {
        if (isPursuing)
        {
            AddForce(Pursuit());
            transform.position += _velocity * Time.deltaTime;
            transform.forward = _velocity;
            return;
        }

        bool inFleeRange = (fleeTarget.position - transform.position).magnitude <= viewRadius;
        seekWeight = inFleeRange ? 0 : 1;
        fleeWeight = inFleeRange ? 1 : 0;

        //Combined Behaviors
        AddForce(Flee(fleeTarget.position) * fleeWeight);
        AddForce(Seek(seekTarget.position) * seekWeight);

        transform.position += _velocity * Time.deltaTime;
        transform.forward = _velocity;
    }

    Vector3 Pursuit()
    {
        // Posicion + Velocity * Time    con o sin tiempo depende de lo que queremos
        Vector3 futurePos = pursuitTarget.transform.position + pursuitTarget.GetVelocity * Time.deltaTime;
        Debug.DrawLine(pursuitTarget.transform.position, futurePos, Color.white);
        return Seek(futurePos);
    }

    Vector3 Evade()
    {
        return -Pursuit();
    }

    //Steering Behavior - Seek
    Vector3 Seek(Vector3 targetPos)
    {
        Vector3 desired = targetPos - transform.position;
        desired.Normalize();
        desired *= maxSpeed;

        Vector3 steering = desired - _velocity;
        steering = Vector3.ClampMagnitude(steering, maxForce);

        return steering;
    }

    Vector3 Flee(Vector3 targetPos)
    {
        return -Seek(targetPos);
    }

    void AddForce(Vector3 force)
    {
        _velocity += force;
        _velocity = Vector3.ClampMagnitude(_velocity, maxSpeed);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, viewRadius);
    }
}
