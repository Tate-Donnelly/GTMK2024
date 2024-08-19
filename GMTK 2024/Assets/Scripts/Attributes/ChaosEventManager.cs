using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaosEventManager : MonoBehaviour
{
    public static ChaosEventManager Instance;
    Queue<string> actionsQueue = new Queue<string>();
    private bool ready = false;

    public event Action<string> TriggerChaosEvent;

    const float COOLDOWN = 15f;
    private float timer = 0;

    private void Awake()
    {
        if (Instance != null)
            Destroy(this);

        Instance = this;
    }

    public void EnqueueAction(String eventText)
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
            TriggerChaosEvent(actionsQueue.Dequeue());
            ready = false;
            return;
        }
    }
}
