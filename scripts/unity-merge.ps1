if ($args.Count -eq 0) {
    git lfs install --local
    if ($LASTEXITCODE -eq 0) { git config merge.unityyamlmerge.driver 'powershell -NoProfile -ExecutionPolicy Bypass -File scripts/unity-merge.ps1 %O %B %A' }
    exit $LASTEXITCODE
}
$v = (Get-Content ProjectSettings/ProjectVersion.txt) -match '^m_EditorVersion: ' -replace '^m_EditorVersion: '
& "$env:ProgramFiles/Unity/Hub/Editor/$v/Editor/Data/Tools/UnityYAMLMerge.exe" merge -p -h --force --fallback none $args[0] $args[1] $args[2] $args[2]
exit $LASTEXITCODE
