using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyHealth : Health
{
    protected override void Death()
    {
        Destroy(gameObject);
    }
}
