using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager instance;
    public TextMeshProUGUI moneyText;
    private int currentMoney = 150;
    
    public int CurrentMoney => currentMoney;
    void Awake()
    {
        instance = this;
        moneyText.text = currentMoney.ToString();
    }

    public void AddMoney(int amount)
    {
        currentMoney += amount;
        moneyText.text = currentMoney.ToString();
    }

    public void RemoveMoney(int amount)
    {
        currentMoney -= amount;
        moneyText.text = currentMoney.ToString();
    }

    public bool IsEnoughMoney(int amount)
    {
        return currentMoney >= amount;
    }
}
