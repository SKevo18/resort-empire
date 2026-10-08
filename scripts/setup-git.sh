#!/bin/sh
set -eu

usage() {
    cat <<'EOF'
Usage: sh scripts/setup-git.sh [path-to-UnityYAMLMerge]

Install Git LFS and the Unity version in ProjectSettings/ProjectVersion.txt first.
On Windows, run this command in Git Bash (included with Git for Windows).
The default Unity Hub installation is detected on macOS and Windows.
For a custom Unity installation, pass the full merge executable path in quotes.

This installs local LFS hooks and configures Unity Smart Merge in .git/config.
It does not change global Git settings, stage files, or download LFS assets.
Run git lfs pull afterward if your clone contains unhydrated LFS pointers.
Rerun this setup after cloning or changing the Unity editor version/location.

Team conventions:
  Use the pinned Unity version and commit Assets together with their .meta files.
  Keep Force Text serialization and Visible Meta Files enabled in Unity.
  Text checks out as LF on both systems; .bat/.cmd files use CRLF.
  Scenes use Smart Merge; importer .meta files use Git's text merge.
  Resolve any remaining conflicts in Git before opening affected assets in Unity.
  Avoid filenames that differ only by case; use a temporary name for case renames.
EOF
}

case "${1:-}" in
    -h|--help) usage; exit 0 ;;
esac
if [ "$#" -gt 1 ]; then
    usage >&2
    exit 1
fi

cd "$(dirname "$0")/.."
git rev-parse --git-dir >/dev/null
if ! git lfs version >/dev/null 2>&1; then
    printf '%s\n' 'Git LFS is required. Install it from https://git-lfs.com/ and rerun setup.' >&2
    exit 1
fi

version=
while IFS=' ' read -r key value rest; do
    if [ "$key" = 'm_EditorVersion:' ]; then
        version=$(printf '%s' "$value" | tr -d '\r')
        break
    fi
done < ProjectSettings/ProjectVersion.txt
if [ -z "$version" ]; then
    printf '%s\n' 'Could not read the pinned Unity editor version.' >&2
    exit 1
fi

merge_tool=${1:-}
if [ -z "$merge_tool" ]; then
    case "$(uname -s)" in
        Darwin)
            editor="/Applications/Unity/Hub/Editor/$version/Unity.app/Contents"
            for candidate in "$editor/Helpers/UnityYAMLMerge" "$editor/Tools/UnityYAMLMerge"; do
                if [ -x "$candidate" ]; then
                    merge_tool=$candidate
                    break
                fi
            done
            ;;
        MINGW*|MSYS*|CYGWIN*)
            program_files=${ProgramW6432:-${PROGRAMW6432:-${ProgramFiles:-${PROGRAMFILES:-'C:/Program Files'}}}}
            editor="$program_files/Unity/Hub/Editor/$version/Editor/Data"
            for candidate in "$editor/Tools/UnityYAMLMerge.exe" "$editor/Helpers/UnityYAMLMerge.exe"; do
                if [ -x "$candidate" ]; then
                    merge_tool=$candidate
                    break
                fi
            done
            ;;
    esac
fi

# Git Bash accepts Windows paths; normalize them before passing them to its shell.
if [ -n "$merge_tool" ] && command -v cygpath >/dev/null 2>&1; then
    merge_tool=$(cygpath -u "$merge_tool")
fi
if [ -z "$merge_tool" ] || [ ! -x "$merge_tool" ]; then
    printf 'UnityYAMLMerge for Unity %s was not found.\n' "$version" >&2
    printf '%s\n' 'Install that editor or rerun setup with its full UnityYAMLMerge executable path.' >&2
    exit 1
fi
# Resolve relative custom paths so merges also work when invoked from subdirectories.
merge_tool="$(cd "$(dirname "$merge_tool")" && pwd)/$(basename "$merge_tool")"

git lfs install --local
git config --local core.autocrlf false
git config --local merge.unityyamlmerge.name 'Unity Smart Merge'
git config --local merge.unityyamlmerge.path "$merge_tool"
# Unity expects base, theirs, ours, output. Read the executable path as data so
# spaces, apostrophes and shell metacharacters in installation paths remain safe.
# Git's temporary inputs have no Unity extension. Force YAML parsing and leave
# unresolved conflicts to Git instead of launching machine-specific GUI tools.
git config --local merge.unityyamlmerge.driver '"$(git config --path --get merge.unityyamlmerge.path)" merge -p -h --force --fallback none "%O" "%B" "%A" "%A"'
git config --local merge.unityyamlmerge.recursive binary

printf 'Configured this clone for Unity %s.\nSmart Merge: %s\n' "$version" "$merge_tool"
printf '%s\n' 'Shared .gitattributes rules control line endings; global Git settings are unchanged.'
