using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;
using UnityEngine.Rendering;

public class JobTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        NativeArray<float> result = new NativeArray<float>(1, Allocator.TempJob);

        // 设置作业数据
        MyJob jobData = new MyJob();
        jobData.a = 10;
        jobData.b = 10;
        jobData.result = result;

        // 调度作业
        JobHandle handle = jobData.Schedule();

        // 等待作业完成
        handle.Complete();

        // NativeArray 的所有副本都指向同一内存，您可以在"您的"NativeArray 副本中访问结果
        Debug.Log(result[0]);

        // 释放由结果数组分配的内存
        result.Dispose();

        MyParallelJob jobData2 = new MyParallelJob();
        MyTransformJob jobData3 = new MyTransformJob();
    }
}
// 将两个浮点值相加的作业
public struct MyParallelJob : IJobParallelFor
{
    [ReadOnly]
    public NativeArray<float> a;
    [ReadOnly]
    public NativeArray<float> b;
    public NativeArray<float> result;

    public void Execute(int i)
    {
        result[i] = a[i] + b[i];
    }
}
public struct MyTransformJob : IJobParallelForTransform
{
    public void Execute(int index, TransformAccess transform)
    {
        
    }
}