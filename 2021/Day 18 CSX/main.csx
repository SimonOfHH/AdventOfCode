//using System.Drawing;
using System;
using System.Text.RegularExpressions;

bool sampleInput = false;
string inputpath = Directory.GetCurrentDirectory() + "\\2021\\Day 18 CSX";
string filename = inputpath + ((sampleInput) ? "\\SampleInput.txt" : "\\Input.txt");

List<SnailfishNumber> input = new List<SnailfishNumber>();
List<SnailfishNumber> input2 = new List<SnailfishNumber>();

using (var reader = new StreamReader(filename))
{
    while (!reader.EndOfStream)
    {
        string line = reader.ReadLine();
        input.Add(new SnailfishNumber(line));
        input2.Add(new SnailfishNumber(line)); // For some reason the further processing updates the original "input"... so create two lists and work with input2 in part 2
    }
}

//RunTests();

#region Part 1
SnailfishNumber additionResult = null;
foreach (var number in input)
{
    if (additionResult == null)
        additionResult = number;
    else
        additionResult = SnailfishNumberCalculator.Addition(additionResult, number);
}
Console.WriteLine("Result   :" + additionResult.ToString());
Console.WriteLine("Magnitude: " + additionResult.Magnitude);
// Wrong answer:
//   -
// Correct Answer: 4008
#endregion Part 1

#region Part 2
List<int> magnitudes = new List<int>();
for (int i = 0; i < input.Count; i++)
{
    for (int i2 = 0; i2 < input.Count; i2++)
    {
        if (i == i2)
            continue;
        var number1 = new SnailfishNumber(input2[i].ToString());
        var number2 = new SnailfishNumber(input2[i2].ToString());
        var additionResult1 = SnailfishNumberCalculator.Addition(number1, number2);
        magnitudes.Add(additionResult1.Magnitude);
        var additionResult2 = SnailfishNumberCalculator.Addition(number2, number1);
        magnitudes.Add(additionResult2.Magnitude);
    }
}
var result = magnitudes.OrderByDescending(v => v).First();
Console.WriteLine("Magnitude: " + result);
// Wrong answer:
//   -
// Correct Answer: 4667
#endregion Part 2

