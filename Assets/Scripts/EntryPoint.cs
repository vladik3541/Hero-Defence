using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntryPoint : MonoBehaviour
{
    [SerializeField] private SelectedRace selectedRace;
    void Awake()
    {
        selectedRace.Initialize();
    }
}
