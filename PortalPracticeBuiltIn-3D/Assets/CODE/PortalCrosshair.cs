using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class PortalCrosshair : MonoBehaviour
{
    public Portal Blue;
    public Portal Orange;
    public Image OnlyOrange;
    public Image OnlyBlue;
    public Image Both;
    public Image None;
    public Image CanvasImage;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Blue && Orange) CanvasImage = Both;
        if(Blue &&!Orange) CanvasImage = OnlyBlue 
        if (!Blue && Orange)
        if(!Blue && !Orange)
    }
}
