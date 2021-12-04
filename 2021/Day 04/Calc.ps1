function Get-BlankBingoBoard() {
    $bingoValue = [PSCustomObject]@{Number = 0; Marked = $false }
    $bingoBoard = [PSCustomObject]@{
        A1      = $bingoValue
        A2      = $bingoValue
        A3      = $bingoValue
        A4      = $bingoValue
        A5      = $bingoValue
        B1      = $bingoValue
        B2      = $bingoValue
        B3      = $bingoValue
        B4      = $bingoValue
        B5      = $bingoValue
        C1      = $bingoValue
        C2      = $bingoValue
        C3      = $bingoValue
        C4      = $bingoValue
        C5      = $bingoValue
        D1      = $bingoValue
        D2      = $bingoValue
        D3      = $bingoValue
        D4      = $bingoValue
        D5      = $bingoValue
        E1      = $bingoValue
        E2      = $bingoValue
        E3      = $bingoValue
        E4      = $bingoValue
        E5      = $bingoValue
        Numbers = $null
    }
    $bingoBoard
}
function Get-ValueIdentifier() {
    param(        
        [int] $LineCounter,
        [int] $i
    )
    switch ($LineCounter) {
        1 { $Identifier = "A$($i)" }
        2 { $Identifier = "B$($i)" }
        3 { $Identifier = "C$($i)" }
        4 { $Identifier = "D$($i)" }
        5 { $Identifier = "E$($i)" }
        Default { throw "Unhandled value" }
    }
    $Identifier
}
function Add-RowToBingoBoard() {
    param(
        [PSCustomObject] $BingoBoard,
        [string] $LineValue,
        [int] $LineCounter
    )
    $lineValues = $LineValue.Split(' ');
    $lineValues = $lineValues.Split('', [System.StringSplitOptions]::RemoveEmptyEntries)
    for ($i = 0; $i -lt $lineValues.Length; $i++) {
        $Identifier = Get-ValueIdentifier -LineCounter $LineCounter -i ($i + 1)
        $BingoBoard.$Identifier = [PSCustomObject]@{Number = $lineValues[$i]; Marked = $false }
        $BingoBoard.Numbers += , $lineValues[$i]
    }
    $BingoBoard
}
function Get-BingoBoardsFromInput() {
    param(
        [string[]] $InputSource
    )
    $bingoBoards = @() # Array to hold the objects
    $bingoBoard = $null;
    $lineCounter = 0
    foreach ($line in $InputSource) {
        if (-not([string]::IsNullOrEmpty($line))) {
            if ($line.Length -lt 15) {
                $lineCounter++
                $bingoBoard = Add-RowToBingoBoard -BingoBoard $bingoBoard -LineValue $line -LineCounter $lineCounter
            }
        }
        else {
            if ($null -ne $bingoBoard) {
                $bingoBoards += $bingoBoard
                $bingoBoard = $null
            }
            $bingoBoard = Get-BlankBingoBoard
            $lineCounter = 0
        }
    }
    $bingoBoards
}
function Update-BingoBoard() {
    param(
        [PSCustomObject] $BingoBoard,
        [int] $BingoValue
    )
    foreach ($property in $BingoBoard.PSObject.Properties | Where-Object { $_.Value.Number -eq $BingoValue }) {
        $identifier = $property.Name
        $BingoBoard.$identifier.Marked = $true
    }
    $BingoBoard
}
function Test-RowForBingo() {
    param(
        [PSCustomObject] $BingoBoard,
        [int] $Row
    )
    for ($i = 0; $i -lt 5; $i++) {
        $identifier = Get-ValueIdentifier -LineCounter $Row -i ($i + 1)
        if ($BingoBoard.$identifier.Marked -eq $false) {
            $false
            return
        }
    }
    $true
}
function Test-ForBingo() {
    param(
        [PSCustomObject] $BingoBoard
    )
    # Check number of already marked values
    $markedValuesCount = ($BingoBoard.PSObject.Properties | Where-Object { $_.Value.Marked -eq $true }).Count
    if ($markedValuesCount -lt 5) {
        # No additional check needed, if we don't even have 5 marked values
        $false
        return
    }
    for ($i = 0; $i -lt 5; $i++) {
        if (Test-RowForBingo -BingoBoard $BingoBoard -Row ($i + 1)) {
            $true
            return
        }
    }
}
function Get-SumOfUnmarkedFields() {
    param(
        [PSCustomObject] $BingoBoard
    )
    $tempSum = 0
    foreach ($property in $BingoBoard.PSObject.Properties | Where-Object { $_.Value.Marked -eq $false }) {
        $tempSum += $property.Value.Number
    }
    $tempSum
}

$sampleInput = $false
if ($sampleInput -eq $true) {
    [string[]] $inputValues = Get-Content (Join-Path $PSScriptRoot "SampleInput.txt")
}
else {
    [string[]] $inputValues = Get-Content (Join-Path $PSScriptRoot "Input.txt")
}
# Part 1

$bingoBoards = Get-BingoBoardsFromInput -InputSource $inputValues
[int[]]$bingoNumbers = $inputValues[0].Split(',')

$bingo = $false
$boardWithBingo = $null
$bingoNumber = 0
for ($i = 0; $i -lt $bingoNumbers.Length; $i++) {
    foreach ($board in $bingoBoards | Where-Object { $_.Numbers -contains $bingoNumbers[$i] }) {
        $board = Update-BingoBoard -BingoBoard $board -BingoValue $bingoNumbers[$i]
        if ($i -gt 4) {
            if (Test-ForBingo -BingoBoard $board) {
                $bingo = $true
                $boardWithBingo = $board
                $bingoNumber = $bingoNumbers[$i]                
                break;
            }
        }
    }
    if ($bingo -eq $true) {
        break
    }
}
$UnmarkedSum = Get-SumOfUnmarkedFields -BingoBoard $boardWithBingo
Write-Host "Bingo! ($($bingoNumber))"
Write-Host "Sum (unmarked): $UnmarkedSum"
Write-Host "Result        : $($UnmarkedSum * $bingoNumber)"
# Wrong answer:
#  
# Correct Answer: 10680

# ====================================================================

# Part 2


# Wrong answer:
#  
# Correct Answer: 