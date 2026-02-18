using UnityEngine;

public interface IDamagable
{
    int MaxHealth { get; set; }
    int CurrentHealth { get; set; }

    void TakeDamage(int damageAmount);

    void Heal(int healAmount);

    void CheckForDeath();
   
}
