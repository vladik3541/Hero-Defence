using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CharacterUnit : ScriptableObject
{
    public string nameUnit;
    public int priceBuy;
    public List<LevelUnit> levelUnits;
}

[Serializable]
public class LevelUnit
{ 
    public int level;
    public int peopleNumber;
    
    public int priceSell;
    public int priceUpgrade;

    public int damage;
    public float speed;
}

