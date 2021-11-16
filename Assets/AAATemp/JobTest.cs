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

        // ������ҵ����
        MyJob jobData = new MyJob();
        jobData.a = 10;
        jobData.b = 10;
        jobData.result = result;

        // ������ҵ
        JobHandle handle = jobData.Schedule();

        // �ȴ���ҵ���
        handle.Complete();

        // NativeArray �����и�����ָ��ͬһ�ڴ棬��������"����"NativeArray �����з��ʽ��
        Debug.Log(result[0]);

        // �ͷ��ɽ�����������ڴ�
        result.Dispose();

        MyParallelJob jobData2 = new MyParallelJob();
        MyTransformJob jobData3 = new MyTransformJob();
    }
}
// ����������ֵ��ӵ���ҵ
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