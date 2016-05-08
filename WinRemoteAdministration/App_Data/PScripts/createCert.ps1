#Cert Genearation Related Functions 

#************************************************************************************** 

#Create Cert, install Cert to My, install Cert to Root, Export Cert as pfx and bind it


Param ( 
    $certcn,
    $password,
    $appId
) 

#Check if the certificate name was used before 
$thumbprintA=(dir cert:\localmachine\My -recurse | where {$_.Subject -match "CN=" + $certcn} | Select-Object -Last 1).thumbprint 

if ($thumbprintA.Length -gt 0) { 
    Write-Host "Duplicated Cert Name used" -ForegroundColor Cyan 
    return 
} 

else { 
    $thumbprintA=New-SelfSignedCertificate -DnsName $certcn -CertStoreLocation cert:\LocalMachine\My | ForEach-Object{ $_.Thumbprint} 
} 

#If generated successfully 
if ($thumbprintA.Length -gt 0) { 

#query the new installed cerificate again 
$thumbprintB=(dir cert:\localmachine\My -recurse | where {$_.Subject -match "CN=" + $certcn} | Select-Object -Last 1).thumbprint 

#If new cert installed sucessfully with the same thumbprint 
    if($thumbprintA -eq $thumbprintB ) { 

        $certfilepath = ".\cert.pfx"
        $message = $certcn + " installed into LocalMachine\My successfully with thumprint "+$thumbprintA 
        Write-Host $message -ForegroundColor Cyan 

        $mypwd = $password | ConvertTo-SecureString -AsPlainText -Force
        Write-Host "Exporting Certificate as .pfx file" -ForegroundColor Cyan 

        Export-PfxCertificate -FilePath $certfilepath -Cert cert:\localmachine\My\$thumbprintA -Password $mypwd 
        Write-Host "Importing Certificate to LocalMachine\Root" -ForegroundColor Cyan 

        Import-PfxCertificate -FilePath $certfilepath -Password $mypwd -CertStoreLocation cert:\LocalMachine\Root 
    } 

    else { 
        Write-Host "Thumbprint is not the same between new cert and installed cert." -ForegroundColor Cyan 
    } 
} 

else { 
    $message = $certcn + " is not created" 
    Write-Host $message -ForegroundColor Cyan 
} 

