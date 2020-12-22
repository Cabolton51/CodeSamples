using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu] 
public class AttackVariables : ScriptableObject
{
   [SerializeField] public FloatVariable Damage;
   [SerializeField] public FloatVariable ShotInterval;
   [SerializeField] public FloatVariable ShotVelocity;
}
