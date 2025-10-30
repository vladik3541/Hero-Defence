using System;

public class UnitHealth : Health
{
    private void OnEnable()
    {
        ResetHealth();
    }

    protected override void Death()
    {
        gameObject.SetActive(false);
    }

    public bool IsDead()
    {
        return currentHealth <= 0f;
    }
}
