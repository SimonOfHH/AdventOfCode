using System.Linq;

bool sampleInput = false;
string inputpath = Directory.GetCurrentDirectory() + "\\2021\\Day 08 CSX";
string filename = inputpath + ((sampleInput) ? "\\SampleInput.txt" : "\\Input.txt");

// Number | Segments
//   0    |    6
//   1    |    2 *
//   2    |    5
//   3    |    5 
//   4    |    4 *
//   5    |    5
//   6    |    6
//   7    |    3 *
//   8    |    7 *
//   9    |    6

Dictionary<List<string>, List<string>> input;
#region Part 1
input = new Dictionary<List<string>, List<string>>();
using (var reader = new StreamReader(filename))
{
    while (!reader.EndOfStream)
    {
        string line = reader.ReadLine();
        input.Add(line.Split('|')[0].Trim().Split(' ').ToList(), line.Split('|')[1].Trim().Split(' ').ToList());
    }
}
Dictionary<int, int> occurences = new Dictionary<int, int>();
foreach (var entry in input)
{
    var values = entry.Value;
    occurences = AddToDictionary(occurences, 1, values.Count(x => x.Length == 2));
    occurences = AddToDictionary(occurences, 4, values.Count(x => x.Length == 4));
    occurences = AddToDictionary(occurences, 7, values.Count(x => x.Length == 3));
    occurences = AddToDictionary(occurences, 8, values.Count(x => x.Length == 7));
}
Console.WriteLine("Result: " + occurences.Sum(x => x.Value));
// Wrong answer:
//   -
// Correct Answer: 272
#endregion Part 1

#region Part 2
input = new Dictionary<List<string>, List<string>>();
using (var reader = new StreamReader(filename))
{
    while (!reader.EndOfStream)
    {
        string line = reader.ReadLine();
        input.Add(line.Split('|')[0].Trim().Split(' ').ToList(), line.Split('|')[1].Trim().Split(' ').ToList());
    }
}
List<int> outputValues = new List<int>();
foreach (var entry in input)
{
    Digits dig = new Digits();
    // After "1", we know the values for "c" and "f"
    dig.AddKnownSegments(1, entry.Key.Where(x => x.Length == 2).First(), entry.Key);
    // After "7", we also know the values for "a"
    dig.AddKnownSegments(7, entry.Key.Where(x => x.Length == 3).First(), entry.Key);
    // After "4", we also know the values for "b" and "d"
    dig.AddKnownSegments(4, entry.Key.Where(x => x.Length == 4).First(), entry.Key);
    // Here we can guess the values for "e" and "g" since "e" should exist 4x in the inputs and "g" should exist 7x
    dig.GuessRemainingSegments(entry.Key);
    //dig.ValidateSegments(entry.Key);
    if (!dig.AllSegmentsKnown)
        throw new Exception("Should not happen");
    string concatNumber = "";
    foreach (string s in entry.Value)
    {
        concatNumber += dig.GetNumberForOutput(s).ToString();
    }
    outputValues.Add(Int32.Parse(concatNumber));
}
Console.WriteLine("Result: " + outputValues.Sum());
// Wrong answer:
//   - 1084251
// Correct Answer: 1007675
#endregion Part 2

private Dictionary<int, int> AddToDictionary(Dictionary<int, int> occ, int index, int value)
{
    if (occ.ContainsKey(index))
        occ[index] += value;
    else
        occ.Add(index, value);
    return occ;
}
public class Digits
{
    //   0:      1:      2:      3:      4:
    //  aaaa    ....    aaaa    aaaa    ....
    // b    c  .    c  .    c  .    c  b    c
    // b    c  .    c  .    c  .    c  b    c
    //  ....    ....    dddd    dddd    dddd
    // e    f  .    f  e    .  .    f  .    f
    // e    f  .    f  e    .  .    f  .    f
    //  gggg    ....    gggg    gggg    ....
    //
    //  5:      6:      7:      8:      9:
    //  aaaa    aaaa    aaaa    aaaa    aaaa
    // b    .  b    .  .    c  b    c  b    c
    // b    .  b    .  .    c  b    c  b    c
    //  dddd    dddd    ....    dddd    dddd
    // .    f  e    f  .    f  e    f  .    f
    // .    f  e    f  .    f  e    f  .    f
    //  gggg    gggg    ....    gggg    gggg
    public Dictionary<string, string> Segments { get; set; }
    public string NumberZero { get { string s = String.Format("{0}{1}{2}{3}{4}{5}", Segments["a"], Segments["b"], Segments["c"], Segments["e"], Segments["f"], Segments["g"]); return String.Concat(s.OrderBy(c => c)); } }
    public string NumberOne { get { string s = String.Format("{0}{1}", Segments["c"], Segments["f"]); return String.Concat(s.OrderBy(c => c)); } }
    public string NumberTwo { get { string s = String.Format("{0}{1}{2}{3}{4}", Segments["a"], Segments["c"], Segments["d"], Segments["e"], Segments["g"]); return String.Concat(s.OrderBy(c => c)); } }
    public string NumberThree { get { string s = String.Format("{0}{1}{2}{3}{4}", Segments["a"], Segments["c"], Segments["d"], Segments["f"], Segments["g"]); return String.Concat(s.OrderBy(c => c)); } }
    public string NumberFour { get { string s = String.Format("{0}{1}{2}{3}", Segments["b"], Segments["c"], Segments["d"], Segments["f"]); return String.Concat(s.OrderBy(c => c)); } }
    public string NumberFive { get { string s = String.Format("{0}{1}{2}{3}{4}", Segments["a"], Segments["b"], Segments["d"], Segments["f"], Segments["g"]); return String.Concat(s.OrderBy(c => c)); } }
    public string NumberSix { get { string s = String.Format("{0}{1}{2}{3}{4}{5}", Segments["a"], Segments["b"], Segments["d"], Segments["e"], Segments["f"], Segments["g"]); return String.Concat(s.OrderBy(c => c)); } }
    public string NumberSeven { get { string s = String.Format("{0}{1}{2}", Segments["a"], Segments["c"], Segments["f"]); return String.Concat(s.OrderBy(c => c)); } }
    public string NumberEight { get { string s = String.Format("{0}{1}{2}{3}{4}{5}{6}", Segments["a"], Segments["b"], Segments["c"], Segments["d"], Segments["e"], Segments["f"], Segments["g"]); return String.Concat(s.OrderBy(c => c)); } }
    public string NumberNine { get { string s = String.Format("{0}{1}{2}{3}{4}{5}", Segments["a"], Segments["b"], Segments["c"], Segments["d"], Segments["f"], Segments["g"]); return String.Concat(s.OrderBy(c => c)); } }
    public bool AllSegmentsKnown { get { return !Segments.ContainsValue(null); } }

