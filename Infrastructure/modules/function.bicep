import {tags as tagsType} from '../types/tags.bicep'

param uniqueName string = uniqueString(resourceGroup().id)
param location string = resourceGroup().location
param tags tagsType

param functionAppName string = 'func${uniqueName}'
param serverFarmResourceId string

module site 'br/public:avm/res/web/site:0.15.1' = {
  name: 'functionDeployment'
  params: {
    kind: 'functionapp,linux'
    name: functionAppName
    tags: tags
    serverFarmResourceId: serverFarmResourceId
    location: location
    appSettingsKeyValuePairs: {
      FUNCTIONS_EXTENSION_VERSION: '~4'
      FUNCTIONS_WORKER_RUNTIME: 'dotnet-isolated'
    }
    httpsOnly: true
    siteConfig: {
      netFrameworkVersion: 'v9.0'
      use32BitWorkerProcess: false
    }
  }
}

output functionAppUrl string = 'https://${functionAppName}.azurewebsites.net'
