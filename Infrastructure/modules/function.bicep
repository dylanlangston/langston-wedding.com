param uniqueName string = uniqueString(resourceGroup().id)
param location string = resourceGroup().location

param functionAppName string = 'func${uniqueName}'

module site 'br/public:avm/res/web/site:0.15.1' = {
  name: 'siteDeployment'
  params: {
    kind: 'functionapp'
    name: functionAppName
    serverFarmResourceId: '<serverFarmResourceId>'
    location: location
    siteConfig: {
      netFrameworkVersion: 'v9.0'
    }
  }
}

output functionAppUrl string = 'https://${functionAppName}.azurewebsites.net'
