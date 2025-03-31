@description('Comma seperated list of IP Addresses to whitelist')
param whitelistedIPs string
@description('Domain name for the app')
param domain string
@description('Domain name for the app\'s api')
param apidomain string

var rg = resourceGroup()
var uniqueName = uniqueString(rg.id)
var location = rg.location
var tags = {
  'app-name': 'Langston-Wedding.com'
}

module staticWebsite './modules/static-website.bicep' = {
  name: 'staticWebsite'
  params: {
    tags: tags
    uniqueName: uniqueName
    location: location
    whitelistedIPs: whitelistedIPs
  }
}

module cosmosdb './modules/cosmos.bicep' = {
  name: 'cosmosdb'
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
    applicationInsightsResourceId: appServicePlan.outputs.applicationInsightsResourceId
    domain: apidomain
    cosmosDbAccountId: cosmosdb.outputs.cosmosdbAccountId
    cosmosDbEndpoint: cosmosdb.outputs.cosmosdbAccountEndpoint
  }
}

output staticWebsiteUrl string = staticWebsite.outputs.staticWebsiteUrl
output cosmosdbEndpoint string = cosmosdb.outputs.cosmosdbAccountEndpoint
output functionAppUrl string = function.outputs.functionAppUrl
output functionCustomDomainVerificationId string = function.outputs.customDomainVerificationId
output resourceSuffix string = uniqueName
