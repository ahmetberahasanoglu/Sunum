using UnityEngine;

public class Hitbox : MonoBehaviour
{
    // Bu hitbox kime bağlı? (Ana EnemyAI scripti)
    public EnemyAI mainAI;
    
    // Vuruş çarpanı (Kafa ise 2x hasar gibi)
    public float damageMultiplier = 1f;

    public void OnHit(float damage)
    {
        // Hasarı ana beyne ilet
        mainAI.TakeDamage(damage * damageMultiplier);
    }
}