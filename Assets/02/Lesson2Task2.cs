using Unity.Jobs;
using Unity.Collections;
using UnityEngine.Jobs;
using UnityEngine;

public class Lesson2Task2 : MonoBehaviour
{
        public NativeArray<Vector3> _positions = new NativeArray<Vector3>(new Vector3[] {new Vector3(1,2,3), new Vector3(3, 4, 5), new Vector3(6, 7, 8), new Vector3(9, 10, 11) }, Allocator.Persistent);
        public NativeArray<Vector3> _velocites = new NativeArray<Vector3>(new Vector3[] {new Vector3(3,2,1), new Vector3(5, 4, 3), new Vector3(8,10,10), new Vector3(2,22,23) }, Allocator.Persistent);
        public NativeArray<Vector3> _res = new NativeArray<Vector3>(4, Allocator.Persistent);

    void Start()
    {

        MyJob myJob = new MyJob
        {
            positions = _positions,
            velocites = _velocites,
            res = _res
        };

        for (int i = 0; i < _res.Length; i++)
        {
            Debug.Log($"{i + 1} элемент равен  {_res[i]}");
        }

        JobHandle jobHandle = myJob.Schedule(_res.Length, 0);
        jobHandle.Complete();

        for (int i = 0; i < _res.Length; i++)
        {
            Debug.Log($"{i + 1} элемент равен  {_res[i]}");
        }
    }


    public struct MyJob : IJobParallelFor
    {
        [ReadOnly] public NativeArray<Vector3> positions;
        [ReadOnly] public NativeArray<Vector3> velocites;
        [WriteOnly] public NativeArray<Vector3> res;

        public void Execute(int index)
        {
            res[index] = positions[index] + velocites[index];
        }
    }

    private void OnDestroy()
    {
        _positions.Dispose();
        _velocites.Dispose();
        _res.Dispose();
    }
}
