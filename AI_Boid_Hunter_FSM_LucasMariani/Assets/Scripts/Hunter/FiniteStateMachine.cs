using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HunterStates
{
    Patrol,
    Chase,
    Idle
}
public class FiniteStateMachine
{
    private IState _currentState;
    public IState CurrentState => _currentState;

    private Dictionary<HunterStates, IState> _allStates = new Dictionary<HunterStates, IState>();

    public void Update()
    {
        _currentState.OnUpdate();
    }

    public void ChangeState(HunterStates state)
    {
        if (_currentState != null) _currentState.OnExit();
        _currentState = _allStates[state];
        _currentState.OnStart();
    }

    public void AddState(HunterStates key, IState value)
    {
        if (!_allStates.ContainsKey(key)) _allStates.Add(key, value);
        else _allStates[key] = value;
    }
}
