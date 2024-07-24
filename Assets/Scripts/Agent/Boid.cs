using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    [SerializeField] private float maxSpeed;
    [SerializeField, Range(0, 0.1f)] private float maxForce;

    [Header("Flocking")]
    [SerializeField] private float _separationRadius = 1;
    [SerializeField] private float _arriveRadius = 1;
    [SerializeField] private float _evadeRadius = 1;
    [SerializeField] private float _viewRadius = 1;
    [SerializeField] private float _foodRadius = 1;

    [Range(0f, 5f), SerializeField] private float separationWeight = 1;
    [Range(0f, 5f), SerializeField] private float alignmentWeight = 1;
    [Range(0f, 5f), SerializeField] private float cohesionWeight = 1;
    [Range(0f, 5f), SerializeField] private float arriveWeight = 1;
    [Range(0f, 5f), SerializeField] private float evadeWeight = 1;

    private Vector3 _velocity;
    public Vector3 GetVelocity => _velocity;

    Food _foodTarget;
    Hunter _hunterTarget;

    void Start()
    {
        GameManager.instance.AddBoid(this);
        _hunterTarget = GameManager.instance.hunter;

        Vector3 randomVector = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized * maxSpeed;
        AddForce(randomVector);
    }

    void Update()
    {
        CheckBounds();
        _foodTarget = TakeFood();

        float evadeWeightTemp = evadeWeight;
        if (Vector3.Distance(_hunterTarget.transform.position, transform.position) > _evadeRadius)
        {
            evadeWeightTemp = 0;
        }
        AddForce(Separation() * separationWeight);
        AddForce(Cohesion() * cohesionWeight);
        AddForce(Alignment() * alignmentWeight);
        AddForce(Evade(_hunterTarget.transform.position) * evadeWeightTemp);
        AddForce(Arrive() * arriveWeight);
        transform.position += _velocity * Time.deltaTime;
        transform.forward = _velocity;
    }

    Vector3 Separation()
    {
        Vector3 desired = Vector3.zero;

        foreach (var boid in GameManager.instance.allBoids)
        {
            Vector3 dist = boid.transform.position - transform.position;
            if (dist.magnitude <= _separationRadius)
                desired += dist;
        }
        if (desired == Vector3.zero) return desired;
        desired = -desired;

        return CalculateSteering(desired);
    }

    Vector3 Alignment()
    {
        Vector3 desired = Vector3.zero;
        int count = 0;
        foreach (var boid in GameManager.instance.allBoids)
        {
            if (boid == this) continue;
            if (Vector3.Distance(boid.transform.position, transform.position) <= _viewRadius)
            {
                desired += boid._velocity;
                count++;
            }
        }
        if (count == 0) return desired;
        desired /= count;

        return CalculateSteering(desired);
    }

    Vector3 Seek(Vector3 targetPos)
    {
        Vector3 desired = targetPos - transform.position;
        desired.Normalize();
        desired *= maxSpeed;
        Vector3 steering = desired - _velocity;
        steering = Vector3.ClampMagnitude(steering, maxForce);

        return steering;
    }

    Vector3 Pursuit(Vector3 target)
    {
        Vector3 futurePos = target + _hunterTarget.GetVelocity() * Time.deltaTime;
        Debug.DrawLine(target, futurePos, Color.white);
        return Seek(futurePos);
    }

    Vector3 Evade(Vector3 target)
    {
        return -Pursuit(target);
    }

    Vector3 Cohesion()
    {
        Vector3 desired = Vector3.zero;
        int count = 0;
        foreach (var boid in GameManager.instance.allBoids)
        {
            if (boid == this) continue;
            if (Vector3.Distance(transform.position, boid.transform.position) <= _viewRadius)
            {
                desired += boid.transform.position;
                count++;
            }
        }
        if (count == 0) return desired;
        desired /= count;
        desired -= transform.position;

        return CalculateSteering(desired);
    }

    Vector3 Arrive()
    {
        Vector3 desired = Vector3.zero;
        if (_foodTarget != null)
        {
            desired = (_foodTarget.transform.position - transform.position);
            Vector3 distance = desired;
            if (distance.magnitude < _arriveRadius)
            {
                if (desired.magnitude <= 1f)
                {
                    _foodTarget.Destroy();
                }
                desired.Normalize();
                desired *= maxSpeed * (distance.magnitude / _arriveRadius);
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
        return desired;
    }

    Vector3 CalculateSteering(Vector3 desired)
    {
        return Vector3.ClampMagnitude((desired.normalized * maxSpeed) - _velocity, maxForce);
    }

    void CheckBounds()
    {
        transform.position = GameManager.instance.ApplyBounds(transform.position);
    }

    void AddForce(Vector3 force)
    {
        _velocity = Vector3.ClampMagnitude(_velocity + force, maxSpeed);
    }

    Food TakeFood()
    {
        _foodTarget = null;

        if (GameManager.instance.AllFoodCount() > 0)
        {
            foreach (var food in GameManager.instance.allFood)
            {
                if (food == null) continue;
                Vector3 distance = food.transform.position - transform.position;

                if (distance.magnitude < _foodRadius)
                {
                    _foodTarget = food;
                    return _foodTarget;
                }
            }
        }
        return _foodTarget;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, _separationRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _foodRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _evadeRadius);
    }
}
