using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : IState
{
    Hunter _hunter;
    FiniteStateMachine _fsm;
    Vector3 _velocity;
    private float _maxForce;
    private float _viewDistance;

    public ChaseState(Hunter hunter, FiniteStateMachine fsm, float maxForce, float vDist)
    {
        _hunter = hunter;
        _fsm = fsm;
        _maxForce = maxForce;
        _viewDistance = vDist;
    }

    public void OnStart()
    {
        Debug.Log("Change State to: " + _fsm.CurrentState);
        _hunter.LoseEnergy();
    }

    public void OnUpdate()
    {
        CheckBounds();

        AddForce(Pursuit());
        _hunter.transform.position += _velocity * Time.deltaTime;
        _hunter.transform.forward = _velocity;
    }

    Vector3 Seek(Vector3 targetPos)
    {
        Vector3 desired = targetPos - _hunter.transform.position;
        desired.Normalize();
        desired *= _hunter.GetSpeed;
        Vector3 steering = desired - _velocity;
        steering = Vector3.ClampMagnitude(steering, _maxForce);

        return steering;
    }

    Vector3 Pursuit()
    {
        Vector3 futurePos = Vector3.zero;
        if (_hunter.HunterTarget != null)
        {
            // Posicion + Velocity * Time    con o sin tiempo depende de lo que queremos
            Vector3 dist = _hunter.HunterTarget.transform.position - _hunter.transform.position;
            if (dist.magnitude > _viewDistance)
            {
                _hunter.HunterTarget = null;
                _fsm.ChangeState(HunterStates.Patrol);
                return futurePos;
            }
            futurePos = _hunter.HunterTarget.transform.position + _hunter.HunterTarget.GetVelocity * Time.deltaTime;
            Debug.DrawLine(_hunter.HunterTarget.transform.position, futurePos, Color.white);
            return Seek(futurePos);
        }
        return futurePos;
    }

    void AddForce(Vector3 force)
    {
        _velocity += force;
        _velocity = Vector3.ClampMagnitude(_velocity, _hunter.GetSpeed);
    }

    public void OnExit()
    {

    }

    void CheckBounds()
    {
        _hunter.transform.position = GameManager.instance.ApplyBounds(_hunter.transform.position);
    }
}
