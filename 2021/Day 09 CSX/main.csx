bool sampleInput = false;
string inputpath = Directory.GetCurrentDirectory() + "\\2021\\Day 09 CSX";
string filename = inputpath + ((sampleInput) ? "\\SampleInput.txt" : "\\Input.txt");

List<string> input;

#region Part 1
input = new List<string>();
using (var reader = new StreamReader(filename))
{
    // List<int>
    // input = reader.ReadLine().Split(',').Select(Int32.Parse).ToList();
    // int[]
    // input = reader.ReadLine().Split(',').Select(Int32.Parse).ToArray<int>();
    // List<string>
    while (!reader.EndOfStream)
    {
        input.Add(reader.ReadLine());
    }
}

var lowPoints = new List<int>();
int i1 = 0;
int i2 = 0;
for (i1 = 0; i1 < input.Count ; i1++)
{
    for (i2 = 0; i2 < input[i1].Length; i2++)
    {
        if (IsLowPoint(input,i1,i2))
        {
            lowPoints.Add(1 + Int32.Parse(input[i1][i2].ToString()));
        }
    }
}
Console.WriteLine("Result: " + lowPoints.Sum());
// Wrong answer:
//   - 1830 (too high)
//   - 1765 (too high)
// Correct Answer: 500
#endregion Part 1

#region Part 2

// Wrong answer:
//   -
// Correct Answer: 
#endregion Part 2

private bool IsLowPoint(List<string> Values, int Row, int Col)
{
    var adjacentValues = GetAdjacentValues(Values, Row, Col);
    int currValue = Int32.Parse(Values[Row][Col].ToString());
    foreach (int value in adjacentValues)
    {
        if (value <= currValue)
            return false;
    }
    return true;
}

private List<int> GetAdjacentValues(List<string> Values, int Row, int Col)
{
    var adjacentValues = new List<int>();

    string prevRow = null;
    string currRow = Values[Row];
    string nextRow = null;
    if (Row > 0)
        prevRow = Values[Row - 1];
    if (Row < Values.Count - 1)
        nextRow = Values[Row + 1];

    int currValue = Int32.Parse(currRow[Col].ToString());
    int? up = null;
    int? down = null;
    int? left = null;
    int? right = null;

    if (prevRow != null)
        up = Int32.Parse(prevRow[Col].ToString());
    if (nextRow != null)
        down = Int32.Parse(nextRow[Col].ToString());
    if (Col > 0)
        left = Int32.Parse(currRow[Col - 1].ToString());
    if (Col < (currRow.Length - 1))
        right = Int32.Parse(currRow[Col + 1].ToString());

    if (up != null) { adjacentValues.Add(up.Value); };
    if (down != null) { adjacentValues.Add(down.Value); };
    if (left != null) { adjacentValues.Add(left.Value); };
    if (right != null) { adjacentValues.Add(right.Value); };
    return adjacentValues;
}