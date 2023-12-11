using Unity.Jobs;
using Unity.Collections;
using UnityEngine.Jobs;
using UnityEngine;

public class Lesson2Task1 : MonoBehaviour
{

    void Start()
    {
        NativeArray<int> intArray = new NativeArray<int>(new int[] { 5, 54, 3, 17, 86, 9, 1, 6 }, Allocator.Persistent);
        MyJob myJob = new MyJob
        {
            intArr = intArray,
        };
        
        for(int i = 0; i < intArray.Length; i++)
        {
            Debug.Log($"{i + 1} элемент равен  {intArray[i]}");
        }

        JobHandle jobHandle = myJob.Schedule();
        jobHandle.Complete();

        for (int i = 0; i < intArray.Length; i++)
        {
            Debug.Log($"{i + 1} элемент равен  {intArray[i]}");
        }
    }


    public struct MyJob : IJob
    {
        public NativeArray<int> intArr;

        public void Execute()
        {
            for (int i = 0; i < intArr.Length; i++)
            {
                if (intArr[i] > 10)
                {
                    intArr[i] = 0;
                }
            }
        }
    }
}
