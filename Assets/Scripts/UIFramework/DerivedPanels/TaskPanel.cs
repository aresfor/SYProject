using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskPanel : BasePanel
{
    private void Awake()
    {
        Button btn = GetComponentInChildren<Button>();
        btn.onClick.AddListener(() => UIPanelManager.Instance.PushPanel("MainUI"));
    }
    public override void OnEnter()
    {
        gameObject.SetActive(true);
    }
    public override void OnPause()
    {
        
    }
    public override void OnResume()
    {
        //gameObject.SetActive(true);
        UIPanelManager.Instance.PopPanel();
    }
    public override void OnExit(bool cache = false)
    {
        gameObject.SetActive(false);
        if(!cache)
        {
            Destroy(gameObject);
        }
    }
}
