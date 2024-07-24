using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunter : MonoBehaviour
{
    [SerializeField] private float   _speed                   = 5;
    [SerializeField] private float   _rechargeEnergyCooldown; 
    [SerializeField] private float   viewDistance             = 1f;
    [SerializeField, Range(0,30)]    private float   _maxEnergy;
    [SerializeField, Range(0,10)]    private float   _energyPerAction;
    [SerializeField, Range(0, 0.1f)] private float   maxForce;   
    private float   _currentEnergy;          
    private Boid    _target;
    public  Boid    HunterTarget { get { return _target; } set { _target = value; } }
    public  float   GetSpeed                 => _speed;
    private FiniteStateMachine _fsm;
    public  List<Transform> allWaypoints = new List<Transform>();
    //private Vector3 _velocity;
    public  Vector3 GetVelocity() 
    {
        return transform.forward * _speed;
    }
    
    void Start()
    {
        SetMaxEnergy();

        _fsm = new FiniteStateMachine();
        _fsm.AddState(HunterStates.Patrol, new PatrolState(this, _fsm, viewDistance));
        _fsm.AddState(HunterStates.Chase, new ChaseState(this, _fsm, maxForce, viewDistance));
        _fsm.AddState(HunterStates.Idle, new IdleState(this, _fsm, _rechargeEnergyCooldown));
        _fsm.ChangeState(HunterStates.Patrol);
    }

    void Update()
    {
        _fsm.Update();
    }

    public void LoseEnergy()
    {
        if (_currentEnergy <= 0) _fsm.ChangeState(HunterStates.Idle);
        _currentEnergy -= _energyPerAction;
        if((_currentEnergy + 5) == 0) Debug.Log("Out of energy");
    }

    public void SetMaxEnergy()
    {
        _currentEnergy = _maxEnergy;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, viewDistance);
    }
}
