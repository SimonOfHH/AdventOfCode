using System.Drawing;
using System.Linq;
using System.IO;

bool sampleInput = false;
string inputpath = Directory.GetCurrentDirectory() + "\\2021\\Day 09 CSX";
string filename = inputpath + ((sampleInput) ? "\\SampleInput.txt" : "\\Input.txt");

List<string> input;

#region Part 1
input = new List<string>();
using (var reader = new StreamReader(filename))
{
    while (!reader.EndOfStream)
    {
        input.Add(reader.ReadLine());
    }
}

var lowPoints = new List<int>();
var lowPoints2 = new Dictionary<Point, int>(); // for Part 2
int i1 = 0;
int i2 = 0;
for (i1 = 0; i1 < input.Count ; i1++)
{
    for (i2 = 0; i2 < input[i1].Length; i2++)
    {
        if (IsLowPoint(input,i1,i2))
        {
            lowPoints.Add(1 + Int32.Parse(input[i1][i2].ToString()));
            lowPoints2.Add(new Point(i1, i2), Int32.Parse(input[i1][i2].ToString()));  // for Part 2
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
var basins = new Dictionary<Point, List<Point>>();
foreach (var pair in lowPoints2)
{
    var adjacentValues = new Dictionary<Point, int>();
    adjacentValues = GetAdjacentValues(input, adjacentValues, pair.Key, pair.Value);
    basins.Add(pair.Key, new List<Point>());
    if (adjacentValues != null)
    {
        foreach (var pair2 in adjacentValues)
        {
            if (!PointIsAlreadyPartOfBasin(basins, pair2.Key))
            {
                basins[pair.Key].Add(pair2.Key);
            }
        }
    }
}

var largestBasins = basins.OrderByDescending(o => o.Value.Count).Take(3);
int sum = 0;
foreach (var pair in largestBasins)
{
    if (sum == 0)
        sum = pair.Value.Count;
    else
        sum = sum * pair.Value.Count;
}
Console.WriteLine("Result: " + sum);
// Wrong answer:
//   - 505120 (too low)
// Correct Answer: 970200
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

private bool PointIsAlreadyPartOfBasin(Dictionary<Point, List<Point>> AllBasins, Point PointToAdd)
{
    var result = AllBasins.Where(p => p.Value.Contains(PointToAdd));
    if (result.Count() > 0)
    {
        return true;
    }
    return false;
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

private Dictionary<Point, int> GetAdjacentValues(List<string> Values, Dictionary<Point, int> Points, Point CurrPoint, int ParentValue)
{
    //var points = new Dictionary<Point,int>();
    Points = AddIfNotNull(Values, Points, new Point(CurrPoint.X - 1, CurrPoint.Y), GetValueAtPoint(Values, new Point(CurrPoint.X - 1, CurrPoint.Y)), ParentValue); // left
    Points = AddIfNotNull(Values, Points, new Point(CurrPoint.X + 1, CurrPoint.Y), GetValueAtPoint(Values, new Point(CurrPoint.X + 1, CurrPoint.Y)), ParentValue); // right
    Points = AddIfNotNull(Values, Points, new Point(CurrPoint.X, CurrPoint.Y - 1), GetValueAtPoint(Values, new Point(CurrPoint.X, CurrPoint.Y - 1)), ParentValue); // up
    Points = AddIfNotNull(Values, Points, new Point(CurrPoint.X, CurrPoint.Y + 1), GetValueAtPoint(Values, new Point(CurrPoint.X, CurrPoint.Y + 1)), ParentValue); // down
    if (Points.Count == 0) { return null; }
    return Points;
}

private Dictionary<Point, int> AddIfNotNull(List<string> Values, Dictionary<Point, int> Pairs, Point CurrPoint, int? CurrValue, int ParentValue)
{
    if (CurrValue == null) { return Pairs; }
    if (CurrValue.Value == 9) { return Pairs; }
    if (Pairs.ContainsKey(CurrPoint)) { return Pairs; }

    Pairs.Add(CurrPoint, CurrValue.Value);
    Pairs = GetAdjacentValues(Values, Pairs, CurrPoint, CurrValue.Value);
    return Pairs;
}

private int? GetValueAtPoint(List<string> Values, Point CurrPoint)
{
    int? value = null;
    try
    {
        value = Int32.Parse(Values[CurrPoint.X][CurrPoint.Y].ToString());
    }
    catch
    {
        // Do nothing, only to avoid all possible checks for valid index
    }
    return value;
}