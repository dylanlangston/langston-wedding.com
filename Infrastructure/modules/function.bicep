import { tags as tagsType } from '../types/tags.bicep'

param uniqueName string = uniqueString(resourceGroup().id)
param location string = resourceGroup().location
param tags tagsType

param functionAppName string = 'func${uniqueName}'
param functionStorageAccountName string = 'funcst${uniqueName}'
param serverFarmResourceId string

resource functionStorage 'Microsoft.Storage/storageAccounts@2022-09-01' = {
  name: functionStorageAccountName
  location: location
  kind: 'StorageV2'
  sku: {
    name: 'Standard_LRS'
  }
  properties: {
    supportsHttpsTrafficOnly: true
    minimumTlsVersion: 'TLS1_2'
    defaultToOAuthAuthentication: true
    allowBlobPublicAccess: false
    publicNetworkAccess: 'Enabled'
  }
}

resource functionApp 'Microsoft.Web/sites@2022-09-01' = {
  name: functionAppName
  location: location
  kind: 'functionapp'
  tags: tags
  properties: {
    serverFarmId: serverFarmResourceId
    siteConfig: {
      appSettings: [
        {
          name: 'AzureWebJobsStorage'
          value: 'DefaultEndpointsProtocol=https;AccountName=${functionStorageAccountName};AccountKey=${listKeys(functionStorage.id, '2022-05-01').keys[0].value};EndpointSuffix=core.windows.net'
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
      cors: {
        allowedOrigins: [
          'https://portal.azure.com'
        ]
      }
      use32BitWorkerProcess: false
      linuxFxVersion: 'DOTNET-ISOLATED|9.0'
    }
    httpsOnly: true
    publicNetworkAccess: 'Enabled'
  }
}

output functionAppUrl string = 'https://${functionAppName}.azurewebsites.net'
