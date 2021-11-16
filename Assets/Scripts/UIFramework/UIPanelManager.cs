using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIPanelManager
{
    private static UIPanelManager _instance;
    public Transform currentUIRootTransform;
    public Transform cacheRootTransform;
    static int index = 0;
    //private enum UIType{MainUi,TaskUi,BattleUI};
    public static UIPanelManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = new UIPanelManager();
                _instance.currentUIRootTransform = GameObject.Find(UIConsts.CurrentUIRootTransform).transform;
                if(_instance.currentUIRootTransform == null)
                {
                    Debug.LogError("UIManager没有找到CurrentUIRoot");
                }
                //缓存父节点放在Canvas下
                _instance.cacheRootTransform = GameObject.Find(UIConsts.UICacheTransform).transform;
                if (_instance.currentUIRootTransform == null)
                {
                    Debug.LogError("UIManager没有找到CacheRoot");
                }
            }
            return _instance;
        }
    }
    private Dictionary<string,BasePanel> panelDic;
    private BasePanel GetPanel(string name)
    {
        if(panelDic == null)
            panelDic = new Dictionary<string, BasePanel>();

        BasePanel panel = null;
        
        if(!panelDic.TryGetValue(name, out panel))
        {
            GameObject panelObj 
                = GameObject.Instantiate(Resources.Load<GameObject>("UIPrefab/" + name),
                currentUIRootTransform);
             panel = panelObj.GetComponent<BasePanel>();
            //index++;
            panelDic.Add(name, panel);
        }
        else
        {
            panel.transform.SetParent(currentUIRootTransform);
        }
        return panel;
    }


    //界面存储栈
    private Stack<BasePanel> panelStack;
    void InitStack()
    {
        if(panelStack == null)
            panelStack = new Stack<BasePanel>();
    }
    public void PushPanel(string name)
    {
        InitStack();

        //停止前一个界面
        if(panelStack.Count > 0)
        {
            BasePanel topPanel = panelStack.Peek();
            topPanel.OnPause();
        }

        BasePanel panel = GetPanel(name);
        panel.transform.SetAsLastSibling();
        //if(panel.panelLayer == PanelLayer.BasicUi)
        //{
        //    panelStack.Clear();
        //    panelDic.Clear();
        //}
        panelStack.Push(panel);
        panel.OnEnter();
    }
    public void Clear(string name,BasePanel panel)
    {
        panelStack.Clear();
        panelDic.Clear();
        panelStack.Push(panel);
        panelDic.Add(name, panel);
    }
    public void Clear()
    {
        panelStack.Clear();
        panelDic.Clear();
    }
    public void PopPanel(bool cache = false)
    {
        InitStack();
        if(panelStack.Count <= 0)
            return;
        BasePanel exitPanel = panelStack.Pop();
        exitPanel.OnExit(true);
        if(cache)
        {
            exitPanel.transform.SetParent(cacheRootTransform);
        }
        else
        {
            panelDic.Remove(exitPanel.name);
        }
        //if(exitPanel.panelLayer == PanelLayer.)
        if(panelStack.Count > 0)
        {
            BasePanel resumePanel = panelStack.Peek();
            resumePanel.OnResume();
        }
    }

}
