using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;
using CsvHelper;

public static class CsvInpoter
{
    public static List<T> LoadText<T> (string path)
    {
        string allPath = Application.dataPath + "/" + path;
        var recose = new List<T>();
        using (var sr = new StreamReader(allPath)) 
        using (var csv = new CsvReader(sr, CultureInfo.InvariantCulture))
        {
            recose.AddRange(csv.GetRecords<T>());
        }
        return recose;
    }
}
