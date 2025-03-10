param subscriptionId string
param resourceGroupName string
param location string

param storageAccountName string = 'st${uniqueString(resourceGroup().id)}'
param functionAppName string = 'func${uniqueString(resourceGroup().id)}'
param appServicePlanName string = 'plan${uniqueString(resourceGroup().id)}'

resource storageAccount 'Microsoft.Storage/storageAccounts@2022-09-01' = {
  name: storageAccountName
  location: location
  kind: 'StorageV2'
  sku: {
    name: 'Standard_LRS'
  }
  properties: {
    allowBlobPublicAccess: true
    staticWebsite: {
      enabled: true
      indexDocument: 'index.html'
      errorDocument404Path: '404.html'
    }
  }
}

resource functionStorage 'Microsoft.Storage/storageAccounts@2022-09-01' = {
  name: '${storageAccountName}func'
  location: location
  kind: 'StorageV2'
  sku: {
    name: 'Standard_LRS'
  }
}

resource appServicePlan 'Microsoft.Web/serverfarms@2022-09-01' = {
  name: appServicePlanName
  location: location
  kind: 'functionapp'
  sku: {
    tier: 'Dynamic'
    name: 'Y1'
  }
}

resource functionApp 'Microsoft.Web/sites@2022-09-01' = {
  name: functionAppName
  location: location
  kind: 'functionapp'
  properties: {
    serverFarmId: appServicePlan.id
    siteConfig: {
      linuxFxVersion: 'DOTNET-ISOLATED|8.0'
      appSettings: [
        {
          name: 'AzureWebJobsStorage'
          value: functionStorage.properties.primaryEndpoints.blob
        }
        {
          name: 'FUNCTIONS_EXTENSION_VERSION'
          value: '~4'
        }
        {
          name: 'FUNCTIONS_WORKER_RUNTIME'
          value: 'dotnet-isolated'
        }
      ]
    }
    storageAccount: {
      id: functionStorage.id
    }
  }
}

output staticWebsiteUrl string = storageAccount.properties.primaryEndpoints.web
output functionAppUrl string = 'https://${functionAppName}.azurewebsites.net'
