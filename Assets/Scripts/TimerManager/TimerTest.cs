using UnityEngine;

public class TimerTest : MonoBehaviour
{
    TimerExMsg handle;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            del d = new del(fuck);
            //拿到的handle中有实际的Handle和保存的应该没什么用的expiredTime参数，不过expiredTime之后马上会被修改，这里可能还需要改一下
            handle = TimerManager.Instance.SetTimer(d, 2.0f);
            //Destroy(this);
        }
        if(Input.GetKeyDown(KeyCode.P))
        {
            TimerManager.Instance.Pause(handle);
        }
        if(Input.GetKeyDown(KeyCode.R))
        {
            TimerManager.Instance.Resume(handle);
        }
    }
    public delegate void del();
    void fuck()
    {
        Debug.LogWarning("hi it's timer call");
    }
}
