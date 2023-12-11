using System;
using System.Collections;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

//создайте задачу типа IJobForTransform, которая будет вращать указанные
//Transform вокруг своей оси с заданной скоростью.

public class Lesson3Task3 : MonoBehaviour
{

    [SerializeField] Transform _transform;
    [SerializeField] GameObject _prefab;
    [SerializeField] int _count;
    [SerializeField] int _spawnRadius;
    [SerializeField] private float _speed = 5;
    TransformAccessArray _accessArray;
    NativeArray<float> _angle;


    void Start()
    {
        _angle = new NativeArray<float>(_count, Allocator.Persistent);
        _accessArray = new TransformAccessArray(SpawnObj(_prefab, _count, _spawnRadius));
    }

    private Transform[] SpawnObj(GameObject prefab, int count, int spawnRadius)
    {
        Transform [] objects = new Transform[count];
        for (int i = 0; i < count; i++)
        {
            objects[i] = Instantiate(prefab).transform;
            objects[i].position = UnityEngine.Random.insideUnitSphere * spawnRadius;
        }

        return objects;
    }

    private void Update()
    {
        JobTask3 rotationJob = new JobTask3()
        {
            Speed = _speed,
            angle = _angle
        };

        JobHandle moveHandle = rotationJob.Schedule(_accessArray);
        moveHandle.Complete();

    }

    private void OnDestroy()
    {
        if (_accessArray.isCreated)
        {
            _angle.Dispose();
            _accessArray.Dispose();
        }
    }
}



public struct JobTask3 : IJobParallelForTransform
{
    public float Speed;
    public NativeArray<float> angle;

    [ReadOnly]
    public float DeltaTime;

    public void Execute(int index, TransformAccess transform)
    {
        transform.localRotation = Quaternion.AngleAxis(angle[index], Vector3.up);
        angle[index] = angle[index] == 180 ? 0 : angle[index] + Speed;

    }
}
