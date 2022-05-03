using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DefaultNamespace
{
    public static class ExtensionMethods
    {
        public static T PickOne<T>(this IEnumerable<T> source) => 
            source.ToArray()[Random.Range(0, source.Count())];
    }
}