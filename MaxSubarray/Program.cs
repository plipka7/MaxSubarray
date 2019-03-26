using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace MaxSubarray {
    class Program {
        private static readonly String[] OPTIONS = {
            "Add a number",
            "Get the max subarray brute",
            "Get the max subarray recursive",
            "Compare brute and recursive",
            "Clear the array",
            "View current array",
            "Exit"
        };

        private static List<int> numbers;

        static void Main(string[] args) {
            String userInput;
            numbers = new List<int>();

            while(true) {
                PrintOptions();
                userInput = Console.ReadLine();

                int userOption;
                if(Int32.TryParse(userInput, out userOption) && userOption >= 0 && userOption <= 6) {
                    switch(userOption) {
                        case 0:
                            Console.WriteLine("Enter a number");
                            userInput = Console.ReadLine();
                            int userNumber;
                            if(Int32.TryParse(userInput, out userNumber)) {
                                numbers.Add(userNumber);
                            } else {
                                Console.WriteLine("Invalid Number");
                            }
                            break;
                        case 1:
                            GetMaxSubarrayBruteForce();
                            break;
                        case 2:
                            int sum = GetMaxSubarrayRecursive(0, numbers.Count - 1);
                            Console.WriteLine("Max sum recursive: {0}", sum);
                            break;
                        case 3:
                            CompareBruteAndRecursive();
                            break;
                        case 4:
                            numbers.Clear();
                            break;
                        case 5:
                            Console.WriteLine("**********Array***********");
                            Console.Write("[");
                            for(int i = 0; i < numbers.Count - 1; ++i) Console.Write(numbers[i] + ", ");
                            Console.WriteLine(numbers[numbers.Count - 1] + "]");                            
                            break;
                        case 6:
                            return;
                    }
                } else {
                    Console.WriteLine("Error with number option input");
                }         
            }
        }

        private static void CompareBruteAndRecursive() {
            Stopwatch bruteWatch = new Stopwatch();
            Stopwatch recursiveWatch = new Stopwatch();
            Stopwatch linearWatch = new Stopwatch();

            Console.WriteLine("Give an array size");
            String userInput = Console.ReadLine();
            int arraySize = 0;
            Random r = new Random();
            if (Int32.TryParse(userInput, out arraySize) && arraySize > 0) {
                numbers.Clear();
                for (int i = 0; i < arraySize; ++i) numbers.Add(r.Next(-100, 100));

                /*Brute force algorithm*/
                bruteWatch.Start();
                int sum1 = GetMaxSubarrayBruteForce();
                bruteWatch.Stop();

                Console.WriteLine("Sum: {0}", sum1);

                /*Recursive algorithm*/
                recursiveWatch.Start();
                int sum2 = GetMaxSubarrayRecursive(0, numbers.Count - 1);
                recursiveWatch.Stop();

                Console.WriteLine("Sum: {0}", sum2);

                /*Linear algorithm*/
                linearWatch.Start();
                int sum3 = GetMaxSubarrayLinear();
                linearWatch.Stop();

                Console.WriteLine("Sum: {0}", sum3);


                Console.WriteLine("Brute Force time: {0}\nRecursive Time: {1}\nLinear Time: {2}", 
                    bruteWatch.Elapsed, 
                    recursiveWatch.Elapsed, 
                    linearWatch.Elapsed);

            } else {
                Console.WriteLine("Invalid array size");
            }
        }

        private static int GetMaxSubarrayRecursive(int low, int high) {
            if (low == high) return numbers[low];
            int mid = (int)Math.Floor((high + low) / 2.0);
            int leftSum = GetMaxSubarrayRecursive(low, mid);
            int rightSum = GetMaxSubarrayRecursive(mid + 1, high);
            int crossSum = FindMaxCrossover(low, high, mid);
            return Math.Max(Math.Max(leftSum, rightSum), crossSum);
        }

        private static int FindMaxCrossover(int low, int high, int mid) {
            int leftSum = Int32.MinValue;
            int rightSum = Int32.MinValue;

            int tempSum = 0;
            for(int left = mid; left >= low; --left) {
                tempSum += numbers[left];
                leftSum = tempSum > leftSum ? tempSum : leftSum;
            }

            tempSum = 0;
            for(int right = mid + 1; right <= high; ++right) {
                tempSum += numbers[right];
                rightSum = tempSum > rightSum ? tempSum : rightSum;
            }

            return leftSum + rightSum;
        }

        private static int GetMaxSubarrayLinear() {
            if (numbers.Count == 1) return numbers[0];
            int maxSum = Int32.MinValue;
            int tempSum = 0;
            for(int left = 0; left < numbers.Count - 1; ++left) {
                tempSum += numbers[left];
                maxSum = tempSum > maxSum ? tempSum : maxSum;
                tempSum = tempSum < 0 ? 0 : tempSum;
            }
            return maxSum;
        }

        private static int GetMaxSubarrayBruteForce() {
            if(numbers.Count == 0) {
                Console.WriteLine("Array is empty");
                return 0;
            }

            if(numbers.Count == 1) {
                return numbers[0];
            }

            int left = 0;
            int right = 0;
            int maxSum = Int32.MinValue;
            for(int tempLeft = 0; tempLeft < numbers.Count - 1; ++tempLeft) {
                int tempSum = numbers[tempLeft];
                if(tempSum > maxSum) {
                    left = tempLeft;
                    right = tempLeft;
                    maxSum = tempSum;
                }
                for(int tempRight = tempLeft + 1; tempRight < numbers.Count; ++tempRight) {
                    tempSum += numbers[tempRight];
                    if(tempSum > maxSum) {
                        left = tempLeft;
                        right = tempRight;
                        maxSum = tempSum;
                    }
                }
            }
            return maxSum;
        }

        private static void PrintOptions() {
            for(int i = 0; i < OPTIONS.Length; ++i) {
                Console.WriteLine("{0}: {1}", i, OPTIONS[i]);
            }
        }
    }
}
