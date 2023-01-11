using System.Collections.Generic;
using UnityEngine;

namespace HemdemGames.SplineTool
{
    public abstract class SplinePointCollection<T> where T : SplinePoint
    {
        [SerializeField] private List<T> points = new List<T>();

        public T this[int index] => points[index];
        
        public int Count => points.Count;
        
        public void Add(T point)
        {
            points.Add(point);
        }

        public void Insert(int index, T point)
        {
            points.Insert(index, point);
        }
        
        public bool Remove(T point)
        {
            return points.Remove(point);
        }

        public void Remove(int index)
        {
            points.RemoveAt(index);
        }

        public void Reverse()
        {
            points.Reverse();
        }
    }
}