    public Digits()
    {
        Segments = new Dictionary<string, string>();
        Segments.Add("a", null);
        Segments.Add("b", null);
        Segments.Add("c", null);
        Segments.Add("d", null);
        Segments.Add("e", null);
        Segments.Add("f", null);
        Segments.Add("g", null);
    }

    public int GetNumberForOutput(string Output)
    {
        if (NumberEqualsOutput(Output, NumberZero)) { return 0; };
        if (NumberEqualsOutput(Output, NumberOne)) { return 1; };
        if (NumberEqualsOutput(Output, NumberTwo)) { return 2; };
        if (NumberEqualsOutput(Output, NumberThree)) { return 3; };
        if (NumberEqualsOutput(Output, NumberFour)) { return 4; };
        if (NumberEqualsOutput(Output, NumberFive)) { return 5; };
        if (NumberEqualsOutput(Output, NumberSix)) { return 6; };
        if (NumberEqualsOutput(Output, NumberSeven)) { return 7; };
        if (NumberEqualsOutput(Output, NumberEight)) { return 8; };
        if (NumberEqualsOutput(Output, NumberNine)) { return 9; };
        return 0;
    }
    private bool NumberEqualsOutput(string Output, string Number)
    {
        if (Output.Length != Number.Length) { return false; }
        char[] chars1 = Output.ToArray();
        char[] chars2 = Number.ToArray();

        foreach (char c1 in chars1)
        {
            if (!chars2.Contains(c1))
                return false;
        }
        return true;
    }
    public void GuessRemainingSegments(List<string> AllInputs)
    {
        Segments["e"] = GetCharacterByOccurence(AllInputs, 4);
        Segments["g"] = GetCharacterByOccurence(AllInputs, 7);
    }
    public string GetCharacterByOccurence(List<string> AllInputs, int occurence)
    {
        string allInputsAsString = String.Join(',', AllInputs.ToArray());
        foreach (var entry in Segments.Where(x => x.Value != null))
        {
            allInputsAsString = allInputsAsString.Replace(entry.Value, "");
        }
        allInputsAsString = allInputsAsString.Replace(",", "");

        var currCharacters = allInputsAsString.Distinct().ToArray<char>();
        foreach (var char1 in currCharacters)
        {
            int counter = allInputsAsString.Count(x => x == char1);
            if (counter == occurence)
            {
                return char1.ToString();
            }
        }

        return null;
    }
    private bool MakesSense(List<string> AllInputs, string Identifier, string Output)
    {
        string allInputsAsString = String.Join(',', AllInputs.ToArray());
        int counter = allInputsAsString.Count(x => x.ToString() == Output);
        switch (Identifier)
        {
            case "a": return counter == 8; break;
            case "b": return counter == 6; break;
            case "c": return counter == 8; break;
            case "d": return counter == 7; break;
            case "e": return counter == 4; break;
            case "f": return counter == 9; break;
            case "g": return counter == 7; break;
        }
        return false;
    }
    public void AddKnownSegments(int Number, string Output, List<string> AllInputs)
    {
        switch (Number)
        {
            case 1:
                foreach (var identifier in new string[] { "c", "f" })
                {
                    for (int i = 0; i < Output.Length; i++)
                    {
                        if (Segments[identifier] == null)
                        {
                            if (MakesSense(AllInputs, identifier, Output[i].ToString()))
                            {
                                if (!Segments.ContainsValue(Output[i].ToString()))
                                {
                                    Segments[identifier] = Output[i].ToString();
                                }
                            }
                        }
                    }
                }
                break;
            case 4:
                // value for "b" should be the only one existing 6x in the inputs
                Segments["b"] = GetCharacterByOccurence(AllInputs, 6);
                foreach (var identifier in new string[] { "b", "c", "d", "f" })
                {
                    for (int i = 0; i < Output.Length; i++)
                    {
                        if (Segments[identifier] == null)
                        {
                            if (MakesSense(AllInputs, identifier, Output[i].ToString()))
                            {
                                if (!Segments.ContainsValue(Output[i].ToString()))
                                {
                                    Segments[identifier] = Output[i].ToString();
                                }
                            }
                        }
                    }
                }
                break;
            case 7:
                foreach (var identifier in new string[] { "a", "c", "f" })
                {
                    for (int i = 0; i < Output.Length; i++)
                    {
                        if (Segments[identifier] == null)
                        {
                            if (MakesSense(AllInputs, identifier, Output[i].ToString()))
                            {
                                if (!Segments.ContainsValue(Output[i].ToString()))
                                {
                                    Segments[identifier] = Output[i].ToString();
                                }
                            }
                        }
                    }
                }
                break;
        }
    }
}