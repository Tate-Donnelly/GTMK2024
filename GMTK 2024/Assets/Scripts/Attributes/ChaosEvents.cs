using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaosEvents : MonoBehaviour
{
    public static ChaosEvents Instance { get; private set; }

    public Dictionary<(string, string), (int, string)> Likes { get; private set; }
    public Dictionary<(string, string), (int, string)> Jobs { get; private set; }
    public Dictionary<(string, string), (int, string)> Achievements { get; private set; }

    void Awake()
    {
        if (Instance != null)
            Destroy(this);

        Instance = this;

        LoadChaosMatrices();

        /*
        var MatToTest = Achievements;
        MatToTest.Print();
        var vec = MatToTest.CreateOnesVector();
        Debug.Log("Before:");
        Debug.Log(MatToTest.VectorToString(vec));
        
        Debug.Log("After:");
        Debug.Log(MatToTest.VectorToString(MatToTest.Multiply(vec)));
        */
    }

    void LoadChaosMatrices()
    {
        Likes = MatrixParser.ParseEventMatrix("LikesEvents");
        Jobs = MatrixParser.ParseEventMatrix("JobsEvents");
        Achievements = MatrixParser.ParseEventMatrix("AchievementsEvents");
    }
}
