using System.Linq;
using System.Collections;


bool sampleInput = false;
string inputpath = Directory.GetCurrentDirectory() + "\\2021\\Day 07 CSX";
string filename = inputpath + ((sampleInput) ? "\\SampleInput.txt" : "\\Input.txt");
int[] numbers;

#region Part 1
using (var reader = new StreamReader(filename))
{
    numbers = reader.ReadLine().Split(',').Select(Int32.Parse).ToArray<int>();
}
Console.WriteLine("Fuel need: " + GetMinimumFuel(numbers, numbers.Min(), numbers.Max(), true));
// Wrong answer:
//   -
// Correct Answer: 341558
#endregion Part 1

#region Part 2
Console.WriteLine("Fuel need: " + GetMinimumFuel(numbers, numbers.Min(), numbers.Max(), false));
// Wrong answer:
//   -
// Correct Answer: 
#endregion Part 2

private int GetMinimumFuel(int[] numbers, int minPosition, int maxPosition, bool part1)
{
    int minSum = int.MaxValue;
    for (int i = minPosition; i <= maxPosition; i++) {
        int sum = GetFuelNeedForStep(numbers, i, part1);

        if (sum < minSum) 
        { 
            minSum = sum; 
        }
    }

    return minSum;
}
private int GetFuelNeedForStep(int[] numbers, int nextPosition, bool part1)
{
    int sum = 0;
    for (int i = 0; i < numbers.Length; i++) {
        int inputPosition = numbers[i];

        int distance = Math.Abs(nextPosition - inputPosition);
        if (part1)
        {
            sum += distance;
        }
        else
        {
            sum += distance * (distance + 1) / 2;
        }
    }
    return sum;
}