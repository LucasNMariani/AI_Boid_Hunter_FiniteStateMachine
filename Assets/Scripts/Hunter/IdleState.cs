using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IState
{
    FiniteStateMachine _fsm;
    Hunter _hunter;
    float _secondsToRechargeEnergy;
    float _timer;

    public IdleState(Hunter hunter, FiniteStateMachine fsm, float seconds)
    {
        _fsm = fsm;
        _hunter = hunter;
        _secondsToRechargeEnergy = seconds;
    }

    public void OnStart()
    {
        Debug.Log("Change State to: " + _fsm.CurrentState);
    }

    public void OnUpdate()
    {
        if (_timer >= _secondsToRechargeEnergy)
        {
            RechargeEnergy();
            _timer = 0;
        }
        else _timer += Time.deltaTime;
    }

    void RechargeEnergy()
    {
        _hunter.SetMaxEnergy();
        Debug.Log(_hunter.gameObject.name + " has a good rest and restored his energy to Full");
        _fsm.ChangeState(HunterStates.Patrol);
    }

    public void OnExit()
    {

    }
}
