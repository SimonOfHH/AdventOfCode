using System.Drawing;
using System.IO;

bool sampleInput = false;
string inputpath = Directory.GetCurrentDirectory() + "\\2021\\Day 13 CSX";
string filename = inputpath + ((sampleInput) ? "\\SampleInput.txt" : "\\Input.txt");

var input = new List<Point>();
var input2 = new List<string>();

#region Part 1
using (var reader = new StreamReader(filename))
{
    while (!reader.EndOfStream)
    {
        string line = reader.ReadLine();
        if (!String.IsNullOrEmpty(line))
        {
            if (line.Contains("fold"))
            {
                input2.Add(line);
            }
            else
            {
                Point p = new Point(line.Split(',').Select(Int32.Parse).ToArray<int>()[0], line.Split(',').Select(Int32.Parse).ToArray<int>()[1]);
                input.Add(p);
            }
        }
    }
}
var result = Fold(input, input2.First());
Console.WriteLine("No of Dots: " + result.Count());
// Wrong answer:
//   -
// Correct Answer: 765
#endregion Part 1

#region Part 2
var points = new List<Point>();
points.AddRange(input);
foreach (var s in input2)
{
    points = Fold(points, s);
}

PrintPoints(points);
// Wrong answer:
//   -
// Correct Answer: RZKZLPGH
#endregion Part 2

private List<Point> Fold(List<Point> Points, string Instruction)
{
    Instruction = Instruction.Replace("fold along ", "");
    var split = Instruction.Split("=");
    return Fold(Points, split[0], Int32.Parse(split[1]));
}

private List<Point> Fold(List<Point> Points, string Direction, int Value)
{
    var pointMapping = GetPointSwitchMapping(Points, Direction, Value);
    var foldedPoints = new List<Point>();
    foreach (var p in Points)
    {
        Point newPoint;
        if (Direction == "x")
            newPoint = new Point(pointMapping[p.X], p.Y);
        else
            newPoint = new Point(p.X, pointMapping[p.Y]);
        if (!foldedPoints.Contains(newPoint))
            foldedPoints.Add(newPoint);
    }
    return foldedPoints;
}

private Dictionary<int, int> GetPointSwitchMapping(List<Point> Points, string Direction, int Value)
{
    var highestValue = 0;
    if (Direction == "x")
        highestValue = Points.OrderBy(p => p.X).Last().X;
    else
        highestValue = Points.OrderBy(p => p.Y).Last().Y;
    var pointMapping = new Dictionary<int, int>();
    for (int i = 0; i <= highestValue; i++)
    {
        if (i > Value)
        {
            pointMapping.Add(i, highestValue - i);
        }
        else
        {
            pointMapping.Add(i, i); // stays the same
        }
    }
    return pointMapping;
}

private void PrintPoints(List<Point> Points)
{
    var highestX = Points.OrderBy(p => p.X).Last().X;
    var highestY = Points.OrderBy(p => p.Y).Last().Y;
    var sb = new StringBuilder();
    for (int i1 = 0; i1 <= highestY; i1++)
    {
        for (int i2 = 0; i2 <= highestX; i2++)
        {
            if (Points.Contains(new Point(i2, i1)))
                sb.Append("#");
            else
                sb.Append(".");
        }
        sb.Append(Environment.NewLine);
    }
    File.WriteAllText(inputpath + "\\" + Points.Count() + ".txt", sb.ToString());
}