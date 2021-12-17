bool sampleInput = false;
string inputpath = Directory.GetCurrentDirectory() + "\\2021\\Day 12 CSX";
string filename = inputpath + ((sampleInput) ? "\\SampleInput.txt" : "\\Input.txt");

var input = new Dictionary<string, string[]>();
var possiblePaths = new List<List<string>>();
var differentElements = new List<string>();
var connections = new Dictionary<string, string>();
var smallCaves = new List<string>();
#region Part 1
using (var reader = new StreamReader(filename))
{
    while (!reader.EndOfStream)
    {
        string line = reader.ReadLine();
        string[] parts = line.Split('-');
        input.Add(line, parts);

        if (!differentElements.Contains(parts[0]))
            differentElements.Add(parts[0]);
        if (!differentElements.Contains(parts[1]))
            differentElements.Add(parts[1]);
    }
}
foreach (string s in differentElements)
{
    if (s == "start")
        continue;
    if (s == "end")
        continue;
    if (s.ToUpper() == s)
        continue;
    smallCaves.Add(s);
}
var result = GetValidCombinations();
Console.WriteLine("Paths: " + result.Count());

// Wrong answer:
//   -
// Correct Answer: 4338
#endregion Part 1

#region Part 2
var result2 = GetValidCombinations2();
Console.WriteLine("Paths: " + result2.Count());
// Wrong answer:
//   -
// Correct Answer: 114189
#endregion Part 2

private List<List<string>> GetValidCombinations()
{
    var currentPath = new List<string>();
    List<string> addedPathways = new List<string>();
    currentPath.Add("start");
    var result = StartFromHere("start", currentPath, false);
    result = RemoveInvalids(result, false);
    return result;
}
private List<List<string>> GetValidCombinations2()
{
    var currentPath = new List<string>();
    List<string> addedPathways = new List<string>();
    currentPath.Add("start");
    var result = StartFromHere("start", currentPath, true);
    result = RemoveInvalids(result, true);
    return result;
}

private List<List<string>> StartFromHere(string StartingPoint, List<string> CurrentPath, bool twice)
{
    List<List<string>> newPaths = new List<List<string>>();

    foreach (var value in input.Where(k => k.Key.Contains(StartingPoint)))
    {
        List<string> newPath = new List<string>();
        newPath.AddRange(CurrentPath);
        string caveToAdd = value.Value.Where(v => v != StartingPoint).First();
        if (AddToPath(StartingPoint, CurrentPath, caveToAdd, twice))
            newPath.Add(caveToAdd);
        else
            continue;
        if (StartingPoint == "end")
            return newPaths;
        if (!IsValidPath(newPath))
            continue;
        newPaths.Add(newPath);
        newPaths.AddRange(StartFromHere(newPath.Last(), newPath, twice));
    }
    return newPaths;
}

private bool AddToPath(string StartingPoint, List<string> CurrentPath, string CaveToAdd, bool twice)
{
    if (CaveToAdd == "start")
        return false;
    if (StartingPoint == CaveToAdd)
        return false;

    if (CaveToAdd.ToLower() == CaveToAdd)
    {
        if (CaveToAdd == "end")
            return true;

        if (!twice)
        {
            if (CurrentPath.Contains(CaveToAdd))
                return false;
        }
        else
        {
            if (ContainsSmallCaveTwice(CurrentPath, CaveToAdd))
                return false;
            //if (ContainsAnySmallCaveTwice(CurrentPath))
            //    return false;
        }
    }
    return true;
}

private bool ContainsSmallCaveTwice(List<string> CurrentPath, string CaveToAdd)
{
    if (CurrentPath.Count(o => o == CaveToAdd) == 2)
        return true;
    return false;
}
private List<List<string>> RemoveInvalids(List<List<string>> paths, bool twice)
{
    List<string> tempPaths = new List<string>();
    string pathAsString = null;
    paths = paths.Where(p => p.Last() == "end").ToList();
    var validPaths = new List<List<string>>();
    for (int i = 0; i < paths.Count(); i++)
    {
        pathAsString = PathToString(paths[i]);
        //currPath = paths[i];
        bool isValid = true;
        if (!IsValidPath(paths[i])) // Search for valid connections
            isValid = false;

        if (tempPaths.Contains(pathAsString)) // Search for duplicates
            isValid = false;
        else
            tempPaths.Add(pathAsString);

        if (isValid)
            validPaths.Add(paths[i]);
    }
    return validPaths;
}
private bool IsValidPath(List<string> CurrentPath)
{
    for (int i = 0; i < CurrentPath.Count() - 1; i++)
    {
        if (!ConnectionExists(CurrentPath[i], CurrentPath[i + 1]))
            return false;
        if (SmallCavesVisitedMoreThanOnce(CurrentPath) > 1)
            return false;
    }
    return true;
}
private bool ConnectionExists(string PointA, string PointB)
{
    string tempIndex = String.Format("{0}-{1}", PointA, PointB);
    if (input.ContainsKey(tempIndex))
        return true;
    tempIndex = String.Format("{0}-{1}", PointB, PointA);
    if (input.ContainsKey(tempIndex))
        return true;
    return false;
}
private int SmallCavesVisitedMoreThanOnce(List<string> CurrentPath)
{
    int visits = 0;
    foreach (string s in smallCaves)
    {
        if (CurrentPath.Count(o => o == s) > 1)
            visits++;
        if (visits > 1)
            return visits;
    }
    return visits;
}
private string PathToString(List<string> Path)
{
    string pathWay = "";
    foreach (string s in Path)
    {
        if (!String.IsNullOrEmpty(pathWay))
            pathWay += ",";
        pathWay += s;
    }
    return pathWay;
}