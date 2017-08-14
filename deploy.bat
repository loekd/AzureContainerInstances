az group create --name acidemo --location westeurope
az group deployment create --name ContainerGroup --resource-group acidemo --template-file azuredeploy.json --parameters azuredeploy.parameters.json