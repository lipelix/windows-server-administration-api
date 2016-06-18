#Stop running process according its id
#Input params: Id - Proces id

param (
    [int]$Id
)

Stop-Process -Id $Id -Force