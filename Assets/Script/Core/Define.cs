using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public enum WindowIds
{
    None = -1,
    PlaceTowerWindow = 0
}

public static class DataTableIds
{
        public static readonly string CrewRankTable = "CrewRankTable";


        public static readonly HashSet<string> AllIds = new HashSet<string>()
        {
            CrewRankTable,
        };

        public static bool Contains(string id)
        {        
                return AllIds.Contains(id);
        }

        public static IEnumerable<string> GetAllIds()
        {
                return AllIds;
        }

}