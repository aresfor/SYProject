using System;

//TODO 池化
public class Timer
{
    public bool bStop;
    public bool bLoop;
    public bool bRequireDelegate;
    public TimerState Status;
    //设置初始ExpiredTime，并不是实际要执行的时间
    public double ExpiredTime;
    //管理Timer是否要被检查,也就是在定时器数组中的下标
    public TimerExMsg Handle;
    public float LoopRate;
    //委托
    public TimerDelegate timerDelegate;

    
    public void Execute()
    {
        timerDelegate.del.DynamicInvoke(timerDelegate.paramList);
    }
    public void Clear()
    {
        timerDelegate.del = null;
        timerDelegate.paramList = null;
    }
    
    public void SetData(Delegate del, bool loop,TimerExMsg handle, float loopRate,bool requireDelegate,double expiredTime,TimerState status = TimerState.Pending,params object[] parameters)
    {
        this.bStop = false;

        this.bLoop = loop;
        this.bRequireDelegate = requireDelegate;
        this.ExpiredTime = expiredTime;
        this.Status = status;
        this.Handle = handle;
        this.LoopRate = loopRate;
        this.timerDelegate = new TimerDelegate();
        this.timerDelegate.del = del;
        this.timerDelegate.paramList = parameters;
    }
}
//保存委托以及参数
public class TimerDelegate
{
    public Delegate del;
    public object[] paramList;
    public TimerDelegate()
    {
        
    }
    //委托是否还是有效
    public bool IsBound()
    {
        foreach (var d in del.GetInvocationList())
        {
            if (d.Target.Equals(null))
                return false;
        }
        return true;
    }
}
//Timer中的expiredTime是一开始设置的多少秒之后执行（比如2）
//这里的expiredTime是实际要执行的时间，一般是加入ActiveSet时候加上InternelTime保存，比如（2+现在时间2.1232）=4.1232
//Handle是定时器标识，设置定时器的时候返回给调用方的就是TimerExMsg
public class TimerExMsg:IComparable<TimerExMsg>
{
    public short Handle;
    public double ExpiredTime;

    public TimerExMsg(short handle,double expiredTime)
    {
        this.Handle = handle;
        this.ExpiredTime = expiredTime;
    }

    public int CompareTo(TimerExMsg other)
    {
        return this.ExpiredTime.CompareTo(other.ExpiredTime);
    }
}
//Timer列表中实际存储的对象，因为定时器缓存池所以要额外标识这个定时器是否被使用
public class TimerData
{
    public Timer timer = null;
    public bool InUse = false;
}