bool sampleInput = false;
string inputpath = Directory.GetCurrentDirectory() + "\\2021\\Day 14 CSX";
string filename = inputpath + ((sampleInput) ? "\\SampleInput.txt" : "\\Input.txt");

var input = new Dictionary<string, string>();

#region Part 1
string template = null;
string originalTemplate = null;
using (var reader = new StreamReader(filename))
{
    template = reader.ReadLine();
    originalTemplate = template;
    reader.ReadLine();
    while (!reader.EndOfStream)
    {
        string line = reader.ReadLine();
        input.Add(line.Split(" -> ")[0].Trim(), line.Split(" -> ")[1].Trim());
    }
}
for (int i = 0; i < 10; i++)
{
    template = ApplyStep(template, input);
}
var distinctChars = template.Distinct().ToList();
var result = new Dictionary<char, int>();
foreach (char c in distinctChars)
{
    result.Add(c, template.Count(x => (x == c)));
}
var orderedResult = result.OrderBy(x => x.Value);
Console.WriteLine("Highest Value: " + orderedResult.Last().Value);
Console.WriteLine(" Lowest Value: " + orderedResult.First().Value);
Console.WriteLine("       Result: " + (orderedResult.Last().Value - orderedResult.First().Value));
// Wrong answer:
//   -
// Correct Answer: 2712
#endregion Part 1

#region Part 2
var allDistinctCharts = GetDistinctChars(input);
var stringPairs = GetPairsForTemplate(originalTemplate);
for (int i = 0; i < 40; i++)
{
    stringPairs = GetUpdatedPairs(stringPairs, input);
}

var orderedResult1 = allDistinctCharts.OrderBy(x => x.Value);
Console.WriteLine("Highest Value: " + orderedResult1.Last().Value);
Console.WriteLine(" Lowest Value: " + orderedResult1.First().Value);
Console.WriteLine("       Result: " + (orderedResult1.Last().Value - orderedResult1.First().Value));

// Wrong answer:
//   -
// Correct Answer: 8336623059567
#endregion Part 2

private string ApplyStep(string Template, Dictionary<string, string> Values)
{
    string newTemplate = "";
    for (int i2 = 0; i2 < Template.Length - 1; i2++)
    {
        string substring = Template.Substring(i2, 2);
        string insertion = Values[substring];
        if (i2 == 0)
            newTemplate = newTemplate + substring[0] + insertion + substring[1];
        else
            newTemplate = newTemplate + insertion + substring[1];
    }
    return newTemplate;
}
private Dictionary<string, long> GetPairsForTemplate(string Template)
{
    var pairs = new Dictionary<string, long>();
    for (int i2 = 0; i2 < Template.Length - 1; i2++)
    {
        string substring = Template.Substring(i2, 2);
        if (pairs.ContainsKey(substring))
            pairs[substring]++;
        else
            pairs.Add(substring, 1);
    }
    UpdateDistinctChars(Template);
    return pairs;
}
private Dictionary<string, long> GetUpdatedPairs(Dictionary<string, long> CurrentPairs, Dictionary<string, string> Values)
{
    var pairs = new Dictionary<string, long>();
    foreach (var s in CurrentPairs)
    {
        string newPair1 = s.Key[0].ToString() + Values[s.Key];
        string newPair2 = Values[s.Key] + s.Key[1].ToString();

        UpdateDistinctChars(Values[s.Key], s.Value);

        if (pairs.ContainsKey(newPair1))
            pairs[newPair1] += s.Value;
        else
            pairs.Add(newPair1, s.Value);

        if (pairs.ContainsKey(newPair2))
            pairs[newPair2] += s.Value;
        else
            pairs.Add(newPair2, s.Value);
    }
    return pairs;
}
private Dictionary<char, long> GetDistinctChars(Dictionary<string, string> Values)
{
    var charsDict = new Dictionary<char, long>();
    foreach (var value in Values)
    {
        foreach (var distinct1 in value.Key.Distinct().ToList())
        {
            if (!charsDict.ContainsKey(distinct1))
                charsDict.Add(distinct1, 0);
        }
        foreach (var distinct1 in value.Value.Distinct().ToList())
        {
            if (!charsDict.ContainsKey(distinct1))
                charsDict.Add(distinct1, 0);
        }
    }
    return charsDict;
}
private void UpdateDistinctChars(string s)
{
    foreach (char c in s)
    {
        allDistinctCharts[c]++;
    }
}
private void UpdateDistinctChars(string s, long value)
{
    foreach (char c in s)
    {
        allDistinctCharts[c] += value;
    }
}