using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ChaosLevels)), RequireComponent(typeof(NPCGenerator))]
public class ChaosLevelsTest : MonoBehaviour
{
    [SerializeField]
    Text npcText;
    [SerializeField]
    Button acceptButton, denyButton;
    [SerializeField]
    Slider chaosMeter;

    ChaosLevels chaosLevels;
    NPCGenerator npcGenerator;

    NPCTraits npc;

    private void Start()
    {
        chaosLevels = GetComponent<ChaosLevels>();
        npcGenerator = GetComponent<NPCGenerator>();

        acceptButton.onClick.AddListener(AcceptNPC);
        denyButton.onClick.AddListener(DenyNPC);

        NextNPC();
    }

    void NextNPC()
    {
        npc = npcGenerator.GenerateRandomNPCTraits();

        npcText.text = npc.ToString();
    }

    void AcceptNPC()
    {
        chaosLevels.AddNPC(npc);

        if (chaosLevels.CitizenCount > 1)
            chaosMeter.value = chaosLevels.ChaosLevel / chaosLevels.ChaosLimit();

        NextNPC();
    }

    void DenyNPC()
    {
        NextNPC();
    }
}
