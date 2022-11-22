# How to zip dotnet projects

Visual Studio project folders contains a lot of files that it generates each time you compile your project, so you don't need them when zipping the folder, what about if we could use [.gitignore](https://help.github.com/articles/ignoring-files/)?

There are ways to do this :) lets check them down

## Using 7Zip

Here is the link to [7zip](http://www.7-zip.org/) if not installed

**Steps:**

- Open a command prompt
- Copy this into the console:
  
  ```powershell
  7z.exe a -t7z "C:\Destination\Path\%DATE:~-4%-%DATE:~4,2%-%DATE:~7,2% BackupName.7z" "C:\Source\Path\FolderToBeBackup" -bd  -mx9 -xr@"C:\Path\To\ExcludeListName"
  ```
  
- Press Enter

The exclude list could be a copy of a `.gitignore` file

## Git archive functionality

If the projects are git repos, the `git archive` [cli command](https://git-scm.com/docs/git-archive/2.34.0) can be used

### Unix

`git archive HEAD -o ${PWD##*/}.zip`

### PowerShell

`git archive HEAD -o ../$(Split-Path -Path ${PWD} -Leaf)-$(Get-Date -UFormat "%Y.%m.%d-%H.%M").zip`
