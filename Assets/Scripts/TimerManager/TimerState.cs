public enum TimerState
{
    //在下一帧加入Active
    Pending = 1,
    Active = 2,
    Paused = 3,
    Executing = 4,
    //从Active修改为可以删除，下个tick删除timer
    ActivePendingRemoval = 5
}