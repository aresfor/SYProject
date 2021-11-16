using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class TimerManager : MonoBehaviour
{
    public static TimerManager Instance;
    public int TimerNum { get => Timers.Count; }
    private BinaryHeap<TimerExMsg> ActiveTimerHeap;
    private HashSet<TimerExMsg> PausedTimerSet;
    private HashSet<TimerExMsg> PendingTimerSet;
    private List<short> TimerHandlePool;
    private List<TimerData> Timers;
    //应对TimerDelegate操作Timer自身情况
    private TimerExMsg CurrentExecutingTimerExMsg;
    private bool bHasBeenTickedThisFrame;
    [SerializeField] private int TimersList_DefaultCapacity = 4;
    //内部事件独立于世界事件，tick中更新
    [SerializeField] private double InternelTime;

    //TODO 对象与其拥有的Timer对应字典

    private void Awake()
    {
        #region 单例
        if (Instance) { Destroy(this); }
        Instance = this;
        #endregion
        //Init
        this.Init();

        //添加空余池
        for (int i = 0;i<TimersList_DefaultCapacity;i++)
        {
            TimerHandlePool.Add((short)i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Tick(Time.deltaTime);
        bHasBeenTickedThisFrame = true;
        StartCoroutine(DoSomeAtEndOfFrame());
    }
    IEnumerator DoSomeAtEndOfFrame()
    {
        yield return new WaitForEndOfFrame();
        bHasBeenTickedThisFrame = false;
    }
    public void Init()
    {
        ActiveTimerHeap = new BinaryHeap<TimerExMsg>();
        PausedTimerSet = new HashSet<TimerExMsg>();
        PendingTimerSet = new HashSet<TimerExMsg>();
        TimerHandlePool = new List<short>();
        Timers = new List<TimerData>();
        for(int i = 0;i<TimersList_DefaultCapacity;i++)
        {
            Timers.Add(new TimerData());
        }
        InternelTime = 0f;
    }

    #region 定时器操作
    public void Reset(TimerExMsg handle)
    {
        TimerData timerData = this.FindTimerData(handle);
        if (timerData == null) { return; }
        //正在执行无法Reset倒计时
        if (!this.TimerExecuting(handle))
        {
            timerData.timer.Handle.ExpiredTime = timerData.timer.ExpiredTime;
            if(timerData.timer.Status != TimerState.Paused)
            {
                if(timerData.timer.Status == TimerState.Active)
                {
                    ActiveTimerHeap.Remove(handle);
                    PendingTimerSet.Add(handle);
                    timerData.timer.Status = TimerState.Pending;
                }
            }
        }
    }
    public void Pause(TimerExMsg handle)
    {
        TimerData timer = this.FindTimerData(handle);
        if(timer == null) { return; }
        if(timer.timer.Status == TimerState.Paused) { return; }
        if (!this.TimerExecuting(handle))
        {
            if(Timers[handle.Handle].timer.Status == TimerState.Active)
            {
                ActiveTimerHeap.Remove(handle);
                //保留倒计时,Resume时用
                Timers[handle.Handle].timer.ExpiredTime -= InternelTime;
            }
            else if(Timers[handle.Handle].timer.Status == TimerState.Pending)
            {
                //因为peding状态为加入active还没有改变expiredTime为实际触发时间，不需要对expiredTime进行修改
                PendingTimerSet.Remove(handle);
            }
            
            //Debug.LogWarning("Paused expiredtime: " + Timers[handle.Handle].timer.ExpireTime);
            PausedTimerSet.Add(handle);
        }
        Timers[handle.Handle].timer.Status = TimerState.Paused;
    }
    public void Resume(TimerExMsg handle)
    {
        foreach (var item in PausedTimerSet)
        {
            if(item == handle)
            {
                Timers[handle.Handle].timer.Status = TimerState.Pending;
                //Debug.LogWarning("Interneltime: " + InternelTime + "expiredtime: " + Timers[handle.Handle].timer.ExpireTime);
                PendingTimerSet.Add(handle);
                break;
            }
        }
        return;
    }
    public void Stop(TimerExMsg handle)
    {
        if(this.TimerExecuting(handle))
        {
            Timers[handle.Handle].timer.bStop = true;
        }
        else
        {
            RemoveTimerData(handle);
        }
    }
    #endregion


    public bool TimerExecuting(TimerExMsg handle)
    {
        if (handle == CurrentExecutingTimerExMsg)
            return true;
        return false;
    }
    //获取Timers列表中空闲位置的标识
    public short GetWorkableHandle()
    {
        short temp;
        if (TimerHandlePool.Count == 0)
        {
            int newCapacity = (int)(Timers.Count * 1.5);
            ExpandTimers(newCapacity);
        }
        temp = TimerHandlePool[0];
        TimerHandlePool.Remove(temp);
        return temp;
    }
    //拓展Timer缓冲池在Timer需求变大的时候，
    //TODO 没有做容量回收
    //TODO 没有考虑拓展失败的异常情况
    private void ExpandTimers(int capacity)
    {
        //List<TimerData> temp = new List<TimerData>(capacity);
        //if (temp == null)
        //{
        //    throw new OutOfMemoryException();
        //}

        int preCapacity = Timers.Count;
        for(int i = preCapacity;i<capacity;i++)
        {
            Timers.Add(new TimerData());
            TimerHandlePool.Add((short)i);
        }
    }
    //外部获取Timer的方法
    public TimerExMsg SetTimer(Delegate del ,double expiredTime, bool requireDelegate = true ,bool loop = false, 
        float loopRate = 1.0f, TimerState status = TimerState.Pending,params object[] parameters)
    {
        Timer timer = new Timer();
        short handle = this.GetWorkableHandle();
        TimerExMsg timerExMsg = new TimerExMsg(handle, expiredTime);
        timer.SetData(del, loop, timerExMsg, loopRate, requireDelegate, expiredTime, status,parameters);
        Timers[timerExMsg.Handle].timer = timer;
        Timers[timerExMsg.Handle].InUse = true;
        PendingTimerSet.Add(timerExMsg);

        return timerExMsg;
    }
    private TimerData FindTimerData(TimerExMsg handle)
    {
        foreach (TimerData item in Timers)
        {
            if(!item.InUse) { continue; }
            if(item.timer.Handle.Equals(handle))
            {
                return item;
            }
        }
        return null;
    }
    private void RemoveTimerData(TimerExMsg handle)
    {
        TimerData item = FindTimerData(handle);
        if(item == null) { return; }

        item.InUse = false;
        TimerHandlePool.Add(item.timer.Handle.Handle);
        item.timer.Clear();
        item.timer = null;

    }
    private void Tick(float deltaTime)
    {
        if(bHasBeenTickedThisFrame) { return; }

        InternelTime += deltaTime;

        while(ActiveTimerHeap.Count > 0)
        {
            TimerExMsg TopHandle = ActiveTimerHeap.GetHeapTop();
            short TopIndex = TopHandle.Handle;
            TimerData TopTimerData = Timers[TopIndex];

            if(TopTimerData.timer.Status.Equals(TimerState.ActivePendingRemoval))
            {
                ActiveTimerHeap.Dequeue();
                RemoveTimerData(TopHandle);
                continue;
            }

            if(InternelTime > TopHandle.ExpiredTime)
            {
                //定时器时间到了，移除最小堆并添加到CurrentExecutingTimerExMsg
                CurrentExecutingTimerExMsg = TopHandle;
                ActiveTimerHeap.Dequeue();
                TopTimerData.timer.Status = TimerState.Executing;

                //定时器Tick间隔可能执行多次Delegate，这里并不是理想的按时间间隔Rate执行Delegate
                int callCount = TopTimerData.timer.bLoop ?
                    (int)((InternelTime - CurrentExecutingTimerExMsg.ExpiredTime) / TopTimerData.timer.LoopRate + 1)
                    : 1;
                for(int i = 0;i<callCount;i++)
                {
                    TopTimerData.timer.Execute();

                    TopTimerData = FindTimerData(TopHandle);
                    if(TopTimerData == null|| TopTimerData.timer.Status != TimerState.Executing)
                    { 
                        break; 
                    }

                    if(TopTimerData!=null)
                    {
                        if(TopTimerData.timer.bStop)
                        {
                            RemoveTimerData(TopHandle);
                            continue;
                        }
                        if(TopTimerData.timer.bLoop && 
                            (!TopTimerData.timer.bRequireDelegate|| 
                            TopTimerData.timer.timerDelegate.IsBound()))
                        {
                            //FIXME

                            //TopTimerData.timer.ExpireTime += callCount * TopTimerData.timer.LoopRate;
                            CurrentExecutingTimerExMsg.ExpiredTime += callCount * TopTimerData.timer.LoopRate;
                            if (TopTimerData.timer.Status == TimerState.Paused)
                            {
                                PausedTimerSet.Add(TopTimerData.timer.Handle);
                            }
                            else
                            {
                                TopTimerData.timer.Status = TimerState.Active;
                                ActiveTimerHeap.Enqueue(CurrentExecutingTimerExMsg);
                            }
                        }
                        else
                        {
                            RemoveTimerData(CurrentExecutingTimerExMsg);
                        }
                    }

                }
            }
            else
            {
                break;
            }

        }
        if(PendingTimerSet.Count > 0)
        {
            foreach (TimerExMsg handle in PendingTimerSet)
            {
                TimerData TimerToActive = FindTimerData(handle);

                //FIXME
                handle.ExpiredTime += InternelTime;
                TimerToActive.timer.Status = TimerState.Active;
               
                ActiveTimerHeap.Enqueue(handle);
            }
            PendingTimerSet.Clear();
        }
        
    }
    
}
