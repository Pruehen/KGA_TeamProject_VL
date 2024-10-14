using System;
using System.Collections.Generic;
namespace BSJ.Sort
{
    public static class Sort
    {
        public static void QuickSort<T>(Span<T> arr, Comparison<T> comparison)
        {
            if (arr.Length <= 1) return;
            QuickSortRecursive(arr, 0, arr.Length - 1, comparison);
        }

        public static void QuickSortRecursive<T>(Span<T> arr, int low, int high, Comparison<T> comparison)
        {
            if (low < high)
            {
                int pi = Partition(arr, low, high, comparison);
                QuickSortRecursive(arr, low, pi - 1, comparison);
                QuickSortRecursive(arr, pi + 1, high, comparison);
            }
        }

        public static int Partition<T>(Span<T> arr, int low, int high, Comparison<T> comparison)
        {
            T pivot = arr[high];
            int i = low - 1;

            for (int j = low; j < high; j++)
            {
                if (comparison(arr[j], pivot) <= 0)
                {
                    i++;
                    (arr[i], arr[j]) = (arr[j], arr[i]);  // tuple
                }
            }

            (arr[i + 1], arr[high]) = (arr[high], arr[i + 1]);
            return i + 1;
        }
    }
}