using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("KameraOptions")]
    [SerializeField] private Transform player;
    [SerializeField] private float distanceAhead;
    [SerializeField] private float cameraSpeed;
    private float lookAhead;

    [Header ("KmaeraBoudary")]
    [SerializeField] private float leftLimit;
    [SerializeField] private float rightLimit;
    [SerializeField] private float upperLimit;  // Batas atas
    [SerializeField] private float lowerLimit;  // Batas bawah

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(player.position.x, transform.position.y, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        lookAhead = Mathf.Lerp(lookAhead, (distanceAhead * player.localScale.x), Time.deltaTime * cameraSpeed);
        float targetPositionX = player.position.x + lookAhead;

        // Batasi posisi kamera
        targetPositionX = Mathf.Clamp(targetPositionX, leftLimit, rightLimit);

        // Batasi posisi Y kamera
        float targetPositionY = Mathf.Clamp(player.position.y, lowerLimit, upperLimit);

        transform.position = new Vector3(targetPositionX, targetPositionY, transform.position.z);
    }
}