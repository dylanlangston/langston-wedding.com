import {tags as tagsType} from '../types/tags.bicep'

param uniqueName string
param location string
param tags tagsType

param appServicePlanName string = 'plan${uniqueName}'

resource appServicePlan 'Microsoft.Web/serverfarms@2022-09-01' = {
  name: appServicePlanName
  location: location
  kind: 'linux'
  tags: tags
  sku: {
    tier: 'Dynamic'
    name: 'Y1'
  }
}

output serverFarmResourceId string = appServicePlan.id
