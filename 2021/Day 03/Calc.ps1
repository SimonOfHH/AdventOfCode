function Get-CommonNumberAtPosition() {
    param(
        [string[]] $arrayToCheck,
        [int]$positionToCheck,
        [ValidateSet("High", "Low")]
        $keepValue
    )
    $count0 = 0
    $count1 = 0
    foreach ($valueToCheck in $arrayToCheck) {
        if ($valueToCheck[$positionToCheck] -eq "0") {
            $count0++
        }
        else {
            $count1++
        }
    }
    if ($count0 -eq $count1) {
        if ($keepValue -eq "High") { 1 } else { 0 }
    }
    else {
        if ($keepValue -eq "High") {
            if ($count0 -gt $count1) { 0 } else { 1 } # Get Highest
        }
        else {
            if ($count0 -lt $count1) { 0 } else { 1 } # Get Lowest
        }
    }
}
function Get-ReducedArrayForValueAtPosition() {
    param(
        [string[]] $arrayToCheck,
        [int]$positionToCheck,
        [ValidateSet("High", "Low")]
        $keepValue
    )
    if ($arrayToCheck.Length -eq 1) {
        $arrayToCheck
    }
    else {
        [string[]] $newValues = @()
        $mostCommonNumber = Get-CommonNumberAtPosition -arrayToCheck $arrayToCheck -positionToCheck $positionToCheck -keepValue $keepValue
        [string[]] $newValues = $arrayToCheck | Where-Object { $_[$positionToCheck] -eq $mostCommonNumber.ToString() }
        $newValues
    }
}
function Get-DiagnosticValue() {
    param(
        [string[]] $arrayToCheck,
        [ValidateSet("Oxygen", "CO2")]
        $valueType
    )
    if ($valueType -eq "Oxygen") { $keepValue = "High" } else { $keepValue = "Low" }
    $tempArray = [string[]]::new($arrayToCheck.Length)
    [array]::copy($arrayToCheck, $tempArray, $arrayToCheck.Length)
    for ($i = 0; $i -lt $tempArray[0].Length; $i++) {
        $tempArray = Get-ReducedArrayForValueAtPosition -arrayToCheck $tempArray -positionToCheck $i -keepValue $keepValue
    }
    $tempArray    
}

$sampleInput = $false
if ($sampleInput -eq $true) {
    [string[]] $inputValues = Get-Content (Join-Path $PSScriptRoot "SampleInput.txt")
}
else {
    [string[]] $inputValues = Get-Content (Join-Path $PSScriptRoot "Input.txt")
}
# Part 1
$occurences = [object[]]::new($inputValues[0].Length)
for ($i = 0; $i -lt $inputValues.Length; $i++) {
    [string]$inputValue = $inputValues[$i]    
    for ($i2 = 0; $i2 -lt $inputValue.Length; $i2++) {        
        $occurences2 = $occurences[$i2]
        if (-not($occurences2)) {
            $occurences2 = [int[]]::new(2)
        }
        [string]$number = $inputValue[$i2] # Explicitly assign to string first, because otherwise PS will do some kind of conversion
        $occurences2[$number] += 1
        $occurences[$i2] = $occurences2
    }
}
[string]$gamma = ""
[string]$epsilon = ""
for ($i = 0; $i -lt $occurences.Length; $i++) {
    if ($occurences[$i][0] -gt $occurences[$i][1]) {
        $gamma += "0"
        $epsilon += "1"
    }
    else {
        $gamma += "1"
        $epsilon += "0"
    }
}
"  Gamma: $gamma (Decimal: $([convert]::ToInt32($gamma, 2)))"
"Epsilon: $epsilon (Decimal: $([convert]::ToInt32($epsilon, 2)))"
" Result: $([convert]::ToInt32($gamma, 2) * [convert]::ToInt32($epsilon, 2))"

# Wrong answer:
#  
# Correct Answer: 2743844

# Part 2
$oxygenValue = Get-DiagnosticValue -arrayToCheck $inputValues -valueType 'Oxygen'
$co2Value = Get-DiagnosticValue -arrayToCheck $inputValues -valueType 'CO2'
"Oxygen: $oxygenValue (Decimal: $([convert]::ToInt32($oxygenValue, 2)))"
"   CO2: $co2Value (Decimal: $([convert]::ToInt32($co2Value, 2)))"
" Result: $([convert]::ToInt32($oxygenValue, 2) * [convert]::ToInt32($co2Value, 2))"

# Wrong answer:
#  
# Correct Answer: 6677951