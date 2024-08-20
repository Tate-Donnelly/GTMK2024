using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaosLevels : MonoBehaviour
{
    public int CitizenCount { get; private set; }
    float[] citizenLikes, citizenFlaws, citizenJobs, citizenAchievements;
    List<string> likesList, jobsList, achievementsList;

    public float ChaosLevel { get; private set; }

    const float ExpectedChaosPerPerson = 18.47f;

    void Start()
    {
        citizenLikes = Attributes.Instance.Likes.CreateZeroVector();
        citizenFlaws = Attributes.Instance.Flaws.CreateZeroVector();
        citizenJobs = Attributes.Instance.Jobs.CreateZeroVector();
        citizenAchievements = Attributes.Instance.Achievements.CreateZeroVector();

        likesList = new List<string>();
        jobsList = new List<string>();
        achievementsList = new List<string>();

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


        EnqueueEvent(npc);

        likesList.Add(Attributes.Instance.Likes.AttributeNames[npc.LikeIndex]);
        jobsList.Add(Attributes.Instance.Jobs.AttributeNames[npc.JobIndex]);
        achievementsList.Add(Attributes.Instance.Achievements.AttributeNames[npc.AchievementIndex]);

#if DEBUG
        chaosDelta = ChaosLevel - chaosDelta;
        Debug.Log($"Population: {CitizenCount}, Chaos: {ChaosLevel}/{ChaosLimit()}, Chaos Delta: +{chaosDelta}");
#endif
    }

    public float ChaosLimit()
    {
        return Mathf.Log(CitizenCount) * ExpectedChaosPerPerson;
    }

    public void EnqueueEvent(NPCTraits npc)
    {
        (int, string) likeEvent = GetEvent(likesList, ChaosEvents.Instance.Likes, Attributes.Instance.Likes.AttributeNames[npc.LikeIndex]);
        (int, string) jobEvent = GetEvent(jobsList, ChaosEvents.Instance.Jobs, Attributes.Instance.Jobs.AttributeNames[npc.JobIndex]);
        (int, string) achievementEvent = GetEvent(achievementsList, ChaosEvents.Instance.Achievements, Attributes.Instance.Achievements.AttributeNames[npc.AchievementIndex]);

        if(likeEvent.Item1 != -2)
        {
            ChaosEventManager.Instance.EnqueueAction(likeEvent);
        }

        if(jobEvent.Item1 != -2)
        {
            ChaosEventManager.Instance.EnqueueAction(jobEvent);
        }

        if (achievementEvent.Item1 != -2)
        {
            ChaosEventManager.Instance.EnqueueAction(achievementEvent);
        }
    }

    public (int, string) GetEvent(List<string> attributeList, Dictionary<(string, string), (int, string)> dict, string attribute)
    {
        List<(int, string)> events = new List<(int, string)>();
        foreach (var item in attributeList)
        {
            if (dict.ContainsKey((attribute, item)))
            {
                events.Add(dict[(attribute, item)]);
            }
        }
        if(events.Count <= 0 )
        {
            return (-2, "");
        }
        events.Sort((x, y) => y.Item1.CompareTo(x.Item1));
        return events[0];
    }

}
