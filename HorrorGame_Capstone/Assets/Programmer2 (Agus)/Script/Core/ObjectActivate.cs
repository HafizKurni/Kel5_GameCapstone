using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectActivate : MonoBehaviour
{
    [Header("Object to Activate")]
    [SerializeField] private GameObject objectToActivate;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ActivateObject();
        }
    }

    private void ActivateObject()
    {
        if (objectToActivate != null)
        {
            objectToActivate.SetActive(true);
        }
    }
}