#region Tests
private void RunTests()
{
    TestReduce();
    TestMagnitude();
    TestAdditionMultiple1();
    TestAdditionMultiple2();
    TestAdditionMultiple3();
    TestAdditionMultiple4();
    TestAddition();
}
private void TestReduce()
{
    var testValues = new Dictionary<string, string>();
    testValues.Add("[[[[[9,8],1],2],3],4]", "[[[[0,9],2],3],4]");
    testValues.Add("[7,[6,[5,[4,[3,2]]]]]", "[7,[6,[5,[7,0]]]]");
    testValues.Add("[[6,[5,[4,[3,2]]]],1]", "[[6,[5,[7,0]]],3]");
    testValues.Add("[[3,[2,[1,[7,3]]]],[6,[5,[4,[3,2]]]]]", "[[3,[2,[8,0]]],[9,[5,[4,[3,2]]]]]");
    testValues.Add("[[3,[2,[8,0]]],[9,[5,[4,[3,2]]]]]", "[[3,[2,[8,0]]],[9,[5,[7,0]]]]");
    int i = 0;
    foreach (var testValue in testValues)
    {
        var testVar = new SnailfishNumber(testValue.Key);
        if (testVar.ReducedNumber.ToString() != testValue.Value)
        {
            SnailfishNumberHelper.Reduce(testVar);
            Console.WriteLine("(Reduce) Didn't work for: " + i);
            Console.WriteLine("                 " + testValue.Key);
            Console.WriteLine("Result         : " + testVar.ReducedNumber.ToString());
            Console.WriteLine("Expected       : " + testValue.Value);
        }
        else
        {
            Console.WriteLine("(Reduce) Success!");
        }
        i++;
    }
}
private void TestMagnitude()
{
    var testValues = new Dictionary<string, int>();
    testValues.Add("[[1,2],[[3,4],5]]", 143);
    testValues.Add("[[[[0,7],4],[[7,8],[6,0]]],[8,1]]", 1384);
    testValues.Add("[[[[1,1],[2,2]],[3,3]],[4,4]]", 445);
    testValues.Add("[[[[3,0],[5,3]],[4,4]],[5,5]]", 791);
    testValues.Add("[[[[5,0],[7,4]],[5,5]],[6,6]]", 1137);
    testValues.Add("[[[[8,7],[7,7]],[[8,6],[7,7]]],[[[0,7],[6,6]],[8,7]]]", 3488);
    int i = 0;
    foreach (var testValue in testValues)
    {
        var testVar = new SnailfishNumber(testValue.Key);
        if (testVar.Magnitude != testValue.Value)
        {
            Console.WriteLine("(Magnitude) Didn't work for: " + i);
            Console.WriteLine("                 " + testValue.Key);
            Console.WriteLine("Result         : " + testVar.Magnitude.ToString());
            Console.WriteLine("Expected       : " + testValue.Value);
        }
        else
        {
            Console.WriteLine("(Magnitude) Success!");
        }
        i++;
    }
}
private void TestAddition()
{
    var testValues = new Dictionary<string[], string>();
    testValues.Add(new string[] { "[[[[4,3],4],4],[7,[[8,4],9]]]", "[1,1]", }, "[[[[0,7],4],[[7,8],[6,0]]],[8,1]]");
    testValues.Add(new string[] { "[[[0,[4,5]],[0,0]],[[[4,5],[2,6]],[9,5]]]", "[7,[[[3,7],[4,3]],[[6,3],[8,8]]]]", }, "[[[[4,0],[5,4]],[[7,7],[6,0]]],[[8,[7,7]],[[7,9],[5,0]]]]");
    testValues.Add(new string[] { "[[[[4,0],[5,4]],[[7,7],[6,0]]],[[8,[7,7]],[[7,9],[5,0]]]]", "[[2,[[0,8],[3,4]]],[[[6,7],1],[7,[1,6]]]]", }, "[[[[6,7],[6,7]],[[7,7],[0,7]]],[[[8,7],[7,7]],[[8,8],[8,0]]]]");
    testValues.Add(new string[] { "[[[[6,7],[6,7]],[[7,7],[0,7]]],[[[8,7],[7,7]],[[8,8],[8,0]]]]", "[[[[2,4],7],[6,[0,5]]],[[[6,8],[2,8]],[[2,1],[4,5]]]]", }, "[[[[7,0],[7,7]],[[7,7],[7,8]]],[[[7,7],[8,8]],[[7,7],[8,7]]]]");
    testValues.Add(new string[] { "[[[[7,0],[7,7]],[[7,7],[7,8]]],[[[7,7],[8,8]],[[7,7],[8,7]]]]", "[7,[5,[[3,8],[1,4]]]]", }, "[[[[7,7],[7,8]],[[9,5],[8,7]]],[[[6,8],[0,8]],[[9,9],[9,0]]]]");
    testValues.Add(new string[] { "[[[[7,7],[7,8]],[[9,5],[8,7]]],[[[6,8],[0,8]],[[9,9],[9,0]]]]", "[[2,[2,2]],[8,[8,1]]]", }, "[[[[6,6],[6,6]],[[6,0],[6,7]]],[[[7,7],[8,9]],[8,[8,1]]]]");
    testValues.Add(new string[] { "[[[[6,6],[6,6]],[[6,0],[6,7]]],[[[7,7],[8,9]],[8,[8,1]]]]", "[2,9]", }, "[[[[6,6],[7,7]],[[0,7],[7,7]]],[[[5,5],[5,6]],9]]");
    testValues.Add(new string[] { "[[[[6,6],[7,7]],[[0,7],[7,7]]],[[[5,5],[5,6]],9]]", "[1,[[[9,3],9],[[9,0],[0,7]]]]", }, "[[[[7,8],[6,7]],[[6,8],[0,8]]],[[[7,7],[5,0]],[[5,5],[5,6]]]]");
    testValues.Add(new string[] { "[[[[7,8],[6,7]],[[6,8],[0,8]]],[[[7,7],[5,0]],[[5,5],[5,6]]]]", "[[[5,[7,4]],7],1]", }, "[[[[7,7],[7,7]],[[8,7],[8,7]]],[[[7,0],[7,7]],9]]");
    testValues.Add(new string[] { "[[[[7,7],[7,7]],[[8,7],[8,7]]],[[[7,0],[7,7]],9]]", "[[[[4,2],2],6],[8,7]]", }, "[[[[8,7],[7,7]],[[8,6],[7,7]]],[[[0,7],[6,6]],[8,7]]]");
    int i = 0;
    foreach (var testValue in testValues)
    {
        SnailfishNumber number1 = new SnailfishNumber(testValue.Key[0]);
        SnailfishNumber number2 = new SnailfishNumber(testValue.Key[1]);
        var testVar = SnailfishNumberCalculator.Addition(number1, number2);
        if (testVar == null)
        {
            Console.WriteLine("(Addition) Didn't work for: " + i);
        }
        else if (testVar.ToString() != testValue.Value)
        {
            Console.WriteLine("(Addition) Didn't work for: " + i);
            Console.WriteLine("                 " + testValue.Key[0]);
            Console.WriteLine("               + " + testValue.Key[1]);
            Console.WriteLine("Expected       : " + testValue.Value);
            Console.WriteLine("Result         : " + testVar.ToString());
        }
        else
        {
            Console.WriteLine("(Addition) Success!");
        }
        i++;
    }
}
private void TestAdditionMultiple1()
{
    var testValues = new List<string>();
    testValues.Add("[1,1]");
    testValues.Add("[2,2]");
    testValues.Add("[3,3]");
    testValues.Add("[4,4]");
    var endResult = "[[[[1,1],[2,2]],[3,3]],[4,4]]";
    SnailfishNumber additionResult = null;
    foreach (string s in testValues)
    {
        var nextNumber = new SnailfishNumber(s);
        if (additionResult == null)
            additionResult = nextNumber;
        else
            additionResult = SnailfishNumberCalculator.Addition(additionResult, nextNumber);
    }
    if (additionResult.ToString() == endResult)
    {
        Console.WriteLine("(Addition Multiple) Success!");
    }
    else
    {
        Console.WriteLine("(Addition Multiple) Didn't work");
    }
}
private void TestAdditionMultiple2()
{
    var testValues = new List<string>();
    testValues.Add("[1,1]");
    testValues.Add("[2,2]");
    testValues.Add("[3,3]");
    testValues.Add("[4,4]");
    testValues.Add("[5,5]");
    var endResult = "[[[[3,0],[5,3]],[4,4]],[5,5]]";
    SnailfishNumber additionResult = null;
    foreach (string s in testValues)
    {
        var nextNumber = new SnailfishNumber(s);
        if (additionResult == null)
            additionResult = nextNumber;
        else
            additionResult = SnailfishNumberCalculator.Addition(additionResult, nextNumber);
    }
    if (additionResult.ToString() == endResult)
    {
        Console.WriteLine("(Addition Multiple) Success!");
    }
    else
    {
        Console.WriteLine("(Addition Multiple) Didn't work");
    }
}
private void TestAdditionMultiple3()
{
    var testValues = new List<string>();
    testValues.Add("[1,1]");
    testValues.Add("[2,2]");
    testValues.Add("[3,3]");
    testValues.Add("[4,4]");
    testValues.Add("[5,5]");
    testValues.Add("[6,6]");
    var endResult = "[[[[5,0],[7,4]],[5,5]],[6,6]]";
    SnailfishNumber additionResult = null;
    foreach (string s in testValues)
    {
        var nextNumber = new SnailfishNumber(s);
        if (additionResult == null)
            additionResult = nextNumber;
        else
            additionResult = SnailfishNumberCalculator.Addition(additionResult, nextNumber);
    }
    if (additionResult.ToString() == endResult)
    {
        Console.WriteLine("(Addition Multiple) Success!");
    }
    else
    {
        Console.WriteLine("(Addition Multiple) Didn't work");
    }
}
private void TestAdditionMultiple4()
{
    var testValues = new List<string>();
    testValues.Add("[[[0,[4,5]],[0,0]],[[[4,5],[2,6]],[9,5]]]");
    testValues.Add("[7,[[[3,7],[4,3]],[[6,3],[8,8]]]]");
    testValues.Add("[[2,[[0,8],[3,4]]],[[[6,7],1],[7,[1,6]]]]");
    testValues.Add("[[[[2,4],7],[6,[0,5]]],[[[6,8],[2,8]],[[2,1],[4,5]]]]");
    testValues.Add("[7,[5,[[3,8],[1,4]]]]");
    testValues.Add("[[2,[2,2]],[8,[8,1]]]");
    testValues.Add("[2,9]");
    testValues.Add("[1,[[[9,3],9],[[9,0],[0,7]]]]");
    testValues.Add("[[[5,[7,4]],7],1]");
    testValues.Add("[[[[4,2],2],6],[8,7]]");
    var endResult = "[[[[8,7],[7,7]],[[8,6],[7,7]]],[[[0,7],[6,6]],[8,7]]]";
    SnailfishNumber additionResult = null;
    foreach (string s in testValues)
    {
        var nextNumber = new SnailfishNumber(s);
        if (additionResult == null)
            additionResult = nextNumber;
        else
            additionResult = SnailfishNumberCalculator.Addition(additionResult, nextNumber);
    }
    if (additionResult.ToString() == endResult)
    {
        Console.WriteLine("(Addition Multiple) Success!");
    }
    else
    {
        Console.WriteLine("(Addition Multiple) Didn't work");
    }
}
#endregion
public class SnailfishNumber
{
    public Guid UniqueId { get; set; }
    public SnailfishNumber X { get; set; }
    public SnailfishNumber Y { get; set; }
    public SnailfishNumber Parent { get; set; }
    public string LiteralValue { get; set; }
    public int NestingLevel { get; set; }
    public bool FromSplit { get; set; }
    public bool PrintIntermediateResults { get; set; }
    public int LiteralValueAsInt
    {
        get
        {
            if (LiteralValue != null)
                return Int32.Parse(LiteralValue);
            else
                return 0;
        }
    }
    public bool IsLiteral
    {
        get
        {
            return LiteralValue != null ? true : false;
        }
    }
    public int Magnitude
    {
        get
        {
            if (IsLiteral)
                return LiteralValueAsInt;
            else
            {
                int xAsInt = X.Magnitude;
                int yAsInt = Y.Magnitude;
                return ((3 * xAsInt) + (2 * yAsInt));
            }
        }
    }
    public SnailfishNumber ReducedNumber
    {
        get
        {
            return Reduce();
        }
    }
    public SnailfishNumber CompletelyReducedNumber
    {
        get
        {
            return SnailfishNumberHelper.ReduceCompletely(this);
        }
    }

