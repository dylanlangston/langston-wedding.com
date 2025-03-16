param uniqueName string = uniqueString(resourceGroup().id)
param location string = resourceGroup().location

param storageAccountName string = 'st${uniqueName}'
param functionAppName string = 'func${uniqueName}'
param appServicePlanName string = 'plan${uniqueName}'

module staticWebsite './modules/static-website.bicep' = {
  name: 'staticWebsite'
  params: {
    uniqueName: uniqueName
    storageAccountName: storageAccountName
    location: location
  }
}

module appServicePlan './modules/app-service.bicep' = {
  name: 'appServicePlan'
  params: {
    uniqueName: uniqueName
    appServicePlanName: appServicePlanName
    location: location
  }
}

module function './modules/function.bicep' = {
  name: 'function'
  params: {
    uniqueName: uniqueName
    functionAppName: functionAppName
    serverFarmResourceId: appServicePlan.outputs.serverFarmResourceId
    location: location
  }
}

output staticWebsiteUrl string = staticWebsite.outputs.staticWebsiteUrl
output functionAppUrl string = function.outputs.functionAppUrl
output resourceSuffix string = uniqueName
