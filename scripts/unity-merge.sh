#!/bin/sh
if [ $# -eq 0 ]; then
    git lfs install --local && exec git config merge.unityyamlmerge.driver 'sh scripts/unity-merge.sh %O %B %A'
fi
v=$(sed -n 's/^m_EditorVersion: //p' ProjectSettings/ProjectVersion.txt)
exec "/Applications/Unity/Hub/Editor/$v/Unity.app/Contents/Helpers/UnityYAMLMerge" merge -p -h --force --fallback none "$1" "$2" "$3" "$3"
