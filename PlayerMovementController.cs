using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private MovementVariables _movementVariables;
    private bool _dashing = false;
    private bool _coolingDown = false;
    private Vector2 _movement = new Vector2(0,0);
    private float _dashSpeed;
    private float _dashLength;
    private float _dashCooldown;
    private float _speed;

    private void Awake()
    {
        UpdateStats();
    }

    void FixedUpdate()
    {
        if(!_dashing)
            _rigidbody.velocity = _movement;
    }

    public void UpdateStats()
    {
        _dashSpeed = _movementVariables.DashSpeed.Value;
        _dashCooldown = _movementVariables.DashCooldown.Value;
        _dashLength = _movementVariables.DashLength.Value;
        _speed = _movementVariables.PlayerSpeed.Value;
    }
    
    public void OnDash()
    {
        if (!_dashing && !_coolingDown)
        {
            StartCoroutine(Dash());
        }
    }
    
   private IEnumerator Dash()
   {
       _dashing = true;
       _rigidbody.velocity = Vector2.ClampMagnitude(_rigidbody.velocity, 1) * _dashSpeed; 
       yield return new WaitForSecondsRealtime(_dashLength);  
       _dashing = false;
       _coolingDown = true;
       yield return new WaitForSecondsRealtime(_dashCooldown);
       _coolingDown = false; 

   }

   public void OnMove(InputAction.CallbackContext ctx)
   {
       if(ctx.started || ctx.canceled)
           _movement = ctx.ReadValue<Vector2>() * _speed;     
   }
}
