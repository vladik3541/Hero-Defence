using System;
using System.Collections.Generic;
using UnityEngine;

public enum RaceType
{
    human, orc, undead, elf
}
[CreateAssetMenu]
public class UnitDataBase : ScriptableObject
{
    public List<ObjectData> humans;
    public List<ObjectData> orcs;
    public List<ObjectData> undeads;
    public List<ObjectData> elf;
    public List<BuildData> builds;
}

[Serializable]
public class ObjectData
{
    [field: SerializeField]
    public string Name { get; private set; }
    [field: SerializeField]
    public int ID { get; private set; }
    [field: SerializeField]
    public Vector2Int Size { get; private set; }
    [field: SerializeField]
    public GameObject Prefab { get; private set; }
    [field: SerializeField]
    public int Price { get; private set; }
}
[Serializable]
public class BuildData
{
    [field: SerializeField]
    public RaceType Type { get; private set; }
    [field: SerializeField]
    public GameObject Throne { get; private set; }
    [field: SerializeField]
    public GameObject Tower { get; private set; }
}

