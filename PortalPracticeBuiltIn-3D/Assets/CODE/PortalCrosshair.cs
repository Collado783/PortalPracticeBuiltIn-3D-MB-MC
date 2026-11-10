using UnityEngine;
using UnityEngine.UI;

public class PortalCrosshair : MonoBehaviour
{
    public Portal Blue;
    public Portal Orange;

    public Sprite OnlyOrange;
    public Sprite OnlyBlue;
    public Sprite Both;
    public Sprite None;

    public Image CanvasImage;

    void Update()
    {
        if (Blue.gameObject.activeSelf && Orange.gameObject.activeSelf)
        {
            CanvasImage.sprite = Both;
        }
        else if (Blue.gameObject.activeSelf)
        {
            CanvasImage.sprite = OnlyBlue;
        }
        else if (Orange.gameObject.activeSelf)
        {
            CanvasImage.sprite = OnlyOrange;
        }
        else
        {
            CanvasImage.sprite = None;
        }
    }
}