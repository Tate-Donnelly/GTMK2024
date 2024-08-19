using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaosEventListener : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ChaosEventManager.Instance.TriggerChaosEvent += OnEventTriggered;
    }

    private void OnEventTriggered((int, string) chaosEvent)
    {
        switch (chaosEvent.Item1) {
            case 1:
                Debug.Log("Breaking News: " + chaosEvent.Item2);
                break;
            case 0:
                Debug.Log("Hot Take: " + chaosEvent.Item2);
                break;
            default:
                break;
        }
    }

    void OnDestroy()
    {
        ChaosEventManager.Instance.TriggerChaosEvent -= OnEventTriggered;
    }
}
