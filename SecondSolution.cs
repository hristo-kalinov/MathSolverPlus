﻿using MathNet.Symbolics;
using System.Text.RegularExpressions;

namespace MySolution
{
    public class SecondSolution
    {
        public static void Main()
        {
        next:
            Console.Write("Expression: ");
            string lettersFilter = "[a-z]";
            string inputExpression = Console.ReadLine()!; //Input in format: a*x + b - c = d*x + e OR a*x + b - c
            string formattedExpression = Regex.Replace(inputExpression, lettersFilter, "x");
            Console.WriteLine($"Answer: " + SolveProblem(formattedExpression)); //Output the solution
            Console.WriteLine("Type any key to continue");
            Console.ReadKey();
            Console.Clear();
            goto next;
        }
        //Main method for solving problems
        private static string SolveProblem(string inputExpression) //Solve problem from input
        {
            if (inputExpression.Contains('=')) //Check if the problem is an equation or a simpler math problem
            {
                //Normalize the equation: a*x + b - c = d*x + e --> a*x - d*x = e - b + c (unknowns on the left, numbers on the right)
                inputExpression = NormalizeEquation(inputExpression.Split('=', StringSplitOptions.RemoveEmptyEntries)[0]!.Trim(),
                                                    inputExpression.Split('=', StringSplitOptions.RemoveEmptyEntries)[1]!.Trim());
                //Simplify left output as much as possible
                string leftOutput = SymbolicExpression
                    .Parse(inputExpression
                    .Split('=', StringSplitOptions.RemoveEmptyEntries)[0]
                    .Trim()).ToString();
                //Simplify right output as much as possible
                string rightOutput = SymbolicExpression.Parse(inputExpression
                    .Split('=', StringSplitOptions.RemoveEmptyEntries)[1]
                    .Trim()).ToString();
                Console.WriteLine($"Step 2: {string.Concat(leftOutput, " = ", rightOutput)}");
                return string.Concat(leftOutput, " = ", rightOutput); //Concat both sides to form the final equation for the solution
            }
            else
            {
                return SymbolicExpression.Parse(inputExpression).ToString(); //Solve the output as much as possible
            }
        }
        //Method for normalizing equations if need be
        private static string NormalizeEquation(string leftSide, string rightSide)
        {
            leftSide = AddSignInBeginning(leftSide);
            rightSide = AddSignInBeginning(rightSide);
            List<char> leftSideSigns = AddSignsInArray(leftSide);
            List<char> rightSideSigns = AddSignsInArray(rightSide);
            List<string> leftSideNumbers = leftSide.Split(new string[] { "+", "-", " + ", " - " }, StringSplitOptions.RemoveEmptyEntries).ToList();
            List<string> rightSideNumbers = rightSide.Split(new string[] { "+", "-", " + ", " - " }, StringSplitOptions.RemoveEmptyEntries).ToList();
            List<string> leftSideNumbersToBeRemoved = new();
            List<string> rightSideNumbersToBeRemoved = new();
            int initialLeftSideNumbersCount = leftSideNumbers.Count;
            int initialRightSideNumbersCount = rightSideNumbers.Count;
            for (int num = 0; num < initialLeftSideNumbersCount; num++)
            {
                if (!leftSideNumbers[num].Contains('x'))
                {
                    if (leftSideSigns[num] == '+')
                    {
                        rightSideNumbers.Add(string.Concat("- ", leftSideNumbers[num]));
                        leftSideNumbersToBeRemoved.Add(leftSideNumbers[num]);
                    }
                    else
                    {
                        rightSideNumbers.Add(string.Concat("+ ", leftSideNumbers[num]));
                        leftSideNumbersToBeRemoved.Add(leftSideNumbers[num]);
                    }
                }
                else if (num != 0 || leftSideSigns[0] == '-')
                {
                    if (leftSideSigns[num] == '+')
                    {
                        leftSideNumbers[num] = string.Concat("+ ", leftSideNumbers[num]);
                    }
                    else
                    {
                        if (num == 0)
                        {
                            leftSideNumbers[num] = string.Concat("-", leftSideNumbers[num]);
                        }
                        else
                        {
                            leftSideNumbers[num] = string.Concat("- ", leftSideNumbers[num]);
                        }
                    }
                }
            }
            for (int num = 0; num < initialRightSideNumbersCount; num++)
            {
                if (rightSideNumbers[num].Contains('x'))
                {
                    if (rightSideSigns[num] == '+')
                    {
                        leftSideNumbers.Add(string.Concat("- ", rightSideNumbers[num]));
                        rightSideNumbersToBeRemoved.Add(rightSideNumbers[num]);
                    }
                    else
                    {
                        leftSideNumbers.Add(string.Concat("+ ", rightSideNumbers[num]));
                        rightSideNumbersToBeRemoved.Add(rightSideNumbers[num]);
                    }
                }
                else if (num != 0 || rightSideSigns[0] == '-')
                {
                    if (rightSideSigns[num] == '+')
                    {
                        rightSideNumbers[num] = string.Concat("+ ", rightSideNumbers[num]);
                    }
                    else
                    {
                        if (num == 0)
                        {
                            rightSideNumbers[num] = string.Concat("-", rightSideNumbers[num]);
                        }
                        else
                        {
                            rightSideNumbers[num] = string.Concat("- ", rightSideNumbers[num]);
                        }
                    }
                }
            }
            leftSideNumbers = RemoveRemainingNumbersAndSigns(leftSideNumbers, leftSideNumbersToBeRemoved);
            rightSideNumbers = RemoveRemainingNumbersAndSigns(rightSideNumbers, rightSideNumbersToBeRemoved);
            Console.WriteLine($"Step 1: {string.Concat(string.Join(" ", leftSideNumbers), " = ", string.Join(" ", rightSideNumbers))}");
            return string.Concat(string.Join(" ", leftSideNumbers), " = ", string.Join(" ", rightSideNumbers)); //Returning the final equation for the solution
        }
        //Method for removal of unnecessary numbers and signs (sort of garbage collection for both sides)
        private static List<string> RemoveRemainingNumbersAndSigns(List<string> currentSideNumbers, List<string> currentSideNumbersToBeRemoved)
        {
            for (int i = 0; i < currentSideNumbersToBeRemoved.Count; i++)
            {
                if (currentSideNumbers.Contains(currentSideNumbersToBeRemoved[i]))
                {
                    currentSideNumbers.Remove(currentSideNumbersToBeRemoved[i]);
                }
            }
            for (int i = 0; i < currentSideNumbers.Count; i++)
            {
                if (currentSideNumbers[0].Contains(' ')
                    || currentSideNumbers[0].Contains('+'))
                {
                    currentSideNumbers[0] = currentSideNumbers[0].Replace('+', ' ').Trim();
                }
            }
            return currentSideNumbers;
        }
        //Add plus or minus in front of the first number so the sign becomes known for the <currentSideSigns> array
        private static string AddSignInBeginning(string side)
        {
            if (!side.StartsWith('-'))
            {
                return string.Concat("+", side);
            }
            return side;
        }
        //Add all signs from the current side to the <currentSideSigns> array
        private static List<char> AddSignsInArray(string side)
        {
            List<char> array = new();
            for (int i = 0; i < side.Length; i++)
            {
                if (side[i] == '+' || side[i] == '-')
                {
                    array.Add(side[i]);
                }
            }
            return array;
        }
    }
}