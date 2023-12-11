using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class TestTask : MonoBehaviour
{
    //задание 2 и 3 в одном файле

    public static CancellationTokenSource cancellationTokenSourceTask1;
    public static CancellationTokenSource cancellationTokenSourceTask2;
    public static CancellationTokenSource cancellationTokenSourceTask3;
    //CancellationToken cancellationToken3;

    Task test1Res;
    Task test2Res;


    void Start()
    {
        cancellationTokenSourceTask1 = new CancellationTokenSource();
        CancellationToken cancellationToken1 = cancellationTokenSourceTask1.Token;
        cancellationTokenSourceTask2 = new CancellationTokenSource();
        CancellationToken cancellationToken2 = cancellationTokenSourceTask2.Token;


        test1Res = Task.Run(() => Task1(cancellationToken1));
        test2Res = Task2(cancellationToken2);

    }

    //Задание 2  
    private async Task<int> Task1(CancellationToken ct)
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        UnityEngine.Debug.Log($"Task1 start");
        if (ct.IsCancellationRequested)
        {
            UnityEngine.Debug.Log($"IsCancellationRequested Task1");
            Cancel();
            return 0;
        }

        await Task.Delay(1000);
        UnityEngine.Debug.Log($"Task1 finish");

        stopwatch.Stop();
        TimeSpan ts = stopwatch.Elapsed;
        UnityEngine.Debug.Log($"1 second timer {ts.Seconds} : {ts.Milliseconds}");

        return 1;
    }


    private async Task<int> Task2(CancellationToken ct)
    {
        UnityEngine.Debug.Log($"Task2 start");

        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        int frameCounter = 0;

        while (frameCounter < 60)
        {
            if (ct.IsCancellationRequested)
            {
                UnityEngine.Debug.Log("IsCancellationRequested Task1 60 frame - " + frameCounter);
                stopwatch.Stop();
                TimeSpan ts1 = stopwatch.Elapsed;
                UnityEngine.Debug.Log($"60 frame timer {ts1.Seconds} : {ts1.Milliseconds}");
                Cancel();
                return 0;
            }

            await Task.Yield(); //ожидать 60 кадров
            frameCounter++;
        }
        UnityEngine.Debug.Log("60 frame - " + frameCounter);

        stopwatch.Stop();
        TimeSpan ts = stopwatch.Elapsed;
        UnityEngine.Debug.Log($"{ts.Seconds} : {ts.Milliseconds}");

        return 2;
    }

    //Задание 3
    public async static Task<bool> WhatTaskFasterAsync(Task task1, Task task2)
    {
        Task taskres = await Task.WhenAny(task1, task2);
        return false;
    }

    public async static Task<bool> Task3(CancellationToken ct, Task task1, Task task2)
    {
        WhatTaskFasterAsync(task1, task2);
        bool flag = true;
        do
        {
            if (task1.IsCompleted)
            {
                UnityEngine.Debug.Log("CancellationToken task1");
                flag = false;
                Cancel();
                return true;
            }
            if (task2.IsCompleted)
            {
                UnityEngine.Debug.Log("CancellationToken task2");
                flag = false;
                Cancel();
                return false;
            }
            if (ct.IsCancellationRequested)
            {
                UnityEngine.Debug.Log("CancellationToken 3");
                flag = false;
                Cancel();
                return false;
            }

            await Task.Yield();
        } while (flag);


        return false;
    }


    private static void Cancel()
    {

        cancellationTokenSourceTask1?.Cancel();
        cancellationTokenSourceTask2?.Cancel();
        cancellationTokenSourceTask3?.Cancel();
    }

    private void OnDestroy()
    {
        if (cancellationTokenSourceTask1 != null)
        {
            cancellationTokenSourceTask1.Dispose();
        }

        if (cancellationTokenSourceTask2 != null)
        {
            cancellationTokenSourceTask2.Dispose();
        }

        if (cancellationTokenSourceTask3 != null)
        {
            cancellationTokenSourceTask3.Dispose();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown("a"))
        {
            Cancel();

            cancellationTokenSourceTask1 = new CancellationTokenSource();
            CancellationToken cancellationToken1 = cancellationTokenSourceTask1.Token;
            cancellationTokenSourceTask2 = new CancellationTokenSource();
            CancellationToken cancellationToken2 = cancellationTokenSourceTask2.Token;
            cancellationTokenSourceTask3 = new CancellationTokenSource();
            CancellationToken cancellationToken3 = cancellationTokenSourceTask3.Token;

            var result = Task3(cancellationToken3, Task1(cancellationToken1), Task2(cancellationToken2));
        }

        if (Input.GetKeyDown("1"))
        {
            cancellationTokenSourceTask1?.Cancel();
        }

        if (Input.GetKeyDown("2"))
        {
            cancellationTokenSourceTask2?.Cancel();
        }

        if (Input.GetKeyDown("3"))
        {
            cancellationTokenSourceTask3?.Cancel();
        }
    }
}

