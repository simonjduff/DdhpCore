using System;
using System.Collections.Generic;
using System.Linq;

namespace LegacyDataImporter.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<IEnumerable<T>> Partition<T>(this IEnumerable<T> input, int partitionSize)
        {
            var inputArray = input.ToArray();
            int partitionCount = (int)Math.Ceiling(inputArray.Length/(double)partitionSize);
            int remainder = inputArray.Length%partitionSize;
            var arrays = new T[partitionCount][];

            for (int i = 0; i < partitionCount - 1; i++)
            {
                arrays[i] = new T[partitionSize];
                Array.Copy(inputArray, i*partitionSize, arrays[i], 0, partitionSize);
            }

            var lastArrayIndex = arrays.Length - 1;
            arrays[lastArrayIndex] = new T[remainder];
            Array.Copy(inputArray, lastArrayIndex * partitionSize, arrays[lastArrayIndex], 0, remainder);

            return arrays;
        }
    }
}