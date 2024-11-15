using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    public float interactionRange = 2f;
    private PlayerInput playerInput;

    private void Awake()
    {
        playerInput = new PlayerInput();
    }

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    private void Interact(InputAction.CallbackContext context)
    {
        // Cari objek yang berada dalam jarak interaksi
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, interactionRange))
        {
            // Kirim perintah interaksi ke objek yang ditemukan
            hit.transform.gameObject.GetComponent<InteractionItem>().Interact();
        }
    }
}