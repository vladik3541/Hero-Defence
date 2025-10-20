using System;
using UnityEngine;
[CreateAssetMenu(menuName = "WaveData")]
public class WavesData : ScriptableObject
{
    public Wave[] waves;
}
[Serializable]
public class Wave
{
    public string name;
    public EnemyData[] enemies;
}
[Serializable]
public struct EnemyData
{
    public GameObject prefabEnemy;
    public int count;
}
