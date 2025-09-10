#!/bin/sh

location=eastus
rg=rg-dotnet
acName=cayers-dotnet-ac

az group create --name ${rg} --location ${location}

az appconfig create --location ${location} --name ${acName} --resource-group ${rg}

az appconfig kv set --name ${acName} --key TestApp:Settings:BackgroundColor --value White -y
az appconfig kv set --name ${acName} --key TestApp:Settings:FontColor --value Black -y
az appconfig kv set --name ${acName} --key TestApp:Settings:FontSize --value 40 -y
az appconfig kv set --name ${acName} --key TestApp:Settings:Message --value "Data from Azure App Configuration" -y
az appconfig kv set --name ${acName} --key TestApp:Settings:Sentinel --value 1 -y

dotnet user-secrets set ConnectionStrings:AppConfig $(az appconfig credential list -g $rg   -n $acName --query "([?name=='Primary Read Only'].connectionString)[0]" --output tsv)