using CsvHelper;
using System.Globalization;
using System.IO;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using System.Linq;
using System;


public abstract class DataTable
{
    public static readonly string FormatPath = "DataTables/{0}";

    public abstract UniTask<(string, DataTable)> LoadAsync(string filename);

    public static async UniTask<List<T>> LoadCSV<T>(string csvText)
    {
        using (var reader = new StringReader(csvText))
        using (var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            var records = new List<T>();

            await foreach (var data in csvReader.GetRecordsAsync<T>())
            {
                records.Add(data);
            }
            return records;
        }
    }
}