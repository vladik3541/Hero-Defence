using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyHealth : Health
{
    public int bounty;
    protected override void Death()
    { 
        MoneyManager.instance.AddMoney(bounty);
        Destroy(gameObject);
    }
}
