using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArriveAgent : MonoBehaviour
{

    public float maxSpeed;

    [Range(0, 0.1f)]
    public float maxForce;

    private Vector3 _velocity;

    public Transform arriveTarget;
    public float arriveRadius;


    // Update is called once per frame
    void Update()
    {
        AddForce(Arrive());

        transform.position += _velocity * Time.deltaTime;
        transform.forward = _velocity;

    }

    Vector3 Arrive()
    {
        Vector3 desired = (arriveTarget.position - transform.position);

        Vector3 distance = desired;

        if(distance.magnitude < arriveRadius)
        {
            desired.Normalize();
            desired *= maxSpeed * (distance.magnitude / arriveRadius);
        }
        else
        {
            desired.Normalize();
            desired *= maxSpeed;
        }

        

        Vector3 steering = desired - _velocity;
        steering = Vector3.ClampMagnitude(steering, maxForce);

        return steering;
    }

    void AddForce(Vector3 force)
    {
        _velocity += force;
        _velocity = Vector3.ClampMagnitude(_velocity, maxSpeed);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(arriveTarget.transform.position, arriveRadius);
    }

}
