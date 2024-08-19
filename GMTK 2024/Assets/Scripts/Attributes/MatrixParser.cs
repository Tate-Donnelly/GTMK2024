using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;
using System;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;

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

    public static Dictionary<(string, string),(int, string)> ParseEventMatrix(string categoryName)
    {
        Dictionary<(string, string), (int, string)> result = new Dictionary<(string, string), (int, string)>();
        var resourcePath = CategoryNameToResourcePath(categoryName);
        var file = Resources.Load<TextAsset>(resourcePath);
        Assert.IsNotNull(file);

        var csvFile = file.ToString().Split('\n');

        string[] strings = csvFile[0].Split(',')[1..];

        var matrixRows = csvFile[1..];

        for (int ii = 0; ii < matrixRows.Length; ++ii)
        {
            var cells = matrixRows[ii].Split(',')[1..];
            string[] columns = cells.ToArray();
            for (int jj = 0; jj < columns.Length; ++jj)
            {
        
                bool number = float.TryParse(columns[jj], out float res);
                if (number)
                {
                    continue;
                }
                char[] chars = columns[jj].ToCharArray();
                int severity = -1;
                switch (chars[0]) 
                {
                    case '#':
                        severity = 1;
                        break;
                    case '*':
                        severity = 0;
                        break;
                    default:
                        break;
                }

                string finalString = columns[jj].Substring(1);

                result.Add((strings[ii], strings[jj]), (severity, finalString));
            }
        }

        return result;
    }
}
