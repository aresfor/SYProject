using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class MainPanel : BasePanel
{
    private CanvasGroup canvasGroup;
    private void Awake() 
    {
        canvasGroup = GetComponent<CanvasGroup>();
        //测试代码
        Button btn = GetComponentInChildren<Button>();
        btn.onClick.AddListener(() => UIPanelManager.Instance.PushPanel("BattleUI"));
    }
    public override void OnEnter()
    {
        foreach (Transform t in GameObject.Find(UIConsts.CurrentUIRootTransform).transform)
        {
            if (t.name.Contains(UIPanelType.MainPanel))
                continue;
            Destroy(t.gameObject);
        }
        UIPanelManager.Instance.Clear(UIPanelType.MainPanel,this);
        gameObject.SetActive(true);
        canvasGroup.blocksRaycasts = true;
        //UIManager.instance.Open(UIPanelType.MainPanel);
    }
    public override void OnPause()
    {
        canvasGroup.blocksRaycasts = false;
    }
    public override void OnResume()
    {
        canvasGroup.blocksRaycasts = true;
    }
    public override void OnExit(bool cache = false)
    {
        this.gameObject.SetActive(false);
        if(!cache)
        {
            Destroy(this.gameObject);
        }
    }
}
