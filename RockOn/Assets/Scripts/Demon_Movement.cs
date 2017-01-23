﻿using UnityEngine;
using System.Collections;

public class Demon_Movement : MonoBehaviour
{
    private float _speed; // movement speed
    //private float _maxRange; // max range at which it stops chasing the target (it's too far) // no longer used
    private float _minRange; // min range at which it stops chasing the target (it's too close)

    private Transform _target; // target's position
    private Transform _enemy; // this object's position
    private Rigidbody2D _rb; // this objects's rigidbody2d
    private Animator _anim; // this object's animator
    private Demon_Attack_Range _attackScript; // for changing speed when attacking
    private float _distance; // distance between enemy and target
    private float _pushBackPower; // how strong is enemy pushed back when pick is active
    private bool _isInRange; // is enemy in range of the player's view?
    private Enemy_Audio _ea;  // C'mon! sounds

    void Start()
    {
        // initializing variables
        _target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        _enemy = GetComponent<Transform>();
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _attackScript = GetComponentInChildren<Demon_Attack_Range>();
        _ea = GetComponent<Enemy_Audio>();

        _speed = 1.5f;
        //_maxRange = 3.0f; // no longer used
        _minRange = 0.67f;
        _pushBackPower = 30.0f;
        _isInRange = false;
    }

    // called when enemy is attacked and pick power-up is active
    public void pushBack()
    {
        // calculate direction (and normalize it so it doesn't change the speed of movement)
        Vector2 direction = new Vector2(_enemy.position.x - _target.position.x, _enemy.position.y - _target.position.y).normalized;
        _rb.AddForce(direction * _speed * _pushBackPower, ForceMode2D.Impulse);
    }

    void FixedUpdate()
    {
        // calculate distance between enemy and target (player)
        _distance = Vector2.Distance(_enemy.position, _target.position);

        // chase target only within set range
        if (_isInRange && _distance >= _minRange) // && _distance <= _maxRange
        {
            // moving the object, animate
            _anim.SetBool("isMoving", true);

            StartCoroutine(Wait(_ea, 2)); //play C'mon! sound

            // calculate direction (and normalize it so it doesn't change the speed of movement)
            Vector2 direction = new Vector2(_target.position.x - _enemy.position.x, _target.position.y - _enemy.position.y).normalized;

            // move enemy according to the direction vector
            if (_attackScript.isCooldown())
            {
                // make enemy slower while cooldown is active
                _rb.AddForce(direction * _speed * 0.5f, ForceMode2D.Impulse);
            }
            else
            {
                _rb.AddForce(direction * _speed, ForceMode2D.Impulse);
            }
        }
        else
        {
            // object not moving, stop animation
            _anim.SetBool("isMoving", false);
        }
    }

    IEnumerator Wait(Enemy_Audio _ea, float delay)  // for playing "Come On!"
    {
        _ea.PlayComeOn();
        _ea.enabled = true;
        yield return new WaitForSeconds(delay);
        _ea.enabled = false;
    }

    public void setIsInRange(bool isInRange)
    {
        _isInRange = isInRange;
    }
}
