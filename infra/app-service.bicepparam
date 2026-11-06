using './app-service.bicep'

param location = 'francecentral'
param webAppName = 'BloodPressureWebApp'
param appServicePlanName = 'ASP-BloodPressureResourceGroup-b8ae'
param appServicePlanSkuName = 'P0v3'
param appServicePlanSkuTier = 'Premium0V3'
param appServicePlanCapacity = 1
param dotnetVersion = '10.0'
param stagingSlotName = 'staging'
