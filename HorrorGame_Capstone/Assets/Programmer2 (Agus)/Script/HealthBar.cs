using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private HealthSystem playerHealth;
    [SerializeField] private Image totalHealthBar;
    [SerializeField] private Image CurrentHealthBar;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CurrentHealthBar.fillAmount = playerHealth.currentHealth / 10;
    }
}
