using System;
using System.Collections.Generic;


/// <summary>
/// 最小堆实现
/// </summary>
/// <typeparam name="T"></typeparam>
public class BinaryHeap<T>
{
    //默认容量为6
    private const int DEFAULT_CAPACITY = 6;
    private int mCount;
    private T[] mItems;
    private Comparer<T> mComparer;
    public int Count => mCount;

    public BinaryHeap() : this(DEFAULT_CAPACITY) { }
    public T GetHeapTop()
    {
        if(mCount > 0)
        {
            return mItems[0];
        }else
        {
            throw new InvalidOperationException();
        }
    }
    public BinaryHeap(int capacity)
    {
        if (capacity < 0)
        {
            throw new IndexOutOfRangeException();
        }
        mItems = new T[capacity];
        mComparer = Comparer<T>.Default;
    }

    /// <summary>
    /// 增加元素到堆，并从后往前依次对各结点为根的子树进行筛选，使之成为堆，直到根结点
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public bool Enqueue(T value)
    {
        if (mCount == mItems.Length)
        {
            ResizeItemStore((int)(mItems.Length * 1.5));
        }

        mItems[mCount++] = value;
        int position = BubbleUp(mCount - 1);

        return (position == 0);
    }

    /// <summary>
    /// 取出堆的最小值
    /// </summary>
    /// <returns></returns>
    public T Dequeue()
    {
        return Dequeue(true);
    }
    public void Remove(T value)
    {
        for(int i = 0;i<mCount;i++)
        {
            if(value.Equals(mItems[i]))
            {
                --mCount;
                mItems[i] = mItems[mCount];
                mItems[mCount] = default(T);
                //维护堆的结构
                BubbleDown();
            }
        }
    }
    private T Dequeue(bool shrink)
    {
        if (mCount == 0)
        {
            throw new InvalidOperationException();
        }
        T result = mItems[0];
        if (mCount == 1)
        {
            mCount = 0;
            mItems[0] = default(T);
        }
        else
        {
            --mCount;
            //取序列最后的元素放在堆顶
            mItems[0] = mItems[mCount];
            mItems[mCount] = default(T);
            // 维护堆的结构
            BubbleDown();
        }
        if (shrink)
        {
            ShrinkStore();
        }
        return result;
    }

    private void ShrinkStore()
    {
        // 如果容量不足一半以上，默认容量会下降。
        if (mItems.Length > DEFAULT_CAPACITY && mCount < (mItems.Length >> 1))
        {
            int newSize = Math.Max(
                DEFAULT_CAPACITY, (((mCount / DEFAULT_CAPACITY) + 1) * DEFAULT_CAPACITY));

            ResizeItemStore(newSize);
        }
    }

    private void ResizeItemStore(int newSize)
    {
        if (mCount < newSize || DEFAULT_CAPACITY <= newSize)
        {
            return;
        }

        T[] temp = new T[newSize];
        Array.Copy(mItems, 0, temp, 0, mCount);
        mItems = temp;
    }

    public void Clear()
    {
        mCount = 0;
        mItems = new T[DEFAULT_CAPACITY];
    }

    /// <summary>
    /// 从前往后依次对各结点为根的子树进行筛选，使之成为堆，直到序列最后的节点
    /// </summary>
    private void BubbleDown()
    {
        int parent = 0;
        int leftChild = (parent * 2) + 1;
        while (leftChild < mCount)
        {
            // 找到子节点中较小的那个
            int rightChild = leftChild + 1;
            int bestChild = (rightChild < mCount && mComparer.Compare(mItems[rightChild], mItems[leftChild]) < 0) ?
                rightChild : leftChild;
            if (mComparer.Compare(mItems[bestChild], mItems[parent]) < 0)
            {
                // 如果子节点小于父节点, 交换子节点和父节点
                T temp = mItems[parent];
                mItems[parent] = mItems[bestChild];
                mItems[bestChild] = temp;
                parent = bestChild;
                leftChild = (parent * 2) + 1;
            }
            else
            {
                break;
            }
        }
    }

    /// <summary>
    /// 从后往前依次对各结点为根的子树进行筛选，使之成为堆，直到根结点
    /// </summary>
    /// <param name="startIndex"></param>
    /// <returns></returns>
    private int BubbleUp(int startIndex)
    {
        while (startIndex > 0)
        {
            int parent = (startIndex - 1) / 2;
            //如果子节点小于父节点，交换子节点和父节点
            if (mComparer.Compare(mItems[startIndex], mItems[parent]) < 0)
            {
                T temp = mItems[startIndex];
                mItems[startIndex] = mItems[parent];
                mItems[parent] = temp;
            }
            else
            {
                break;
            }
            startIndex = parent;
        }
        return startIndex;
    }
}
