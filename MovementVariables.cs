using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MovementVariables : ScriptableObject
{
    [SerializeField] public FloatVariable DashLength;
    [SerializeField] public FloatVariable DashCooldown;
    [SerializeField] public FloatVariable DashSpeed;
    [SerializeField] public FloatVariable PlayerSpeed;
}
