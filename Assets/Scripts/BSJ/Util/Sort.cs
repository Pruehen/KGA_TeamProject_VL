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
                int pi = HoarePartition(arr, low, high, comparison);
                QuickSortRecursive(arr, low, pi - 1, comparison);
                QuickSortRecursive(arr, pi + 1, high, comparison);
            }
        }

        public static int LomutoPartition<T>(Span<T> arr, int low, int high, Comparison<T> comparison)
        {
            T pivot = arr[high];
            int i = low - 1;

            for (int j = low; j < high; j++)
            {
                if (comparison(arr[j], pivot) <= 0)
                {
                    i++;
                    (arr[i], arr[j]) = (arr[j], arr[i]);  // tuple Swap
                }
            }

            (arr[i + 1], arr[high]) = (arr[high], arr[i + 1]);
            return i + 1;
        }

            // 1, 7, 8, 3, 6, 2, 5
            // 1, 7, 8, 3, 6, 2, 5
            // 1, 7, 8, 3, 6, 2, 5
            // 1, 3, 8, 7, 6, 2, 5
            // 1, 3, 2, 7, 6, 8, 5
            // 1, 3, 2, 5, 6, 8, 7

        public static int HoarePartition<T>(Span<T> arr, int low, int high, Comparison<T> comparison)
        {
            int pivotIndex = low;
            T pivotValue = arr[pivotIndex];
            int leftPointer = low + 1;
            int rightPointer = high;

            while (true)
            {
                // Move leftPointer right until we find an element greater than the pivot
                while (leftPointer <= high && comparison(arr[leftPointer], pivotValue) <= 0)
                    leftPointer++;

                // Move rightPointer left until we find an element less than or equal to the pivot
                while (rightPointer > low && comparison(arr[rightPointer], pivotValue) > 0)
                    rightPointer--;

                if (leftPointer < rightPointer)
                {
                    // Swap elements at leftPointer and rightPointer
                    (arr[leftPointer], arr[rightPointer]) = (arr[rightPointer], arr[leftPointer]);
                }
                else
                {
                    // Pointers have crossed, partition is complete
                    int finalPivotPosition = rightPointer;
                    (arr[pivotIndex], arr[finalPivotPosition]) = (arr[finalPivotPosition], arr[pivotIndex]);
                    return finalPivotPosition;
                }
            }
        }
        // 5, 7, 8, 3, 6, 2, 1
        //    .              .
        // 5, 1, 8, 3, 6, 2, 7
        //       .        .  
        // 5, 1, 8, 3, 6, 2, 7
        //          .  .  
        // 5, 1, 8, 3, 6, 2, 7
        // .        .
        // 3, 1, 2, 5, 6, 8, 7
        

        //https://ldgeao99.tistory.com/376
    }
}