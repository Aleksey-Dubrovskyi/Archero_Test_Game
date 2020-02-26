using UnityEngine;

public class DamageManager : MonoBehaviour
{
    public static DamageManager instance;
    public int shellDamage;
    public int meleDamage;
    int currentHP;


    private void Awake()
    {
        instance = this;
    }

    public void TakeShellDamage(int unitHP)
    {
        currentHP = unitHP;
        currentHP -= shellDamage;
    }

    public void TakeMeleDamage(int unitHP)
    {
        currentHP = unitHP;
        currentHP -= meleDamage;
    }

    public int UpdateHpInfo()
    {
        return currentHP;
    }
}
