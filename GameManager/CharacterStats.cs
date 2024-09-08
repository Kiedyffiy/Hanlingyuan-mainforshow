using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public CharacterData_SO templateData;
    public CharacterData_SO CharacterData;
    public AttackData_SO AttackData;
    private void Awake()
    {
        if (templateData != null)
        {
            CharacterData = Instantiate(templateData);
        }
    }
    public int maxHP 
    {
        get
        {
            if(CharacterData != null)
                return CharacterData.maxHP;
            else 
                return 0;
        }
        set
        {
            CharacterData.maxHP = value;
        }
    }

    public int currentHP
    {
        get
        {
            if (CharacterData != null)
                return CharacterData.currentHP;
            else
                return 0;
        }
        set
        {
            CharacterData.currentHP = value;
        }
    }

    public float defence
    {
        get
        {
            if (CharacterData != null)
                return CharacterData.defence;
            else
                return 0;
        }
        set
        {
            CharacterData.defence = value;
        }

    }

    public float[] Damage
    {
        get
        {
            if (AttackData != null)
                return AttackData.Damage;
            else
                return null;
        }
        
    }

    public void TakeDamage(CharacterStats attacker, int attackType, CharacterStats defender)
    {
        int damage = (int)(attacker.Damage[attackType] * (1 - defender.defence));

        currentHP = Mathf.Max(currentHP - damage, 0);

        //TODO: Update UI
    }

}
