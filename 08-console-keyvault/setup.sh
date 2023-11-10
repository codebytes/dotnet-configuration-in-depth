#!/bin/sh

location=eastus
rg=rg-dotnet
kvName=cayersdotnetkv

az group create --name ${rg} --location ${location}

az keyvault create --location ${location} --name ${kvName} --resource-group ${rg}

az keyvault secret set --name Message --vault-name ${kvName} --value "Hello from KV"
