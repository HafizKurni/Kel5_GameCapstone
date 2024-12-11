using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageZoom : MonoBehaviour
{
    [SerializeField] private GameObject zoomedImagePanel;
    [SerializeField] private Image zoomedImage;

    // Method untuk menampilkan gambar dalam zoom
    public void ShowZoomedImage(Sprite imageToZoom)
    {
        zoomedImagePanel.SetActive(true); // Aktifkan panel zoom
        zoomedImage.sprite = imageToZoom; // Set sprite dari gambar
        CanvasGroup canvasGroup = zoomedImagePanel.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 1; // Tampilkan panel
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    // Method untuk menutup zoom
    public void CloseZoom()
    {
        CanvasGroup canvasGroup = zoomedImagePanel.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0; // Sembunyikan panel
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        zoomedImagePanel.SetActive(false); // Nonaktifkan panel
    }
}
