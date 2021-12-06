using System.Linq;
using System.Collections;


bool sampleInput = false;
string inputpath = Directory.GetCurrentDirectory() + "\\2021\\Day 06 CSX";
string filename = inputpath + ((sampleInput) ? "\\SampleInput.txt" : "\\Input.txt");

#region Part 1

List<int> fishes;
using (var reader = new StreamReader(filename))
{
    fishes = reader.ReadLine().Split(',').Select(Int32.Parse).ToList();
}
int days = 80;
for (int i = 0; i < days; i++)
{
    int noOfFishesToAdd = 0;
    for (int i2 = 0; i2 < fishes.Count(); i2++)
    {
        if (fishes[i2] == 0)
        {
            fishes[i2] = 6;
            noOfFishesToAdd++;
        }
        else
            fishes[i2]--;
    }
    for (int i3 = 0; i3 < noOfFishesToAdd; i3++)
    {
        fishes.Add(8);
    }
}
Console.WriteLine(fishes.Count().ToString());
// Wrong answer:
//   -
// Correct Answer: 355386
#endregion Part 1

#region Part 2
Dictionary<int, long> fishesByAge = new Dictionary<int, long>();
days = 256;
using (var reader = new StreamReader(filename))
{
    fishes = reader.ReadLine().Split(',').Select(Int32.Parse).ToList();
}
fishesByAge = fishes.GroupBy(g => g).ToDictionary(d => d.Key, d => (long)d.Count());

for (int i = 0; i < days; i++)
{
    fishesByAge = GetNextDayAging(fishesByAge);
}
Console.WriteLine(fishesByAge.Values.Sum().ToString());
// Wrong answer:
//   -
// Correct Answer: 1613415325809
#endregion Part 2

private Dictionary<int, long> GetNextDayAging(Dictionary<int, long> fishesByAge)
{
    Dictionary<int, long> newFishesByAge = new Dictionary<int, long>();
    newFishesByAge.Add(8, GetValueOrDefault(fishesByAge, 0, 0));
    newFishesByAge.Add(7, GetValueOrDefault(fishesByAge, 8, 0));
    newFishesByAge.Add(6, GetValueOrDefault(fishesByAge, 0, 0) + GetValueOrDefault(fishesByAge, 7, 0));
    newFishesByAge.Add(5, GetValueOrDefault(fishesByAge, 6, 0));
    newFishesByAge.Add(4, GetValueOrDefault(fishesByAge, 5, 0));
    newFishesByAge.Add(3, GetValueOrDefault(fishesByAge, 4, 0));
    newFishesByAge.Add(2, GetValueOrDefault(fishesByAge, 3, 0));
    newFishesByAge.Add(1, GetValueOrDefault(fishesByAge, 2, 0));
    newFishesByAge.Add(0, GetValueOrDefault(fishesByAge, 1, 0));
    return newFishesByAge;
}

public static TValue GetValueOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue = default(TValue))
{
    if (dictionary == null) { throw new ArgumentNullException(nameof(dictionary)); } // using C# 6
    if (key == null) { throw new ArgumentNullException(nameof(key)); } //  using C# 6

    TValue value;
    return dictionary.TryGetValue(key, out value) ? value : defaultValue;
}