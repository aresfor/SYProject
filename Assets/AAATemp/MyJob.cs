// ����������ֵ��ӵ���ҵ
using Unity.Collections;
using Unity.Jobs;

public struct MyJob : IJob
{
    public float a;
    public float b;
    public NativeArray<float> result;

    public void Execute()
    {
        result[0] = a + b;
    }
}