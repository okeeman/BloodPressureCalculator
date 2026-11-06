param location string = resourceGroup().location
param webAppName string
param appServicePlanName string
param appServicePlanSkuName string
param appServicePlanSkuTier string
param appServicePlanCapacity int = 1
param dotnetVersion string = '10.0'
param stagingSlotName string = 'staging'

resource appServicePlan 'Microsoft.Web/serverfarms@2024-11-01' = {
  name: appServicePlanName
  location: location
  kind: 'linux'
  sku: {
    name: appServicePlanSkuName
    tier: appServicePlanSkuTier
    capacity: appServicePlanCapacity
  }
  properties: {
    reserved: true
  }
}

resource webApp 'Microsoft.Web/sites@2024-11-01' = {
  name: webAppName
  location: location
  kind: 'app,linux'
  properties: {
    serverFarmId: appServicePlan.id
    httpsOnly: true
    siteConfig: {
      alwaysOn: true
      ftpsState: 'Disabled'
      linuxFxVersion: 'DOTNETCORE|${dotnetVersion}'
    }
  }
}

resource stagingSlot 'Microsoft.Web/sites/slots@2024-11-01' = {
  parent: webApp
  name: stagingSlotName
  location: location
  properties: {
    serverFarmId: appServicePlan.id
    httpsOnly: true
    siteConfig: {
      alwaysOn: true
      ftpsState: 'Disabled'
      linuxFxVersion: 'DOTNETCORE|${dotnetVersion}'
    }
  }
}

resource ftpPublishingPolicy 'Microsoft.Web/sites/basicPublishingCredentialsPolicies@2024-11-01' = {
  parent: webApp
  name: 'ftp'
  properties: {
    allow: false
  }
}

resource scmPublishingPolicy 'Microsoft.Web/sites/basicPublishingCredentialsPolicies@2024-11-01' = {
  parent: webApp
  name: 'scm'
  properties: {
    allow: false
  }
}

output webAppName string = webApp.name
