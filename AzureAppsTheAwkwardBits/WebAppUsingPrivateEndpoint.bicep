// Deploys a Web App, configured to use a VNET.
// Deploys a Storage Account with a private endpoint making its blob storage endpoint available on that VNET.

param location string = resourceGroup().location
param baseName string
param appName string = '${baseName}web'
param planName string = '${baseName}plan'
param planSku string = 'S1'
param storageName string = '${baseName}stg'

var vnetName = 'VsLive2023Vnet'
var vnetAddressPrefix = '10.0.0.0/16'
var mainSubnetPrefix = '10.0.0.0/24'
var mainSubnetName = 'main'
var appServiceSubnetPrefix = '10.0.1.0/24'
var appServiceSubnetName = 'appservice'

resource vnet 'Microsoft.Network/virtualNetworks@2022-09-01' = {
  name: vnetName
  location: location
  properties: {
    addressSpace: { addressPrefixes: [ vnetAddressPrefix ] }
    subnets: [
      {
        name: mainSubnetName
        properties: {
          addressPrefix: mainSubnetPrefix
          
          // This needs to be present for us to be allowed to specify this subnet in
          // the storage account's networkAcls.virtualNetworkRules
          serviceEndpoints: [ { service: 'Microsoft.Storage' } ]
        }
      }
      {
        name: appServiceSubnetName
        properties: {
          addressPrefix: appServiceSubnetPrefix
          delegations: [
            {
              name: 'appservice'
              properties: {
                serviceName: 'Microsoft.Web/serverFarms'
              }
            }
          ]
        }
      }
    ]
  }
}

resource appServicePlan 'Microsoft.Web/serverfarms@2022-03-01' = {
  name: planName
  location: location
  sku: {
    name: planSku
  }
  kind: 'app'
}

resource webApp 'Microsoft.Web/sites@2022-03-01' = {
  name: appName
  location: location
  kind: 'app'
  properties: {
    serverFarmId: appServicePlan.id
    httpsOnly: true

    virtualNetworkSubnetId: vnet.properties.subnets[1].id
    siteConfig: {
      // Causes all outbound requests made by the web app to go
      // via the vnet.
      vnetRouteAllEnabled: true

      http20Enabled: true
      minTlsVersion: '1.2'

      appSettings: [
        {
          name: 'Storage:ConnectionString'
          value: 'DefaultEndpointsProtocol=https;AccountName=idgvsvd1stg;AccountKey=${storageAccount.listKeys().keys[0].value};EndpointSuffix=core.windows.net'
        }
      ]
    }
  }
}

resource storageAccount 'Microsoft.Storage/storageAccounts@2022-09-01' = {
  name: storageName
  location: location
  kind: 'StorageV2'
  sku: {
    name: 'Standard_LRS'
  }
  properties: {
    minimumTlsVersion: 'TLS1_2'
    allowBlobPublicAccess: false

    // Ensure storage account only accessible from VNET
    //networkAcls: {
    //  defaultAction: 'Deny'
    //  virtualNetworkRules: [
    //    { id: vnet.properties.subnets[0].id }
    //  ]
    //}
  }
}      

resource storagePrivateEndpointBlob 'Microsoft.Network/privateEndpoints@2022-09-01' = {
  name: '${storageName}-pe'
  location: location
  properties: {
    subnet: { id: vnet.properties.subnets[0].id }
    privateLinkServiceConnections: [
      {
        name: '${storageName}-blob-pl'
        properties: {
          groupIds: [ 'blob' ]
          privateLinkServiceId: storageAccount.id
        }
      }
    ]
  }
}

// We need a private DNS zone so that when something on the VNET
// tries to connect to the storage account, the hostname (e.g.
// myaccount.blob.core.windows.net) resolves to the IP address of
// the private endpoint.

var blobPrivateDnsZoneName = 'privatelink.blob.${environment().suffixes.storage}'
resource privateEndpointDnsZone 'Microsoft.Network/privateDnsZones@2020-06-01' = {
  name: blobPrivateDnsZoneName
  location: 'global'
}

// Private DNS Zone Groups provide a link between private endpoints
// and DNS zones. This is a child of the private endpoint, but it
// points back to the DNS zone, and it has the effect of adding an
// 'A' record to the DNS zone for this private endpoint.
resource storagePrivateEndpointBlobDnsZoneGroup 'Microsoft.Network/privateEndpoints/privateDnsZoneGroups@2022-09-01' = {
  parent: storagePrivateEndpointBlob
  name: 'privateendpointsdns-blob'
  properties: {
    privateDnsZoneConfigs: [
      {
        name: blobPrivateDnsZoneName
        properties: {
          privateDnsZoneId: privateEndpointDnsZone.id
        }
      }
    ]
  }
}

// This associates our DNS Zone with our VNET.
resource blobPrivateDnsZoneVnetLink 'Microsoft.Network/privateDnsZones/virtualNetworkLinks@2020-06-01' = {
  parent: privateEndpointDnsZone
  name: '${storageAccount.name}-blob'
  location: 'global'
  properties: {
    // We don't want new hosts added to the VNET to cause entries to
    // be added to this DNS zone. We're only here to support blob
    // storage private endpoints.
    registrationEnabled: false
    virtualNetwork: {
      id: vnet.id
    }
  }
}
