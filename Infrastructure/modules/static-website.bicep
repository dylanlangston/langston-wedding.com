param uniqueName string = uniqueString(resourceGroup().id)
param location string = resourceGroup().location

param storageAccountName string = 'st${uniqueName}'

module staticWebSite 'br/public:avm/res/storage/storage-account:0.18.2' = {
  name: 'staticWebSiteDeployment'
  params: {
    name: storageAccountName
    kind: 'StorageV2'
    location: location
    skuName: 'Standard_LRS'
    allowBlobPublicAccess: true
    supportsHttpsTrafficOnly: true
    networkAcls: {
      defaultAction: 'Allow'
      bypass: 'AzureServices'
      ipRules: []
      virtualNetworkRules: []
    }
  }
}

output staticWebsiteUrl string = staticWebSite.outputs.primaryBlobEndpoint
