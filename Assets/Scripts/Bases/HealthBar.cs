using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private Image healthBar;

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(Camera.main.transform);
    }

    public void UpdateHealthBar (float health)
    {
        healthBar.fillAmount = health;
    }
}
