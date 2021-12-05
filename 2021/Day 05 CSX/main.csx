using System.Drawing;
bool sampleInput = false;
string inputpath = Directory.GetCurrentDirectory() + "\\2021\\Day 05 CSX";
string filename = inputpath + ((sampleInput) ? "\\SampleInput.txt" : "\\Input.txt");

#region Part 1
var pointHits = new Dictionary<Point, int>();
using (var reader = new StreamReader(filename))
{
    while (!reader.EndOfStream)
    {
        string[] values = reader.ReadLine().Split(" -> ");
        foreach (Point p in ToListOfPointsP1(ToPoint(values[0]), ToPoint(values[1])) ?? new List<Point>())
        {
            if (pointHits.ContainsKey(p))
                pointHits[p]++;
            else
                pointHits.Add(p, 1);
        }
    }
}
var numberOfDangerousAreas = pointHits.Count(c => c.Value > 1);
Console.WriteLine(String.Format("Dangerous areas: {0}", numberOfDangerousAreas));

// Part 1
// Wrong answer:
//   -
// Correct Answer: 5698
#endregion Part 1

#region Part 2
pointHits = new Dictionary<Point, int>();
using (var reader = new StreamReader(filename))
{
    while (!reader.EndOfStream)
    {
        string[] values = reader.ReadLine().Split(" -> ");
        foreach (Point p in ToListOfPointsP2(ToPoint(values[0]), ToPoint(values[1])) ?? new List<Point>())
        {
            if (pointHits.ContainsKey(p))
                pointHits[p]++;
            else
                pointHits.Add(p, 1);
        }
    }
}
numberOfDangerousAreas = pointHits.Count(c => c.Value > 1);
Console.WriteLine(String.Format("Dangerous areas: {0}", numberOfDangerousAreas));

// Part 2
// Wrong answer:
//   -
// Correct Answer: 15463
#endregion Part 2

private Point ToPoint(string s)
{
    int x = Convert.ToInt32(s.Split(',')[0]);
    int y = Convert.ToInt32(s.Split(',')[1]);
    return new Point(x, y);
}
private List<Point> ToListOfPointsP1(Point p1, Point p2) // for part 1
{
    if ((p1.X != p2.X) && (p1.Y != p2.Y)) // only horizontal or vertical lines
        return null;

    List<Point> points = new List<Point>();
    points.Add(p1);
    if (!p1.Equals(p2))
    {
        Point tempPoint = p1;
        do
        {
            tempPoint = GetNextPointP1(tempPoint, p2);
            points.Add(tempPoint);
        } while (!tempPoint.Equals(p2));
    }
    return points;
}
private List<Point> ToListOfPointsP2(Point p1, Point p2) // for part 2
{
    List<Point> points = new List<Point>();
    points.Add(p1);
    if (!p1.Equals(p2))
    {
        Point tempPoint = p1;
        do
        {
            tempPoint = GetNextPointP2(tempPoint, p2);
            points.Add(tempPoint);
        } while (!tempPoint.Equals(p2));
    }
    return points;
}
private Point GetNextPointP1(Point p1, Point p2) // for part 1
{
    Point tempPoint = p1;
    if (p1.X == p2.X)
    {
        if (p1.Y > p2.Y)
            tempPoint.Y--;
        else
            tempPoint.Y++;
    }
    else
    {
        if (p1.X > p2.X)
            tempPoint.X--;
        else
            tempPoint.X++;
    }
    return tempPoint;
}
private Point GetNextPointP2(Point p1, Point p2) // for part 2
{
    Point tempPoint = p1;
    if (p1.X == p2.X)
    {
        if (p1.Y > p2.Y)
            tempPoint.Y--;
        else
            tempPoint.Y++;
    }
    else if (p1.Y == p2.Y)
    {
        if (p1.X > p2.X)
            tempPoint.X--;
        else
            tempPoint.X++;
    }
    else
    {
        if (p1.Y > p2.Y)
            tempPoint.Y--;
        else
            tempPoint.Y++;
        if (p1.X > p2.X)
            tempPoint.X--;
        else
            tempPoint.X++;
    }
    return tempPoint;
}