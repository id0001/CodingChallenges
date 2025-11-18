param(
    [int]$Story,
    [int]$Amount,
    [string] $Answers
)

$projectDir = "$PWD\\Events\\$Story"
$challengeDir = "$projectDir\\Challenges"
$templateFile = "$PWD\\Templates\\template.txt"
$answerJson = $null

if(Test-Path -Path $Answers) {
    $answerJson = Get-Content -Path $Answers -Raw | ConvertFrom-Json 
}

if( -not (Test-Path -Path $projectDir)) {
    Write-Error "Directory for event $Story not found."
    exit
}

if( -not (Test-Path -Path $challengeDir)) {
    New-Item -Path $challengeDir -ItemType Directory
}

for($quest = 1; $quest -le $Amount; $quest++){
    $paddedQuest = "{0:d2}" -f $quest
    $questFile = "$challengeDir\\Quest$paddedQuest.cs"

    if( -not (Test-Path -Path $questFile)) {
        $template = Get-Content -Path $templateFile
        $template = $template.Replace("{{story}}", $Story)
        $template = $template.Replace("{{paddedQuest}}", $paddedQuest)
        $template = $template.Replace("{{quest}}", $quest)

        if($null -eq $answerJson) {
            $template = $template.Replace("{{expected_1}}", "")
            $template = $template.Replace("{{expected_2}}", "")
            $template = $template.Replace("{{expected_3}}", "")
        } else {
            $p1 = $answerJson.$challenge."1" 
            $template = $template.Replace("{{expected_1}}", ", ""$p1"")")

            $p2 = $answerJson.$challenge."2"
            $template = $null -ne $p2 ? $template.Replace("{{expected_2}}", ", ""$p2"")") : $template.Replace("{{expected_2}}", "")

            $p3 = $answerJson.$challenge."3"
            $template = $null -ne $p3 ? $template.Replace("{{expected_3}}", ", ""$p3"")") : $template.Replace("{{expected_3}}", "")
        }

        Set-Content -Path $questFile -Value $template
    }
}