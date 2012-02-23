function Get-ScriptDirectory {   
	if (Test-Path variable:\hostinvocation) 
        {$FullPath=$hostinvocation.MyCommand.Path}
    else {
        $FullPath=(get-variable myinvocation -scope script).value.Mycommand.Definition }    
    if (Test-Path $FullPath) {
        return (Split-Path $FullPath) 
        }
    else {
        $FullPath=(Get-Location).path
        Write-Warning ("Get-ScriptDirectory: Powershell Host <" + $Host.name + "> may not be compatible with this function, the current directory <" + $FullPath + "> will be used.")
        return $FullPath
        }
}

function AddTo-HostsFile{
	param(
		[parameter(Mandatory=$true,position=0)]
		[string]
		$IPAddress,
		[parameter(Mandatory=$true,position=1)]
		[string]
		$HostName
	)

	$HostsLocation = "$env:windir\System32\drivers\etc\hosts";
	$NewHostEntry = "$IPAddress`t$HostName";

	if((gc $HostsLocation) -contains $NewHostEntry)
	{
        Write-Host "The hosts file already contains the entry: $NewHostEntry.  File not updated.";
	}
	else
	{
        Write-Host "The hosts file does not contain the entry: $NewHostEntry.  Attempting to update.";
        Add-Content -Path $HostsLocation -Value $NewHostEntry;
        
        if((gc $HostsLocation) -contains $NewHostEntry)
        {
            Write-Host "New entry, $NewHostEntry, added to $HostsLocation.";
        }
        else
        {
            Write-Host "The new entry, $NewHostEntry, was not added to $HostsLocation.";
        }
	}	
}

function Create-WebSite() {
	param(		
		[string]
		$relativePath,		
		[string]
		$siteName
	)
	
	$fullPhysicalPath = Join-Path (Get-ScriptDirectory) $relativePath
	$fullSitePath = "iis:\sites\$siteName"
	if ((Test-Path -path $fullSitePath) -ne $True)
	{
		Write-Host "Creating $siteName site..."
		New-Item $fullSitePath -bindings @{protocol="http";bindingInformation="*:80:$siteName"} -physicalPath $fullPhysicalPath
	} else {
		Write-Host "$siteName already exists, skipping..."
    }	
	
	Addto-HostsFile "127.0.0.1" $siteName	
}

if ((Test-Path -path iis:) -ne $True)
{
	throw "Must have IIS snap-in enabled. Use ImportSystemModules to load."
} 

Create-WebSite "Api" "api.dotnetgroup.dev"

Create-WebSite "Web" "dotnetgroup.dev"

Create-WebSite "Web.Mobile" "m.dotnetgroup.dev"