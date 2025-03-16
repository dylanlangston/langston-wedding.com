import {tags as tagsType} from '../types/tags.bicep'

param uniqueName string = uniqueString(resourceGroup().id)
param location string = resourceGroup().location
param tags tagsType

param storageAccountName string = 'st${uniqueName}'

module staticWebSite 'br/public:avm/res/storage/storage-account:0.18.2' = {
  name: 'staticWebSiteDeployment'
  params: {
    name: storageAccountName
    kind: 'StorageV2'
    tags: tags
    location: location
    skuName: 'Standard_LRS'
    supportsHttpsTrafficOnly: true
    minimumTlsVersion: 'TLS1_2'
    publicNetworkAccess: 'Enabled'
  }
}

output staticWebsiteUrl string = staticWebSite.outputs.primaryBlobEndpoint
