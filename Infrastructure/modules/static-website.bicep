param uniqueName string = uniqueString(resourceGroup().id)
param location string = resourceGroup().location

param storageAccountName string = 'st${uniqueName}'

resource storageAccount 'Microsoft.Storage/storageAccounts@2022-09-01' = {
  name: storageAccountName
  location: location
  kind: 'StorageV2'
  sku: {
    name: 'Standard_LRS'
  }
  properties: {
    supportsHttpsTrafficOnly: true
  }
}

output staticWebsiteUrl string = storageAccount.properties.primaryEndpoints.web
