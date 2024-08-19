using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Attributes))]
public class NPCGenerator : MonoBehaviour
{
    void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            Debug.Log(GenerateRandomNPCTraits());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public NPCTraits GenerateRandomNPCTraits()
    {
        var npcTraits = new NPCTraits();

        System.Random rand = new System.Random();
        npcTraits.LikeIndex = rand.Next(Attributes.Instance.Likes.AttributeCount);
        npcTraits.FlawIndex = rand.Next(Attributes.Instance.Flaws.AttributeCount);
        npcTraits.JobIndex = rand.Next(Attributes.Instance.Jobs.AttributeCount);
        npcTraits.AchievementIndex = rand.Next(Attributes.Instance.Achievements.AttributeCount);

        return npcTraits;
    }
}
