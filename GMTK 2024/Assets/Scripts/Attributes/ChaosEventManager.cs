using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaosEventManager : MonoBehaviour
{
    public static ChaosEventManager Instance;
    Queue<(int, string)> actionsQueue = new Queue<(int, string)>();
    private bool ready = false;

    public event Action<(int, string)> TriggerChaosEvent;

    const float COOLDOWN = 15f;
    private float timer = 0;

    private void Awake()
    {
        if (Instance != null)
            Destroy(this);

        Instance = this;
    }

    public void EnqueueAction((int, string) eventText)
    {
        actionsQueue.Enqueue(eventText);
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > COOLDOWN)
        {
            timer = 0;
            ready = true;
        }

        if (actionsQueue.Count <= 0)
        {
            return;
        }

        if (ready)
        {
            TriggerChaosEvent?.Invoke(actionsQueue.Dequeue());
            ready = false;
            return;
        }
    }
}
