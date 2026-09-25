# Tutorial: Deploying Azure Functions to Azure Cloud

This tutorial provides step-by-step instructions on how to deploy an Azure Function project to Microsoft Azure Cloud.

## Prerequisites

1. **Azure Subscription**: You must have an active Azure subscription.
2. **Azure CLI**: Install Azure CLI (`az`) on your local machine.
3. **Azure Functions Core Tools**: Install Azure Functions Core Tools (`func`).
4. **.NET 8 SDK**: Since our function is built on .NET 8, ensure you have the SDK installed.

## Step 1: Login to Azure

Open your terminal and run the following command to log into your Azure account:

```bash
az login
```

Follow the on-screen instructions in your web browser to authenticate.

## Step 2: Create Azure Resources

You will need a Resource Group, a Storage Account, and a Function App. 

1. **Create a Resource Group:**
   ```bash
   az group create --name BattleGameResourceGroup --location eastus
   ```

2. **Create a Storage Account:**
   Azure Functions require a storage account. 
   ```bash
   az storage account create --name battlegamestorage123 --location eastus --resource-group BattleGameResourceGroup --sku Standard_LRS
   ```

3. **Create the Function App:**
   ```bash
   az functionapp create --resource-group BattleGameResourceGroup --consumption-plan-location eastus --runtime dotnet-isolated --runtime-version 8.0 --functions-version 4 --name BattleGameFunctionApp --storage-account battlegamestorage123
   ```

## Step 3: Deploy the Code

Navigate to the directory containing your Azure Function project (`Backend` directory). Use the Azure Functions Core Tools to deploy the function to the Function App created in the previous step.

```bash
cd Backend
func azure functionapp publish BattleGameFunctionApp
```

## Step 4: Configure Application Settings (Optional but Recommended)

If your function relies on a database (like our EF Core implementation), you must configure the connection string in the Azure Function App settings.

```bash
az functionapp config appsettings set --name BattleGameFunctionApp --resource-group BattleGameResourceGroup --settings "SqlConnectionString=Server=your_server;Database=BATTLEGAME;User Id=sa;Password=your_password;"
```

## Step 5: Test the Deployment

Once the deployment is successful, you will see a list of HTTP trigger URLs in the terminal output. You can use tools like Postman, `curl`, or your web browser to call these endpoints and verify they work as expected.
