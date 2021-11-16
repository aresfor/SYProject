using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRoot : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UIPanelManager panelManager = UIPanelManager.Instance;
        panelManager.currentUIRootTransform = GameObject.Find("Canvas").transform;
        panelManager.PushPanel(UIPanelType.MainPanel);
    }
}
