param uniqueName string
param location string
param tags object

var cosmosAccountName = 'cosmosdb${uniqueName}'

resource cosmosdbAccount 'Microsoft.DocumentDB/databaseAccounts@2021-04-15' = {
  name: cosmosAccountName
  location: location
  tags: tags
  kind: 'GlobalDocumentDB'
  properties: {
    databaseAccountOfferType: 'Standard'
    locations: [
      {
        locationName: location
        failoverPriority: 0
        isZoneRedundant: false
      }
    ]
    consistencyPolicy: {
      defaultConsistencyLevel: 'Eventual'
    }
  }
}

output cosmosdbAccountEndpoint string = cosmosdbAccount.properties.documentEndpoint
output cosmosdbAccountId string = cosmosdbAccount.id
