using UnityEngine.UI;
using UnityEngine;

public class PortalCrosshair : MonoBehaviour
{
    public bool Blue;
    public bool Orange;
    public Portal BluePortal;
    public Portal OrangePortal;
    public Sprite OnlyOrange;
    public Sprite OnlyBlue;
    public Sprite Both;
    public Sprite None;
    public Image CanvasImage;

    void Start()
    {
        CanvasImage = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        //en update tengo que hacer que busque y asocie cada portal nuevamente, por si desaparecen que los
        //vuelva a pillar, pero los dos tiene nel mismo code asi que tengo que buscar si se puede mirar por name o algo
        if (OrangePortal == null) Orange = false; else Orange = true;
        if (BluePortal == null) Blue = false; else Blue = true;
        if (Blue && Orange) CanvasImage.sprite = Both;
        if (Blue && !Orange) CanvasImage.sprite = OnlyBlue;
        if (!Blue && Orange) CanvasImage.sprite = OnlyOrange;
        if (!Blue && !Orange) CanvasImage.sprite = None;
    }
}
