using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotions : MonoBehaviour
{
    [SerializeField] public float healValue;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<PlayerHealth>().AddHealth(healValue);
            gameObject.SetActive(false);
        }
    }

    public float GetHealValue()
    {
        return healValue;
    }

    public void SetHealValue(float value)
    {
        healValue = value;
    }
}
