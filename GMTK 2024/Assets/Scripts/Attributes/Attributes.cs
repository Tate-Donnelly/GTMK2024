using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attributes : MonoBehaviour
{
    public static Attributes Instance { get; private set; }

    public ChaosMatrix Likes { get; private set; }
    public ChaosMatrix Flaws { get; private set; }
    public ChaosMatrix Jobs { get; private set; }
    public ChaosMatrix Achievements { get; private set; }

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
        Likes = MatrixParser.ParseChaosMatrix("Likes");
        Flaws = MatrixParser.ParseChaosMatrix("Flaws");
        Jobs = MatrixParser.ParseChaosMatrix("Jobs");
        Achievements = MatrixParser.ParseChaosMatrix("Achievements");
    }
}
