$Version = $args[0]

$Update = $args[1]
if ( ($Update -eq "Y") -or ($Update -eq "y") )
{
    # No need to copy from Debug anymore - data is copied during build
    # Just clean up any unwanted files from bin\data if they exist
    Remove-Item -Recurse -Force "bin\data\RandoPaths.csv" -ErrorAction Ignore

    Write-Host "Updating versions in bin\data..."
    (Get-Content "bin\data\modpack\modconfig.ini") `
        -replace '_VERSION_', "$Version" |
    Out-File "bin\data\modpack\modconfig.ini"
    (Get-Content "bin\data\modpack\readme.txt") `
        -replace '_VERSION_', "$Version" |
    Out-File "bin\data\modpack\readme.txt"
}

$Update = $args[2]
if ( ($Update -eq "Y") -or ($Update -eq "y") )
{
    dotnet publish -r win-x64 -c Release --no-self-contained /p:CopyLocalLockFileAssemblies=true --output "bin\publish"
    
    Write-Host "Organizing DLLs into libs folder..."
    New-Item -ItemType Directory -Force -Path "bin\publish\libs"
    Get-ChildItem "bin\publish\*.dll" | Move-Item -Destination "bin\publish\libs"

    Write-Host "Copying data to bin\publish..."

    Remove-Item -Recurse -Force "bin\publish\data" -ErrorAction Ignore
    Copy-Item -Path "bin\data\" -Destination "bin\publish\data" -Recurse -Force

    Write-Host "Creating 7z file..."
    Remove-Item -Recurse -Force "bin\publish\LRRandomizer$Version.7z" -ErrorAction Ignore
    Push-Location -Path "bin\publish"
    & "7z.exe" a -t7z -mx=9 "LRRandomizer$Version.7z" "data" "README.pdf" "LRRando.exe" "libs"
    Pop-Location

    Copy-Item -Path "bin\publish\LRRandomizer$Version.7z" -Destination "bin\build\LRRandomizerPreview.7z" -Force
}