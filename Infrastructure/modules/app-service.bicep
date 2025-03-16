param uniqueName string = uniqueString(resourceGroup().id)
param location string = resourceGroup().location

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

output serverFarmResourceId string = appServicePlan.id
