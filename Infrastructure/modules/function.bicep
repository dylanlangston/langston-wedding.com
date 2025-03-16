param uniqueName string = uniqueString(resourceGroup().id)
param location string = resourceGroup().location

param functionStorageAccountName string = 'funcst${uniqueName}'
param functionAppName string = 'func${uniqueName}'
param appServicePlanName string = 'plan${uniqueName}'

resource appServicePlan 'Microsoft.Web/serverfarms@2022-09-01' = {
  name: appServicePlanName
  location: location
  kind: 'functionapp'
  sku: {
    tier: 'Dynamic'
    name: 'Y1'
  }
}

module storage 'br/public:avm/res/storage/storage-account:0.18.2' = {
  name: 'storageDeployment'
  params: {
    name: functionStorageAccountName
    kind: 'StorageV2'
    location: location
    skuName: 'Standard_LRS'
  }
}

module site 'br/public:avm/res/web/site:0.15.1' = {
  name: 'functionDeployment'
  params: {
    kind: 'functionapp'
    name: functionAppName
    serverFarmResourceId: appServicePlan.id
    location: location
    siteConfig: {
      netFrameworkVersion: 'v9.0'
      use32BitWorkerProcess: false
    }
  }
}

output functionAppUrl string = 'https://${functionAppName}.azurewebsites.net'
