using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]

public class BattlePanel : BasePanel
{
    private CanvasGroup canvasGroup;
    private void Awake() 
    {
        //Debug.Log("Battle::Start()");
        canvasGroup = GetComponent<CanvasGroup>();
        Button btn = GetComponentInChildren<Button>();
        btn.onClick.AddListener(() => UIPanelManager.Instance.PushPanel("TaskUI"));
    }
    public override void OnEnter()
    {
        gameObject.SetActive(true);

        canvasGroup.blocksRaycasts = true;
    }
    public override void OnPause()
    {
        gameObject.SetActive(false);
        canvasGroup.blocksRaycasts = false;
    }
    public override void OnResume()
    {
        gameObject.SetActive(true);

        canvasGroup.blocksRaycasts = true;
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