    #region Constructors 
    public SnailfishNumber()
    {
        this.UniqueId = Guid.NewGuid();
    }
    public SnailfishNumber(string value)
    {
        this.UniqueId = Guid.NewGuid();
        this.NestingLevel++;
        Initialize(value);
        if (X != null)
            if (!X.IsLiteral)
                X.Parent = this;
        if (Y != null)
            if (!Y.IsLiteral)
                Y.Parent = this;
    }
    public SnailfishNumber(string value, int nestingLevel)
    {
        this.UniqueId = Guid.NewGuid();
        this.NestingLevel = nestingLevel + 1;
        Initialize(value);
        if (X != null)
            //if (!X.IsLiteral)
            X.Parent = this;
        if (Y != null)
            //if (!Y.IsLiteral)
            Y.Parent = this;
    }
    public SnailfishNumber(int number)
    {
        this.UniqueId = Guid.NewGuid();
        this.NestingLevel++;
        LiteralValue = number.ToString();
    }
    public SnailfishNumber(SnailfishNumber x, SnailfishNumber y)
    {
        this.UniqueId = Guid.NewGuid();
        this.NestingLevel++;
        this.X = SnailfishNumberHelper.IncreaseAllChildNestingLevels(x);
        this.Y = SnailfishNumberHelper.IncreaseAllChildNestingLevels(y);
        if (this.X != null)
            //if (!X.IsLiteral)
            X.Parent = this;
        if (this.Y != null)
            //if (!Y.IsLiteral)
            Y.Parent = this;
    }
    #endregion
    private SnailfishNumber Reduce()
    {
        return SnailfishNumberHelper.Reduce(this);
    }
    private void Initialize(string InputNumber)
    {
        if (!InputNumber.Contains(","))
        {
            LiteralValue = InputNumber;
            return;
        }
        string x = "";
        string y = "";
        int noOfOpenBrackets = 0;
        int noOfCloseBrackets = 0;
        bool addToX = true;
        for (int i = 0; i < InputNumber.Length; i++)
        {
            if (InputNumber[i] == '[')
            {
                noOfOpenBrackets++;
            }
            if (InputNumber[i] == ']')
            {
                noOfCloseBrackets++;
            }
            if (noOfOpenBrackets <= noOfCloseBrackets)
                break;
            if ((noOfOpenBrackets - noOfCloseBrackets == 1) && (InputNumber[i] == ','))
            {
                addToX = false;
                continue;
            }
            if (addToX)
            {
                x += InputNumber[i].ToString();
            }
            else
                y += InputNumber[i].ToString();
        }
        x = x.Substring(1); // Remove first "["
        X = new SnailfishNumber(x, this.NestingLevel);
        Y = new SnailfishNumber(y, this.NestingLevel);
    }

