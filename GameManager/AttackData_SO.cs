using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attack", menuName = "Character Stats/Attack")]
public class AttackData_SO : ScriptableObject
{
    public float[] Attack_Range;

    public float[] CoolDown;

    public float[] Damage;

    //数组下标为攻击的类别
}
