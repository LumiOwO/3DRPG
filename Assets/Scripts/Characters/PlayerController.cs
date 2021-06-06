using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    private NavMeshAgent _agent;

    private Animator _anim;

    private GameObject _attackTarget;
    private float _lastAttackTime;

    void Awake() {
        _agent = GetComponent<NavMeshAgent>();
        _anim = GetComponent<Animator>();
    }

    void Start() {
        MouseManager.instance.OnMouseClicked += EventMove;
        MouseManager.instance.OnEnemyClicked += EventAttack;
    }

    void Update() {
        SwitchAnimation();

        _lastAttackTime -= Time.deltaTime;
    }

    void SwitchAnimation() {
        _anim.SetFloat("Speed", _agent.velocity.sqrMagnitude);
    }

    void EventMove(Vector3 target) {
        StopAllCoroutines();
        _agent.isStopped = false;
        _agent.destination = target;
    }

    void EventAttack(GameObject target)
    {
        if (target == null) {
            return;
        }
        _attackTarget = target;
        StartCoroutine(MoveToAttackTarget());
    }

    IEnumerator MoveToAttackTarget() {
        _agent.isStopped = false;

        transform.LookAt(_attackTarget.transform);

        // TODO: attack distance
        while(Vector3.Distance(_attackTarget.transform.position, transform.position) > 1) {
            _agent.destination = _attackTarget.transform.position;
            yield return null;
        }

        _agent.isStopped = true;
        
        // Attack
        if (_lastAttackTime < 0) {
            _anim.SetTrigger("Attack");
            // TODO: reset CD time
            _lastAttackTime = 0.5f;
        }
    }
}
