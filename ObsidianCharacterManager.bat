cd S:\Personale\ObsidianCharacterManager\ObsidianCharacterManager

dotnet publish ObsidianCharacterManager -c Release -r win-x86 --self-contained true -p:PublishSingleFile=true -p:DebugSymbols=false

if exist ObsidianCharacterManagerPublish (
    del /q ObsidianCharacterManagerPublish\*
    for /D %%p in (ObsidianCharacterManagerPublish\*) do rmdir "%%p" /s /q
    rmdir ObsidianCharacterManagerPublish
)

mkdir ObsidianCharacterManagerPublish
robocopy S:\Personale\ObsidianCharacterManager\ObsidianCharacterManager\ObsidianCharacterManager\bin\Release\net8.0\publish\win-x86 ObsidianCharacterManagerPublish /e /move

mkdir ObsidianCharacterManagerPublish\Symbols
move ObsidianCharacterManagerPublish\*.pdb ObsidianCharacterManagerPublish\Symbols

copy S:\Personale\ObsidianCharacterManager\ObsidianCharacterManager\ObsidianCharacterManager\appsettings.json ObsidianCharacterManagerPublish

"C:\Program Files\7-Zip\7z.exe" a ObsidianCharacterManagerPublish.7z ObsidianCharacterManagerPublish

del /q ObsidianCharacterManagerPublish\*
for /D %%p in (ObsidianCharacterManagerPublish\*) do rmdir "%%p" /s /q
rmdir ObsidianCharacterManagerPublish

pause