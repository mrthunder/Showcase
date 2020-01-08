using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class RPGAttributes
{
    [BoxGroup("Health"),SerializeField, MaxValue("_maxHealthPoints")]
    private uint _currentHealthPoints = 0;
    [BoxGroup("Health"), SerializeField]
    private uint _maxHealthPoints = 0;

    [BoxGroup("Attack"),SerializeField,ReadOnly]
    private uint _currentAttack = 10;
    [BoxGroup("Attack"),SerializeField,OnValueChanged("OnAttackChange")]
    private uint _maxAttack = 10;

    [BoxGroup("Defense"),SerializeField,ReadOnly]
    private uint _currentDefense = 10;
    [BoxGroup("Defense"),SerializeField, OnValueChanged("OnDefenseChange")]
    private uint _maxDefense = 10;

    private void Awake()
    {
        _currentHealthPoints = _maxHealthPoints;
        _currentAttack = _maxAttack;
        _currentDefense = _maxDefense;
    }
       
    /// <summary>
    /// Reduce the health points by the damage less the defense
    /// </summary>
    /// <param name="damage">how much hp to reduce</param>
    /// <param name="didDied">did they died after the damage</param>
    public void Damage(uint damage, out bool didDied)
    {
        var currentDamage = damage - _currentDefense;
        currentDamage = (uint)Mathf.Clamp(currentDamage, 0, _currentHealthPoints);
        didDied = (currentDamage == _currentHealthPoints);
        _currentHealthPoints -= currentDamage;
    }

    public void Heal(uint amount)
    {
        uint healingPoints = (uint)Mathf.Clamp(amount, 0, _maxHealthPoints);
        _currentHealthPoints += healingPoints;
    }

    #region Odin
    public void OnAttackChange()
    {
        _currentAttack = _maxAttack;
    }

    public void OnDefenseChange()
    {
        _currentDefense = _maxDefense;
    }
    #endregion

}
