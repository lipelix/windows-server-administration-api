#Search Active Directory for at least 30 days inactive computers

Search-ADAccount -AccountInactive -ComputersOnly -TimeSpan "30" | Sort-Object SamAccountName | Select-Object -property Name, LastLogonDate