using CsvHelper;
using CsvHelper.Configuration;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;


public abstract class DataTable
{
    public static readonly string FormatPath = "DataTables/{0}";

    public abstract UniTask<(string, DataTable)> LoadAsync(string filename);

    public static async UniTask<List<T>> LoadCSV<T>(string csvText)
    {
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HeaderValidated = null,
            MissingFieldFound = null
        };

        using (var reader = new StringReader(csvText))
        using (var csvReader = new CsvReader(reader, config))
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