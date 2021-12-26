bool sampleInput = false;
string inputpath = Directory.GetCurrentDirectory() + "\\2021\\Day 17 CSX";
string filename = inputpath + ((sampleInput) ? "\\SampleInput.txt" : "\\Input.txt");

Area area;
using (var reader = new StreamReader(filename))
{
    area = new Area(reader.ReadLine());
}

#region Part 1
Console.WriteLine("Highest Y: " + GetHighestY(area));
// Wrong answer:
//   -
// Correct Answer: 6903
#endregion Part 1

#region Part 2
var probes = GetAllPossibleValues(area);
Console.WriteLine("Number of Velocities: " + probes.Count(x => x.InsideAreaIndicator == true));
// Wrong answer:
//   -
// Correct Answer: 2351
#endregion Part 2

public int GetHighestY(Area area)
{
    int highestY = 0;
    Console.Write("Calculating ...");
    for (int x = 0; x < 1000; x++) // Just assuming that any X higher than 1000 doesn't make sense
    {
        Console.Write(".");
        for (int y = 0; y < 1000; y++) // Just assuming that any Y higher than 1000 doesn't make sense
        {
            var probe = new Probe(x, y, area);
            if (probe.InsideAreaIndicator)
            {
                if (probe.HighestY > highestY)
                    highestY = probe.HighestY;
            }
        }
    }
    Console.WriteLine("");
    return highestY;
}
public List<Probe> GetAllPossibleValues(Area area)
{
    List<Probe> probes = new List<Probe>();
    Console.Write("Calculating ...");
    for (int x = 0; x < 1000; x++) // Just assuming that any X higher than 1000 doesn't make sense
    {
        Console.Write(".");
        for (int y = -1000; y < 1000; y++) // Just assuming that any Y higher than 1000 doesn't make sense
        {
            var probe = new Probe(x, y, area);
            probes.Add(probe);
        }
    }
    Console.WriteLine("");
    return probes;
}
public class Area
{
    public int TargetAreaXBegin { get; set; }
    public int TargetAreaXEnd { get; set; }
    public int TargetAreaYBegin { get; set; }
    public int TargetAreaYEnd { get; set; }

    public Area(string init)
    {
        init = init.Substring(init.IndexOf(":") + 1);
        var split = init.Split(',');
        var splitX = split[0].Trim().Substring(2).Split("..".ToArray(), StringSplitOptions.RemoveEmptyEntries);
        var splitY = split[1].Trim().Substring(2).Split("..".ToArray(), StringSplitOptions.RemoveEmptyEntries);
        TargetAreaXBegin = int.Parse(splitX[0]);
        TargetAreaXEnd = int.Parse(splitX[1]);
        TargetAreaYBegin = int.Parse(splitY[1]);
        TargetAreaYEnd = int.Parse(splitY[0]);
    }
}
public class Probe
{
    private bool _insideArea;
    private int OriginalVelocityX { get; set; }
    private int OriginalVelocityY { get; set; }
    public int VelocityX { get; private set; }
    public int VelocityY { get; private set; }
    public int PositionX { get; private set; }
    public int PositionY { get; private set; }
    public int HighestY { get; private set; }
    public bool InsideAreaIndicator
    {
        get { return _insideArea; }
    }
    public Probe(int velocityX, int velocityY, Area area)
    {
        PositionX = 0;
        PositionY = 0;
        VelocityX = velocityX;
        VelocityY = velocityY;
        OriginalVelocityX = VelocityX;
        OriginalVelocityY = VelocityY;
        CheckTrajectory(area);
    }
    private void CheckTrajectory(Area area)
    {
        while ((!InsideArea(area)) && (!MissedArea(area)))
        {
            CalculateNextIncrement();
            if (InsideArea(area))
                _insideArea = true;
        }
    }
    private void CalculateNextIncrement()
    {
        PositionX += VelocityX;
        PositionY += VelocityY;
        if (VelocityX > 0)
            VelocityX--;
        else if (VelocityX < 0)
            VelocityX++;
        VelocityY--;
        if (PositionY > HighestY)
            HighestY = PositionY;
    }
    private bool InsideArea(Area area)
    {
        if (((PositionX >= area.TargetAreaXBegin) && (PositionX <= area.TargetAreaXEnd)) && ((PositionY <= area.TargetAreaYBegin) && (PositionY >= area.TargetAreaYEnd)))
            return true;
        return false;
    }
    private bool MissedArea(Area area)
    {
        if (PositionX > area.TargetAreaXEnd)
            return true;
        if (PositionY < area.TargetAreaYEnd)
            return true;
        return false;
    }
    public override string ToString()
    {
        return String.Format("x={0},y={1} ({2})", OriginalVelocityX, OriginalVelocityY, _insideArea);
    }
}