using UnityEngine;
using System;

public static class BattleUtility {
    private static float HitChance;
    private static float Precision;
    private static float DodgeChance;
    private static float CriticalChance;
    private static float CriticalDamage;

    public static bool CalculateHit() {
        if(DodgeChance == 0.0) {
            return true;
        }
        else if(DodgeChance == 1.0) {
            return false;
        }
        float ChanceToHit = (1f - DodgeChance) + (Precision * 0.01f);
        int Hit = (int)(ChanceToHit*100);
        return UnityEngine.Random.Range(0,100) < Hit;
    }

    public static float CalculateDamage(float BaseDamage) {
        float Damage = BaseDamage;
        if(UnityEngine.Random.Range(0,100) < CriticalChance) {
            Damage *= CriticalDamage;
        }
        return Damage;
    }
}