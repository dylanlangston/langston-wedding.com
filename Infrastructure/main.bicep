var rg = resourceGroup()
var uniqueName = uniqueString(rg.id)
var location =  rg.location
var tags = {
  'app-name': 'Langston-Wedding.com'
}

module staticWebsite './modules/static-website.bicep' = {
  name: 'staticWebsite'
  params: {
    tags: tags
    uniqueName: uniqueName
    location: location
  }
}

module appServicePlan './modules/app-service.bicep' = {
  name: 'appServicePlan'
  params: {
    tags: tags
    uniqueName: uniqueName
    location: location
  }
}

module function './modules/function.bicep' = {
  name: 'function'
  params: {
    tags: tags
    uniqueName: uniqueName
    location: location
    serverFarmResourceId: appServicePlan.outputs.serverFarmResourceId
  }
}

output staticWebsiteUrl string = staticWebsite.outputs.staticWebsiteUrl
output functionAppUrl string = function.outputs.functionAppUrl
output resourceSuffix string = uniqueName
