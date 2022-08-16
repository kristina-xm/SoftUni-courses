﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace Box
{
    public class Box<T> : IComparable<T> where T : IComparable<T>
    {
        public Box(T element)
        {
            Element = element;

        }

        public Box(List<T> elementsList)
        {
            Elements = elementsList;
        }

        public T Element { get; }
        public List<T> Elements { get; }

       

        public int CountOfEl<T>(List<T> list, T readLine) where T : IComparable => list.Count(word => word.CompareTo(readLine) > 0);

        public void Swap(List<T> elements, int indexOne, int indexTwo)
        {
            T firstEl = elements[indexOne];
            elements[indexOne] = elements[indexTwo];
            elements[indexTwo] = firstEl;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (T element in Elements)
            {
                sb.AppendLine($"{element.GetType()}: {element}");
            }
            return sb.ToString().TrimEnd();
        }

        public int CompareTo(T other)
       => Element.CompareTo(other);
    }
}
