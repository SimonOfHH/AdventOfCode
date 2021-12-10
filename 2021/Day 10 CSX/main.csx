using System.Drawing;
using System.Linq;
using System.IO;

bool sampleInput = false;
string inputpath = Directory.GetCurrentDirectory() + "\\2021\\Day 10 CSX";
string filename = inputpath + ((sampleInput) ? "\\SampleInput.txt" : "\\Input.txt");

List<string> input;

#region Part 1
using (var reader = new StreamReader(filename))
{
    input = new List<string>();
    while (!reader.EndOfStream)
    {
        input.Add(reader.ReadLine());
    }
}

List<string> remainingLines = new List<string>();
List<char> illegalChars = new List<char>();
foreach (string s in input)
{
    List<char> nextExpectedClosingBracketStack = new List<char>();
    bool wasIllegal = false;
    for (int i1 = 0; i1 < s.Length; i1++)
    {
        if (IsOpeningBracket(s[i1]))
        {
            nextExpectedClosingBracketStack.Add(GetClosingBracket(s[i1]));
        }
        else
        {
            if (nextExpectedClosingBracketStack[nextExpectedClosingBracketStack.Count - 1] != s[i1])
            {
                illegalChars.Add(s[i1]);
                wasIllegal = true;
                break;
            }
            else
            {
                nextExpectedClosingBracketStack.RemoveAt(nextExpectedClosingBracketStack.Count - 1);
            }
        }
    }
    if (!wasIllegal)
        remainingLines.Add(s);
}
int sum = 0;
foreach (char c in new char[] { ')', ']', '}', '>' })
{
    int occurences = illegalChars.Count(x => x == c);
    sum = sum + (occurences * GetNumericValue(c, true));
}
Console.WriteLine("Result: " + sum);

// Wrong answer:
//   -
// Correct Answer: 464991
#endregion Part 1

#region Part 2
List<long> scores = new List<long>();

foreach (string s in remainingLines)
{
    List<char> nextExpectedClosingBracketStack = new List<char>();
    for (int i1 = 0; i1 < s.Length; i1++)
    {
        if (IsOpeningBracket(s[i1]))
        {
            nextExpectedClosingBracketStack.Add(GetClosingBracket(s[i1]));
        }
        else
        {
            nextExpectedClosingBracketStack.RemoveAt(nextExpectedClosingBracketStack.Count - 1);
        }
    }
    long score = 0;
    nextExpectedClosingBracketStack.Reverse();
    foreach (char c in nextExpectedClosingBracketStack)
    {
        score = score * 5;
        score += GetNumericValue(c, false);
    }
    scores.Add(score);
    Console.WriteLine("");
}
scores.Sort();
double half = scores.Count() / 2;
Console.WriteLine("Result: " + scores[(int)Math.Ceiling(half)]);
// Wrong answer:
//   - 335579505 (too low)
// Correct Answer: 
#endregion Part 2

private int GetNumericValue(char c, bool part1)
{
    if (part1)
    {
        switch (c)
        {
            case ')': return 3;
            case ']': return 57;
            case '}': return 1197;
            case '>': return 25137;
        }
    }
    else
    {
        switch (c)
        {
            case ')': return 1;
            case ']': return 2;
            case '}': return 3;
            case '>': return 4;
        }
    }
    return 0; // Shouldn't happen, just for compiler
}

private bool IsOpeningBracket(char c)
{
    if (new char[] { '(', '[', '{', '<' }.Contains(c)) { return true; };
    return false;
}
private char GetClosingBracket(char OpeningBracket)
{
    if (OpeningBracket == '(') { return ')'; };
    if (OpeningBracket == '[') { return ']'; };
    if (OpeningBracket == '{') { return '}'; };
    if (OpeningBracket == '<') { return '>'; };
    return ')'; // Shouldn't happen, just for compiler
}

public static TValue GetValueOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue = default(TValue))
{
    if (dictionary == null) { throw new ArgumentNullException(nameof(dictionary)); } // using C# 6
    if (key == null) { throw new ArgumentNullException(nameof(key)); } //  using C# 6

    TValue value;
    return dictionary.TryGetValue(key, out value) ? value : defaultValue;
}