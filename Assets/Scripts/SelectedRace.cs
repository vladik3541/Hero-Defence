using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectedRace : MonoBehaviour
{
    [SerializeField] private Button race1;
    [SerializeField] private Button race2;
    [SerializeField] private Button race3;
    [SerializeField] private Button race4;
    [SerializeField] private PlacementSystem placementSystem;
    [SerializeField] private ThroneAndTowerSpawner throneAndTowerSpawner;
    [SerializeField] private WaveSpawner waveSpawner;
    public void Initialize()
    {
        race1.onClick.AddListener((() => Selected(RaceType.human)));
        race2.onClick.AddListener((() => Selected(RaceType.orc)));
        race3.onClick.AddListener((() => Selected(RaceType.undead)));
    }
    private void Selected(RaceType raceType)
    {
        placementSystem.Initialize(raceType);
        throneAndTowerSpawner.Initialize(raceType);
        waveSpawner.Initialize();
        gameObject.SetActive(false);
    }
}
