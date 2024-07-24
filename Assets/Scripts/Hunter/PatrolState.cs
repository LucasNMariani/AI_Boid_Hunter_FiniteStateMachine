using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IState
{
    Hunter _hunter;
    FiniteStateMachine _fsm;
    int _dir = 1;
    float _viewDistance;
    int _currentWaypoint = 0;

    public PatrolState(Hunter hunter, FiniteStateMachine fsm, float vDist)
    {
        _hunter = hunter;
        _fsm = fsm;
        _viewDistance = vDist;
    }

    public void OnStart()
    {
        Debug.Log("Change State to: " + _fsm.CurrentState);
    }

    public void OnUpdate()
    {
        Patrol();
        GetBoid();
    }

    void Patrol()
    {
        Transform waypoint = _hunter.allWaypoints[_currentWaypoint]; 
        Vector3 dir = waypoint.position - _hunter.transform.position;
        dir.y = 0;

        _hunter.transform.forward = dir;
        _hunter.transform.position += _hunter.transform.forward * _hunter.GetSpeed * Time.deltaTime;

        if (dir.magnitude <= 0.3f)
        {
            _currentWaypoint += _dir;
            if (_currentWaypoint > _hunter.allWaypoints.Count - 1 || _currentWaypoint < 0)
            {
                _dir *= -1;
                _currentWaypoint += _dir * 2;
            }
            _hunter.LoseEnergy();
        }
    }

    Boid GetBoid()
    {
        _hunter.HunterTarget = null;
        foreach (var boid in GameManager.instance.allBoids)
        {
            Vector3 dist = boid.transform.position - _hunter.transform.position;
            if (dist.magnitude <= _viewDistance)
            {
                _hunter.HunterTarget = boid;
                _fsm.ChangeState(HunterStates.Chase);
                return _hunter.HunterTarget;
            }
        }
        return _hunter.HunterTarget;
    }

    public void OnExit()
    {
        
    }
}