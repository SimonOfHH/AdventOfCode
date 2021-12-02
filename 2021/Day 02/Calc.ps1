$sampleInput = $false
if ($sampleInput -eq $true) {
    $inputValues = Get-Content (Join-Path $PSScriptRoot "SampleInput.txt")
}
else {
    $inputValues = Get-Content (Join-Path $PSScriptRoot "Input.txt")
}

# Part 1 
$horizontalPos = 0
$depth = 0

foreach ($inputValue in $inputValues) {
    $operation = $inputValue.Substring(0, $inputValue.IndexOf(' '))
    [int] $value = $inputValue.Substring($inputValue.IndexOf(' ') + 1)
    switch ($operation) {
        'forward' { $horizontalPos += $value }
        'down' { $depth += $value }
        'up' { $depth -= $value }
        default {
            throw "Unhandled"
        }
    }
}
"Horizontal: $($horizontalPos)"
"Depth: $($depth)"
"Result: $($horizontalPos * $depth)"

# Wrong answer:
#  
# Correct Answer: 1893605

# Part 2
$horizontalPos = 0
$depth = 0
$aim = 0
foreach ($inputValue in $inputValues) {
    $operation = $inputValue.Substring(0, $inputValue.IndexOf(' '))
    [int] $value = $inputValue.Substring($inputValue.IndexOf(' ') + 1)
    switch ($operation) {
        'forward' { 
            $horizontalPos += $value 
            $depth += ($aim * $value)
        }
        'down' { $aim += $value }
        'up' { $aim -= $value }
        default {
            throw "Unhandled"
        }
    }
}
"Horizontal: $($horizontalPos)"
"Depth: $($depth)"
"Result: $($horizontalPos * $depth)"

# Wrong answer:
#  
# Correct Answer: 2120734350