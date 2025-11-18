param(
    [int]$Year,
    [int]$Amount,
    [string] $Answers
)

$projectDir = "$PWD\\Years\\$Year"
$challengeDir = "$projectDir\\Challenges"
$templateFile = "$PWD\\Templates\\template.txt"
$answerJson = $null

if(Test-Path -Path $Answers) {
    $answerJson = Get-Content -Path $Answers -Raw | ConvertFrom-Json 
}

if( -not (Test-Path -Path $projectDir)) {
    Write-Error "Directory for year $Year not found."
    exit
}

if( -not (Test-Path -Path $challengeDir)) {
    New-Item -Path $challengeDir -ItemType Directory
}

for($day = 1; $day -le $Amount; $day++){
    $zeroDay = "{0:d2}" -f $day
    $dayFile = "$challengeDir\\Challenge$zeroDay.cs"

    if( -not (Test-Path -Path $dayFile)) {
        $template = Get-Content -Path $templateFile
        $template = $template.Replace("{{year}}", $Year)
        $template = $template.Replace("{{zeroday}}", $zeroDay)
        $template = $template.Replace("{{day}}", $day)

        if($null -eq $answerJson) {
            $template = $template.Replace("{{expected_1}}", "")
            $template = $template.Replace("{{expected_2}}", "")
        } else {
            $p1 = $answerJson.$day."1" 
            $template = $template.Replace("{{expected_1}}", ", ""$p1"")")

            $p2 = $answerJson.$day."2"
            $template = $null -ne $p2 ? $template.Replace("{{expected_2}}", ", ""$p2"")") : $template.Replace("{{expected_2}}", "")
        }


        Set-Content -Path $dayFile -Value $template
    }
}