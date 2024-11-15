using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionItem : MonoBehaviour
{
    public float interactionRange = 2f;
    public KeyCode interactionKey = KeyCode.F;
    public void Update()
    {
        // Cek jika tombol interaksi ditekan
        if (Input.GetKeyDown(interactionKey))
        {
            Interact();
        }
    }

    public void Interact()
    {
        // Cari semua collider dalam jangkauan interaksi
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, interactionRange);
        foreach (var hitCollider in hitColliders)
        {
            // Cek apakah objek yang terdeteksi adalah tile
            if (hitCollider.CompareTag("Gate")) // Pastikan tiles memiliki tag "Tile"
            {
                Destroy(hitCollider.gameObject); // Hapus objek tile
                Debug.Log("Removed tile: " + hitCollider.name);
            }
        }
    }
}