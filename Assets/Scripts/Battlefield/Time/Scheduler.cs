using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameTime))]
[DefaultExecutionOrder(-900)]
public class Scheduler : MonoBehaviour
{
    GameTime time;

    class Job
    {
        public double next;
        public double period;
        public Action action;
        public bool cancelled;
    }

    readonly List<Job> heap = new();

    void Awake()
    {
        time = FindObjectOfType<GameTime>();
        if (!time) Debug.LogError("Scheduler needs a GameTime in the scene.");
    }

    public Action Every(double periodSeconds, Action action, bool runImmediately = false)
    {
        var now = time.Now;
        var j = new Job
        {
            period = Math.Max(0.000001, periodSeconds),
            action = action,
            next = runImmediately ? now : now + periodSeconds
        };
        Push(j);
        return () => j.cancelled = true;
    }

    void Update()
    {
        if (Time.timeScale == 0f) return;

        var now = time.Now;

        if (heap.Count > 0 && heap[0].next <= now)
        {
            var j = Pop();
            if (!j.cancelled)
            {
                try { j.action?.Invoke(); }
                catch (Exception e) { Debug.LogException(e); }

                j.next += j.period;
                if (!j.cancelled) Push(j);
            }
        }
    }

    void Push(Job j)
    {
        heap.Add(j);
        int i = heap.Count - 1;
        while (i > 0)
        {
            int p = (i - 1) / 2;
            if (heap[p].next <= heap[i].next) break;
            (heap[p], heap[i]) = (heap[i], heap[p]); i = p;
        }
    }

    Job Pop()
    {
        var root = heap[0];
        int last = heap.Count - 1;
        heap[0] = heap[last];
        heap.RemoveAt(last);

        int i = 0;
        while (true)
        {
            int l = 2 * i + 1, r = l + 1, s = i;
            if (l < heap.Count && heap[l].next < heap[s].next) s = l;
            if (r < heap.Count && heap[r].next < heap[s].next) s = r;
            if (s == i) break;
            (heap[s], heap[i]) = (heap[i], heap[s]); i = s;
        }
        return root;
    }
}
