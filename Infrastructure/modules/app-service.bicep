import {tags as tagsType} from '../types/tags.bicep'

param uniqueName string
param location string
param tags tagsType

param appServicePlanName string = 'plan${uniqueName}'
param applicationInsightsName string = 'insight${uniqueName}'

resource applicationInsight 'Microsoft.Insights/components@2020-02-02' = {
  name: applicationInsightsName
  location: location
  tags: tags
  properties: {
    Application_Type: 'web'
  }
  kind: 'web'
}

resource appServicePlan 'Microsoft.Web/serverfarms@2022-09-01' = {
  name: appServicePlanName
  location: location
  kind: 'linux'
  tags: tags
  sku: {
    tier: 'Dynamic'
    name: 'Y1'
  }
  properties: {
    reserved: true
  }
}

output serverFarmResourceId string = appServicePlan.id
output applicationInsightsResourceId string = applicationInsight.id
