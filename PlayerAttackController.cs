using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackController : MonoBehaviour
{
    [SerializeField] private AttackVariables _attackVariables;
    [SerializeField] private GameObject _projectile;
    float _damage;
    float _shotVelocity;
    float _shotInterval;
    private bool _shooting;
    private bool _coolingDown;
    private shootingDirections _currentDirection = shootingDirections.None;
    private List<shootingDirections> currentShootingDirectionsPressed;
    private Dictionary<shootingDirections, Vector2> shootDirectionLookUp;

    private enum shootingDirections
    {
        Up,
        Down,
        Left,
        Right,
        None
    }
    
    private void Awake()
    {
        UpdateStats();
        currentShootingDirectionsPressed = new List<shootingDirections>();
        shootDirectionLookUp = new Dictionary<shootingDirections, Vector2>();
        shootDirectionLookUp.Add(shootingDirections.Up, Vector2.up);
        shootDirectionLookUp.Add(shootingDirections.Down, Vector2.down);
        shootDirectionLookUp.Add(shootingDirections.Left, Vector2.left);
        shootDirectionLookUp.Add(shootingDirections.Right, Vector2.right);
    }

    public void UpdateStats()
    {
        _damage = _attackVariables.Damage.Value;
        _shotVelocity = _attackVariables.ShotVelocity.Value;
        _shotInterval = _attackVariables.ShotInterval.Value;
    }

    void FixedUpdate()
    {
        if (_shooting && !_coolingDown)
        {
            _coolingDown = true;
            Shoot();
        }
    }

    public void Shoot()
    {
        GameObject bullet = ObjectPool.Instance.GetPooledObject();
        bullet.transform.position = transform.position;
        bullet.SetActive(true);
        bullet.GetComponent<Rigidbody2D>().velocity = shootDirectionLookUp[_currentDirection] * _shotVelocity;
        StartCoroutine(CoolDown());
    }

    #region ShootingInputHandlers 
    //Latest Wrestle With Unity's new input system
    public void UpdateShootDirectionUP(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            currentShootingDirectionsPressed.Add(shootingDirections.Up);
        }
        else if (ctx.canceled)
        {
            currentShootingDirectionsPressed.Remove(shootingDirections.Up);
        }

        UpdateShoot();
    }

    public void UpdateShootDirectionDOWN(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            currentShootingDirectionsPressed.Add(shootingDirections.Down);
            
        }
        else if (ctx.canceled)
        {
            currentShootingDirectionsPressed.Remove(shootingDirections.Down);
        }

        UpdateShoot();
    }

    public void UpdateShootDirectionLEFT(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            currentShootingDirectionsPressed.Add(shootingDirections.Left);
        }
        else if (ctx.canceled)
        {
            currentShootingDirectionsPressed.Remove(shootingDirections.Left);
        }

        UpdateShoot();
    }

    public void UpdateShootDirectionRIGHT(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            currentShootingDirectionsPressed.Add(shootingDirections.Right);
        }
        else if (ctx.canceled)
        {
            currentShootingDirectionsPressed.Remove(shootingDirections.Right);
        }

        UpdateShoot();
    }
    

    #endregion
    
    void UpdateShoot()
    {
        if (!currentShootingDirectionsPressed.Any())
        {
            _shooting = false;
            _currentDirection = shootingDirections.None;
            return;
        }

        _shooting = true;
        _currentDirection = currentShootingDirectionsPressed.Last();
    }

    private IEnumerator CoolDown()
    {
        yield return new WaitForSecondsRealtime(_shotInterval);
        _coolingDown = false;
    }
}