<#
Simple PowerShell script to run SQL scripts for the Videoteka project against a LocalDB instance.
Usage:
  Open Developer PowerShell and run:
	.\db\run_sql_scripts.ps1
  Or specify instance:
	.\db\run_sql_scripts.ps1 -Instance "(localdb)\MSSQLLocalDB"

The script will try to use sqlcmd if available, otherwise Invoke-Sqlcmd from the SqlServer module.
#>

param(
	[string]$Instance = "(localdb)\MSSQLLocalDB",
	[string[]]$ScriptFiles = @('create_tables.sql', 'seed_genres.sql', 'seed_sample_data.sql')
)

Set-StrictMode -Version Latest

$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Definition

function Run-SqlFile($filePath) {
	Write-Host "Running: $filePath" -ForegroundColor Cyan

	# Prefer sqlcmd if available
	$sqlcmd = Get-Command sqlcmd -ErrorAction SilentlyContinue
	if ($sqlcmd) {
		& sqlcmd -S $Instance -i $filePath
		if ($LASTEXITCODE -ne 0) {
			throw "sqlcmd exited with code $LASTEXITCODE"
		}
		return
	}

	# Fallback to Invoke-Sqlcmd if SqlServer module is available
	$mod = Get-Module -ListAvailable -Name SqlServer
	if ($mod) {
		Import-Module SqlServer -ErrorAction Stop
		Invoke-Sqlcmd -ServerInstance $Instance -InputFile $filePath -ErrorAction Stop
		return
	}

	throw "Neither 'sqlcmd' nor the 'SqlServer' PowerShell module is available. Install SQL Server tools or the SqlServer module."
}

try {
	foreach ($f in $ScriptFiles) {
		$full = Join-Path $scriptDir $f
		if (-not (Test-Path $full)) {
			throw "Script not found: $full"
		}
		Run-SqlFile $full
		Write-Host "Finished: $f" -ForegroundColor Green
	}

	Write-Host "All scripts executed successfully." -ForegroundColor Green
	exit 0
}
catch {
	Write-Host "ERROR: $_" -ForegroundColor Red
	Write-Host "Stopped. Fix the error and re-run the script." -ForegroundColor Yellow
	exit 1
}
