using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Data", menuName = "Character Stats/Defence")]

public class CharacterData_SO : ScriptableObject
{
    [Header("Stats Info")]

    public int maxHP;

    public int currentHP;

    public float defence;//ºı…À¬ 
}
