using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

/**
 * <see cref="ChaosMatrix.ChaosFactors"/>
 */
public struct ChaosMatrix
{
    public string CategoryName;
    public string PluralCategoryName;
    public int AttributeCount;
    public string[] AttributeNames;
    public float[][] Matrix;

    /**
     * Chaos factors are the amount to which a person with the attribute will increase the chaos meter by.
     * 
     * When adding a new person, we don't have to do matrix multiplication, we just have to add the chaos factor associated with their trait.
     */
    public float[] ChaosFactors;

    public void UpdateChaosFactors() => ChaosFactors = Multiply(CreateOnesVector());

    public float[] CreateZeroVector()
    {
        float[] vec = new float[AttributeCount];
        for (int ii = 0; ii < AttributeCount; ++ii)
            vec[ii] = 0f;
        return vec;
    }
    public float[] CreateOnesVector()
    {
        float[] vec = new float[AttributeCount];
        for (int ii = 0; ii < AttributeCount; ++ii)
            vec[ii] = 1f;
        return vec;
    }

    public float[] Multiply(float[] vector)
    {
        Assert.AreEqual(vector.Length, AttributeCount);
        float[] result = new float[AttributeCount];

        // Weird iteration for vector x matrix multiplication for cache locality
        for (int ii = 0; ii < AttributeCount; ++ii)
        {
            for (int jj = 0; jj < AttributeCount; ++jj)
            {
                result[jj] += vector[ii] * Matrix[ii][jj];
            }
        }

        return result;
    }

    public void Print()
    {
#if DEBUG
        string msg = $"{CategoryName} ({AttributeCount} {PluralCategoryName}):\n";
        foreach (var attribute in AttributeNames)
        {
            msg += attribute + ", ";
        }
        msg = msg[..^2] + '\n';

        foreach (var row in Matrix)
        {
            msg += VectorToString(row) + '\n';
        }

        Debug.Log(msg);
#endif
    }
    public string VectorToString(float[] vec)
    {
#if DEBUG
        Assert.AreEqual(vec.Length, AttributeCount);
        string msg = "";
        foreach (var val in vec)
        {
            msg += val.ToString("0.00") + ", ";
        }
        return msg[..^2];
#endif
    }
}
