using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class InteractionItem : MonoBehaviour
{
    public GameObject interactPrompt;
    public float interactionRange = 2f;
    private PlayerInput playerInput;
    private Animator anim;
    
    private void Start()
    {
        interactPrompt.SetActive(false);
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.F))
        {
            Interact();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && interactPrompt.activeSelf)
        {
            interactPrompt.SetActive(true);
            Debug.Log("Player entered trigger: " + gameObject.name);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interactPrompt.SetActive(false);
        }
    }

    private void Interact()
    {
        Debug.Log("Interacted with: " + gameObject.name);
        anim.SetTrigger("Activate");
    }
}