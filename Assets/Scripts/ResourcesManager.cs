using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ResourcesManager : MonoBehaviour
{
    public static ResourcesManager instans;

    [SerializeField] private TextMeshProUGUI textGold;
    [SerializeField] private TextMeshProUGUI textTree;
    [SerializeField] private TextMeshProUGUI textUnit;

    [SerializeField] private int gold, tree;

    [SerializeField] private int currentUnit;
    [SerializeField] private int maxUnit;

    private void Awake()
    {
        instans = this;
        UpdateGold();
        UpdateTree();
        UpdateUnit();
    }
    public void SpendGold(int count)
    {
        gold -= count;
        UpdateGold();

    }
    public void AddGold(int count)
    {
        gold += count;
        UpdateGold();
    }
    public void SpendTree(int count)
    {
        tree -= count;
        UpdateTree();
    }
    public void AddTree(int count)
    {
        tree += count;
        UpdateTree();
    }


    public void AddUnit(int unit)
    {
        currentUnit += unit;
        UpdateUnit();
    }
    public void RemoveUnit(int unit)
    {
        currentUnit -= unit;
        UpdateUnit();
    }
    public bool EnoughGold(int amount) => gold >= amount;
    public bool EnoughTree(int amount) => tree >= amount;
    public bool EnoughUnit(int amount) => currentUnit >= amount;
    private void UpdateGold()
    {
        textGold.text = gold.ToString();
    }
    private void UpdateTree()
    {
        textTree.text = tree.ToString();
    }
    private void UpdateUnit()
    {
        textUnit.text = currentUnit.ToString()+"/"+maxUnit.ToString();

    }
}
