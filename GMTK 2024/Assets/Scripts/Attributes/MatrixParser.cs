using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;
using System;
using System.Text.RegularExpressions;

public class MatrixParser
{
    private static string CategoryNameToResourcePath(string categoryName) => $"AttributeMatrices/{categoryName}";

    public static ChaosMatrix ParseChaosMatrix(string categoryName)
    {
        var resourcePath = CategoryNameToResourcePath(categoryName);
        var file = Resources.Load<TextAsset>(resourcePath);
        Assert.IsNotNull(file);

        ChaosMatrix matrix = new ChaosMatrix();
        matrix.PluralCategoryName = categoryName;
        // Remove the plural for displaying on character description
        matrix.CategoryName = categoryName[..^1];

        var csvFile = file.ToString().Split('\n');

        matrix.AttributeNames = csvFile[0].Split(',')[1..];
        matrix.AttributeCount = matrix.AttributeNames.Length;
        matrix.Matrix = new float[matrix.AttributeCount][];

        var matrixRows = csvFile[1..];

        for (int ii = 0; ii < matrixRows.Length; ++ii)
        {
            var cells = matrixRows[ii].Split(',')[1..];
            matrix.Matrix[ii] = cells.Select(str => float.Parse(str)).ToArray();
        }
        matrix.UpdateChaosFactors();
        return matrix;
    }
}