    #region Overrides
    public override string ToString()
    {
        if (LiteralValue != null)
            return String.Format("{0}", LiteralValue);
        return String.Format("[{0},{1}]", X, Y);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(UniqueId);
    }

    private bool Equals(SnailfishNumber o)
    {
        return this.UniqueId.Equals(o.UniqueId);
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((SnailfishNumber)obj);
    }
    #endregion
}
public static class SnailfishNumberHelper
{
    private static int GetNumberOfFourPairNesting(SnailfishNumber Number)
    {
        var pair = GetChildNumbers(Number).Where(o => !o.IsLiteral && o.X.IsLiteral && o.Y.IsLiteral && (o.NestingLevel == 5));
        return pair.Count();
    }
    public static bool ContainsFourPairNesting(SnailfishNumber Number)
    {
        return GetNumberOfFourPairNesting(Number) > 0;
    }
    public static SnailfishNumber Reduce(SnailfishNumber ToReduce)
    {
        if (ContainsFourPairNesting(ToReduce))
        {
            var returnValue = Explode(ToReduce);
            if (ToReduce.PrintIntermediateResults)
                Console.WriteLine("after explode: " + returnValue.ToString());
            return returnValue;
        }
        if (ToReduce.LiteralValueAsInt > 10)
        {
            var returnValue = Split(ToReduce);
            if (ToReduce.PrintIntermediateResults)
                Console.WriteLine("after split  : " + returnValue.ToString());
            return returnValue;
        }
        if (AnyNumberOver10(ToReduce))
        {
            var returnValue = SplitFirst(ToReduce);
            if (ToReduce.PrintIntermediateResults)
                Console.WriteLine("after split  : " + returnValue.ToString());
            return returnValue;
        }
        if (ToReduce.IsLiteral)
            return ToReduce;
        return ToReduce; // never hit, just for compiler here
    }
    public static SnailfishNumber ReduceCompletely(SnailfishNumber ToReduce)
    {
        var ReturnValue = ToReduce;
        while (CanReduce(ReturnValue))
            ReturnValue = Reduce(ReturnValue);
        return ReturnValue;
    }
    internal static bool CanReduce(SnailfishNumber ToReduce)
    {
        if (ContainsFourPairNesting(ToReduce))
            return true;
        if (AnyNumberOver10(ToReduce))
            return true;
        if (ToReduce.LiteralValueAsInt > 10)
            return true;
        return false;
    }
    private static bool AnyNumberOver10(SnailfishNumber Number)
    {
        var childNumbers = GetChildNumbers(Number);
        return childNumbers.Count(x => x.LiteralValueAsInt >= 10) > 0;
    }
    public static SnailfishNumber SplitFirst(SnailfishNumber ToSplit)
    {
        if (ToSplit.FromSplit)
            return ToSplit;
        var result = GetChildNumbers(ToSplit);
        var result1 = result.First(o => o.IsLiteral && o.LiteralValueAsInt >= 10);
        var splitted = Split(result1);
        var replaced = ReplaceChildNumber(ToSplit, result1, splitted);
        replaced = RebuildNestingLevelAndParents(replaced);
        return replaced;
    }
    public static SnailfishNumber Split(SnailfishNumber ToSplit)
    {
        int originalNumberOfFourPairNesting = GetNumberOfFourPairNesting(ToSplit);
        if (!ToSplit.IsLiteral)
            return ToSplit;
        if (ToSplit.LiteralValueAsInt < 10)
            return ToSplit;
        double newX = Math.Floor((double)ToSplit.LiteralValueAsInt / 2);
        double newY = Math.Ceiling((double)ToSplit.LiteralValueAsInt / 2);
        string pairString = String.Format("[{0},{1}]", newX, newY);
        var newNumber = new SnailfishNumber(pairString);
        newNumber.FromSplit = true;
        newNumber.Parent = ToSplit.Parent;
        newNumber = RebuildNestingLevelAndParents(newNumber);
        return newNumber;
    }
    public static SnailfishNumber Explode(SnailfishNumber SourceNumber)
    {
        /*
        To explode a pair, the pair's left value is added to the first regular number to the left of the exploding pair (if any), 
        and the pair's right value is added to the first regular number to the right of the exploding pair (if any). 
        Exploding pairs will always consist of two regular numbers. 
        Then, the entire exploding pair is replaced with the regular number 0.
        */
        SnailfishNumber explodingPair = null;
        SnailfishNumber leftValue = null;
        SnailfishNumber rightValue = null;
        GetExplodingPair(SourceNumber, ref explodingPair);
        GetValueToTheLeft(explodingPair, ref leftValue);
        GetValueToTheRight(explodingPair, ref rightValue);
        var newNumber = SnailfishNumberHelper.CreateNewNumberWithReplacedValues(SourceNumber, explodingPair, leftValue, rightValue);
        newNumber = RebuildNestingLevelAndParents(newNumber);
        return newNumber;
    }
    public static SnailfishNumber CreateNewNumberWithReplacedValues(SnailfishNumber SourceNumber, SnailfishNumber ExplodingPair, SnailfishNumber LeftValue, SnailfishNumber RightValue)
    {
        SnailfishNumber newLeftValue = null;
        SnailfishNumber newRightValue = null;
        SnailfishNumber newNumber = SourceNumber;
        if (LeftValue != null)
        {
            newLeftValue = new SnailfishNumber(LeftValue.LiteralValueAsInt + ExplodingPair.X.LiteralValueAsInt);
            newLeftValue.Parent = LeftValue.Parent;
        }
        if (RightValue != null)
        {
            newRightValue = new SnailfishNumber(RightValue.LiteralValueAsInt + ExplodingPair.Y.LiteralValueAsInt);
            newRightValue.Parent = RightValue.Parent;
        }
        var newExplodingPair = new SnailfishNumber(0);
        newExplodingPair.Parent = ExplodingPair.Parent;
        if (newLeftValue != null)
            newNumber = ReplaceChildNumber(newNumber, LeftValue, newLeftValue);
        if (newRightValue != null)
            newNumber = ReplaceChildNumber(newNumber, RightValue, newRightValue);
        newNumber = ReplaceChildNumber(newNumber, ExplodingPair, newExplodingPair);
        newNumber = RebuildNestingLevelAndParents(newNumber);
        return newNumber;
    }
    private static List<SnailfishNumber> GetChildNumbers(SnailfishNumber Number)
    {
        var list = new List<SnailfishNumber>();
        list.Add(null);
        if (Number == null)
            return list;
        if (Number.X != null)
            if (!list.Contains(Number.X))
                list.Add(Number.X);
        foreach (var item in GetChildNumbers(list.Last()))
        {
            if (!list.Contains(item))
                list.Add(item);
        }
        if (Number.Y != null)
            if (!list.Contains(Number.Y))
                list.Add(Number.Y);
        foreach (var item in GetChildNumbers(list.Last()))
        {
            if (!list.Contains(item))
                list.Add(item);
        }
        list = list.Where(o => o != null).ToList<SnailfishNumber>();
        return list;
    }
    private static bool NumberNeedsToBeReplaced(SnailfishNumber SourceNumber, SnailfishNumber ExplodingPair, SnailfishNumber LeftValue, SnailfishNumber RightValue)
    {
        if (SourceNumber.Equals(ExplodingPair))
            return true;
        if (LeftValue != null)
            if (SourceNumber.Equals(LeftValue))
                return true;
        if (RightValue != null)
            if (SourceNumber.Equals(RightValue))
                return true;
        return false;
    }
    private static SnailfishNumber ReplaceChildNumber(SnailfishNumber SourceNumber, SnailfishNumber ReplaceFrom, SnailfishNumber ReplaceTo)
    {
        if (SourceNumber.Equals(ReplaceFrom))
            return ReplaceTo;
        if (SourceNumber.X != null)
            SourceNumber.X = ReplaceChildNumber(SourceNumber.X, ReplaceFrom, ReplaceTo);
        if (SourceNumber.Y != null)
            SourceNumber.Y = ReplaceChildNumber(SourceNumber.Y, ReplaceFrom, ReplaceTo);
        return SourceNumber;
    }
    private static void GetValueToTheLeft(SnailfishNumber StartingValue, ref SnailfishNumber ValueToTheLeft)
    {
        var currentNumber = StartingValue.Parent;
        while ((ValueToTheLeft == null) && (currentNumber != null))
        {
            if (currentNumber.X.IsLiteral)
                ValueToTheLeft = currentNumber.X;
            else
            {
                if (NumberContainsChild(currentNumber.Y, StartingValue))
                    ValueToTheLeft = GetLastLiteral(currentNumber.X);
                else
                    currentNumber = currentNumber.Parent;
            }
        }
        ExceptionIfNotLiteral(ValueToTheLeft);
    }
    private static void GetValueToTheRight(SnailfishNumber StartingValue, ref SnailfishNumber ValueToTheLeft)
    {
        var currentNumber = StartingValue.Parent;
        while ((ValueToTheLeft == null) && (currentNumber != null))
        {
            if (currentNumber.Y.IsLiteral)
                ValueToTheLeft = currentNumber.Y;
            else
            {
                if (NumberContainsChild(currentNumber.X, StartingValue))
                    ValueToTheLeft = GetFirstLiteral(currentNumber.Y);
                else
                    currentNumber = currentNumber.Parent;
            }
        }
        ExceptionIfNotLiteral(ValueToTheLeft);
    }
    private static SnailfishNumber GetFirstLiteral(SnailfishNumber StartingValue)
    {
        if (StartingValue.X != null)
            return GetFirstLiteral(StartingValue.X);
        return StartingValue;
    }
    private static SnailfishNumber GetLastLiteral(SnailfishNumber StartingValue)
    {
        if (StartingValue.Y != null)
            return GetLastLiteral(StartingValue.Y);
        return StartingValue;
    }
    private static bool NumberContainsChild(SnailfishNumber SourceNumber, SnailfishNumber ChildNumber)
    {
        bool found = false;
        if (SourceNumber.Equals(ChildNumber))
            return true;
        if (SourceNumber.X != null)
            found = NumberContainsChild(SourceNumber.X, ChildNumber);
        if (SourceNumber.Y != null)
            found = found || NumberContainsChild(SourceNumber.Y, ChildNumber);
        return found;
    }
    private static void GetExplodingPair(SnailfishNumber SourceNumber, ref SnailfishNumber ExplodingPair)
    {
        ExplodingPair = GetChildNumbers(SourceNumber).FirstOrDefault(o => !o.IsLiteral && o.X.IsLiteral && o.Y.IsLiteral && o.NestingLevel == 5);
        if (ExplodingPair == null)
            throw new ArgumentNullException("ExplodingPair");
        ExceptionIfNotLiteral(ExplodingPair.X);
        ExceptionIfNotLiteral(ExplodingPair.Y);
    }
    private static SnailfishNumber RebuildNestingLevelAndParents(SnailfishNumber SourceNumber)
    {
        SourceNumber = RebuildNestingLevel(SourceNumber, 0);
        SourceNumber = RebuildParents(SourceNumber, null);
        return SourceNumber;
    }
    private static SnailfishNumber RebuildNestingLevel(SnailfishNumber SourceNumber, int NestingLevel)
    {
        NestingLevel++;
        SourceNumber.NestingLevel = NestingLevel;
        if (SourceNumber.X != null)
            SourceNumber.X = RebuildNestingLevel(SourceNumber.X, NestingLevel);
        if (SourceNumber.Y != null)
            SourceNumber.Y = RebuildNestingLevel(SourceNumber.Y, NestingLevel);
        return SourceNumber;
    }
    private static SnailfishNumber RebuildParents(SnailfishNumber SourceNumber, SnailfishNumber Parent)
    {
        if (Parent != null)
            SourceNumber.Parent = Parent;
        if (SourceNumber.X != null)
            SourceNumber.X = RebuildParents(SourceNumber.X, SourceNumber);
        if (SourceNumber.Y != null)
            SourceNumber.Y = RebuildParents(SourceNumber.Y, SourceNumber);
        return SourceNumber;
    }
    public static SnailfishNumber IncreaseAllChildNestingLevels(SnailfishNumber SourceNumber)
    {
        SourceNumber.NestingLevel++;
        if (SourceNumber.X != null)
            SourceNumber.X = IncreaseAllChildNestingLevels(SourceNumber.X);
        if (SourceNumber.Y != null)
            SourceNumber.Y = IncreaseAllChildNestingLevels(SourceNumber.Y);
        return SourceNumber;
    }
    private static void ExceptionIfNotLiteral(SnailfishNumber number)
    {
        if (number != null)
            if (!number.IsLiteral)
                throw new Exception("Should be a literal value");
    }
}
public static class SnailfishNumberCalculator
{
    public static SnailfishNumber Addition(SnailfishNumber Number1, SnailfishNumber Number2)
    {
        return Addition(Number1, Number2, false);
    }
    public static SnailfishNumber Addition(SnailfishNumber Number1, SnailfishNumber Number2, bool PrintIntermediateResults)
    {
        SnailfishNumber newNumber = new SnailfishNumber(Number1, Number2);
        newNumber.PrintIntermediateResults = PrintIntermediateResults;
        return newNumber.CompletelyReducedNumber;
    }
}
public enum ReduceAction
{
    None,
    Explode,
    Split
}