using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaosLevels : MonoBehaviour
{
    public int CitizenCount { get; private set; }
    float[] citizenLikes, citizenFlaws, citizenJobs, citizenAchievements;

    public float ChaosLevel { get; private set; }

    const float ExpectedChaosPerPerson = 18.47f;

    void Start()
    {
        citizenLikes = Attributes.Instance.Likes.CreateZeroVector();
        citizenFlaws = Attributes.Instance.Flaws.CreateZeroVector();
        citizenJobs = Attributes.Instance.Jobs.CreateZeroVector();
        citizenAchievements = Attributes.Instance.Achievements.CreateZeroVector();

        CitizenCount = 0;
        ChaosLevel = 0;
    }

    public void AddNPC(NPCTraits npc)
    {
        CitizenCount++;
#if DEBUG
        var chaosDelta = ChaosLevel;
#endif
        ChaosLevel += Attributes.Instance.Likes.Multiply(citizenLikes)[npc.LikeIndex];
        ChaosLevel += Attributes.Instance.Flaws.Multiply(citizenFlaws)[npc.FlawIndex];
        ChaosLevel += Attributes.Instance.Jobs.Multiply(citizenJobs)[npc.JobIndex];
        ChaosLevel += Attributes.Instance.Achievements.Multiply(citizenAchievements)[npc.AchievementIndex];

        citizenLikes[npc.LikeIndex]++;
        citizenFlaws[npc.FlawIndex]++;
        citizenJobs[npc.JobIndex]++;
        citizenAchievements[npc.AchievementIndex]++;

#if DEBUG
        chaosDelta = ChaosLevel - chaosDelta;
        Debug.Log($"Population: {CitizenCount}, Chaos: {ChaosLevel}/{ChaosLimit()}, Chaos Delta: +{chaosDelta}");
#endif
    }

    public float ChaosLimit()
    {
        return Mathf.Log(CitizenCount) * ExpectedChaosPerPerson;
    }
}
