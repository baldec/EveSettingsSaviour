# ESS - EveSettingsSaviour
C# application that assist with managing EVE user account and character settings.
- Copy account settings from one user account to one or more other user account(s).
- Copy character settings from one character to one or more other character(s).
- Create a backup of a settings folder.
- Load user settings from backup.

Application is still in very early alpha state so Ux needs a bit of work to be more intuitive.

## System Requirements
Built using dotnet 5 and requires the desktop runtime(.NET Desktop Runtime), which is free to download from Microsoft.
https://dotnet.microsoft.com/download/dotnet/5.0

Currently the desktop runtime is only available for computers using windows, though this might change with time. 

## Usage

### Copy user settings
1. Click *Scan Settings*
2. Select target on the right side
3. Click *Copy Settings*

### Backup user settings
1. Click *From* and select folder
2. Click *To* and select destination folder
3. Click *Create Backup of Folder*

### Load user settings from backup
1. Click *Scan Settings*
2. Click *From*
3. Click *Load From*
4. Select target on the right side
5. Click *Copy Settings* 
