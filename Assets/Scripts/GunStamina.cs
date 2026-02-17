using UnityEngine;

public class GunStamina : MonoBehaviour
{
    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float currentStamina;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentStamina = maxStamina;
    }

    public void UseStamina(float amount)
    {
        currentStamina -= amount;
        if (currentStamina < 0)
        {
            currentStamina = 0;
        }
    }

    public void RegenerateStamina(float amount)
    {
        currentStamina += amount;
        if (currentStamina > maxStamina)
        {
            currentStamina = maxStamina;
        }
    }

    public float GetCurrentStamina()
    {
        return currentStamina;
    }

    public bool CanFire(float amount)
    {
        return currentStamina >= amount;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
