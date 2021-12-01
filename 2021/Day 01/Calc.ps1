$sampleInput = $false
if ($sampleInput -eq $true) {
    [int[]] $numbers = Get-Content (Join-Path $PSScriptRoot "SampleInput.txt")
}
else {
    [int[]] $numbers = Get-Content (Join-Path $PSScriptRoot "Input.txt")
}

# Part 1
$prevNumber = 0
$increases = @()
$decreases = @()
foreach ($number in $numbers) {
    if ($prevNumber -eq 0) {
        $prevNumber = $number
    }
    if ($number -lt $prevNumber) {
        $decreases += $number
    }
    if ($number -gt $prevNumber) {
        $increases += $number
    }
    $prevNumber = $number
}
"Part 1 Increases: $($increases.Count)"
"Part 1 Decreases: $($decreases.Count)"

# Wrong answer:
#  
# Correct Answer: 1791

# Part 2
$newNumbers = @()
$tempSum1 = 0
for ($i = 0; $i -lt $numbers.Length; $i++) {
    for ($i2 = 0; $i2 -lt 3; $i2++) {
        $index = $i + $i2
        if ($index -lt $numbers.Length) {
            $tempSum1 += $numbers[$index]
        }
        else {
            $tempSum1 = 0
        }
    }
    if ($tempSum1 -ne 0) {
        $newNumbers += $tempSum1
        $tempSum1 = 0
    }
}

$prevNumber = 0
$increases = @()
$decreases = @()
foreach ($number in $newNumbers) {
    if ($prevNumber -eq 0) {
        $prevNumber = $number
    }
    if ($number -lt $prevNumber) {
        $decreases += $number
    }
    if ($number -gt $prevNumber) {
        $increases += $number
    }
    $prevNumber = $number
}
"Part 2 Increases: $($increases.Count)"
"Part 2 Decreases: $($decreases.Count)"

# Wrong answer:
#  
# Correct Answer: 1822