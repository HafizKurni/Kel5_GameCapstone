using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageZoom : MonoBehaviour
{
    [SerializeField] private GameObject zoomedImagePanel;
    [SerializeField] private Image zoomedImage;
    public void ShowZoomedImage(Sprite imageToZoom)
    {
        zoomedImagePanel.SetActive(true);
        zoomedImage.sprite = imageToZoom;
        CanvasGroup canvasGroup = zoomedImagePanel.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }
    public void CloseZoom()
    {
        CanvasGroup canvasGroup = zoomedImagePanel.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        zoomedImagePanel.SetActive(false);
    }
}
