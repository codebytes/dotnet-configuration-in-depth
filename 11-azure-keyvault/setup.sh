#!/bin/sh

location=eastus
rg=rg-dotnet
kvName=cayersdotnetkv

az group create --name ${rg} --location ${location}

az keyvault create --location ${location} --name ${kvName} --resource-group ${rg}


# Assign the Key Vault Administrator role to the current user
userId=$(az ad signed-in-user show --query id --output tsv)
kvResourceId=$(az keyvault show --name ${kvName} --resource-group ${rg} --query id --output tsv)
az role assignment create --assignee ${userId} --role "Key Vault Administrator" --scope ${kvResourceId}

az keyvault secret set --name Message --vault-name ${kvName} --value "Hello from KV"
