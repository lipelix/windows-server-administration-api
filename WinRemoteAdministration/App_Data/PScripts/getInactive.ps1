﻿#Search Active Directory for at least 30 days inactive users and computers

Search-ADAccount -AccountInactive -TimeSpan "30" | Sort-Object SamAccountName | Select-Object -property SamAccountName, LastLogonDate, ObjectClass