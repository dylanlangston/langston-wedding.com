param uniqueName string = uniqueString(resourceGroup().id)
param location string = resourceGroup().location

param storageAccountName string = 'st${uniqueName}'
param functionAppName string = 'func${uniqueName}'

module staticWebsite './modules/static-website.bicep' = {
  name: 'staticWebsite'
  params: {
    uniqueName: uniqueName
    storageAccountName: storageAccountName
    location: location
  }
}

module function './modules/function.bicep' = {
  name: 'function'
  params: {
    uniqueName: uniqueName
    functionAppName: functionAppName
    location: location
  }
}

output staticWebsiteUrl string = staticWebsite.outputs.staticWebsiteUrl
output functionAppUrl string = function.outputs.functionAppUrl
output resourceSuffix string = uniqueName
