using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState{
    GUARD, PATROL, CHASE, DEAD
};

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour
{
    private EnemyState _enemyState;

    private NavMeshAgent _agent;
    private Animator _anim;
    private bool isWalk;
    private bool isChase;
    private bool isFollow;

    private GameObject _attackTarget;

    private float _speed;

    [Header("Basic Settings")]
    public float sightRadius;
    public bool isGuard;


    void Awake() {
        _agent = GetComponent<NavMeshAgent>(); 
        _anim  = GetComponent<Animator>();
        _speed = _agent.speed;
    }

    void Update() {
        SwitchState();
        SwitchAnimation();
    }

    void SwitchState() {
        switch (_enemyState) {
            case EnemyState.GUARD:
                ActionGuard();
                break;
            case EnemyState.PATROL:
                ActionPatrol();
                break;
            case EnemyState.CHASE:
                ActionChase();
                break;
            case EnemyState.DEAD:
                ActionDead();
                break;
        }
    }

    void SwitchAnimation() {
        _anim.SetBool("Walk", isWalk);
        _anim.SetBool("Chase", isChase);
        _anim.SetBool("Follow", isFollow);
    }

    void ActionGuard() {
        // TODO: ActionGuard
        if (FoundPlayer()) {
            _enemyState = EnemyState.CHASE;
        }
    }

    void ActionPatrol() {
        // TODO: ActionPatrol
        if (FoundPlayer()) {
            _enemyState = EnemyState.CHASE;
        }
    }

    void ActionChase() {
        // TODO: chase
        _speed = _agent.speed;
        // TODO: attack
        isWalk = false;
        isChase = true;
        if (!FoundPlayer()) {
            // TODO: back to original state
            isFollow = false;
            _agent.destination = transform.position;
        } else {
            isFollow = true;
            _agent.destination = _attackTarget.transform.position;
        }
    }

    void ActionDead() {
        // TODO: ActionDead
        if (FoundPlayer()) {
            _enemyState = EnemyState.CHASE;
        }
    }

    bool FoundPlayer() {
        var colliders = Physics.OverlapSphere(transform.position, sightRadius);
        foreach (var target in colliders) {
            if (target.CompareTag("Player")) {
                _attackTarget = target.gameObject;
                return true;
            }
        }

        _attackTarget = null;
        return false;
    }
}
