using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct NPCTraits
{
    public int LikeIndex, FlawIndex, JobIndex, AchievementIndex;

    public override string ToString() => $"Like: {Attributes.Instance.Likes.AttributeNames[LikeIndex]}, Flaw: {Attributes.Instance.Flaws.AttributeNames[FlawIndex]}, Job: {Attributes.Instance.Jobs.AttributeNames[JobIndex]}, Achievement: {Attributes.Instance.Achievements.AttributeNames[AchievementIndex]}";
}
