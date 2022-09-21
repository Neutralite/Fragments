using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthComponent : MonoBehaviour
{
    [SerializeField]
    private float maxHealth, currentHealth;

    private float Health
    {
        get => currentHealth;
        set
        {
            currentHealth = Mathf.Clamp(value, 0, maxHealth);
            OnHealthChanged.Invoke(currentHealth/maxHealth);

            if (currentHealth == 0)
            {
                OnDeath.Invoke();
            }
        }
    }

    private void Update()
    {
        // test 
        //if (Input.GetKeyDown(KeyCode.H))
        //{
        //    Heal(1);
        //}
        //if (Input.GetKeyDown(KeyCode.D))
        //{
        //    TakeDamage(1);
        //}
    }

    public UnityEvent OnDeath;

    public UnityEvent<float> OnHealthChanged;

    public void FullyHeal()
    {
        Health = maxHealth;
    }

    public void Heal(float value)
    {
        Health += value;
    }

    public void TakeDamage(float value)
    {
        Health -= value;
    }
}
