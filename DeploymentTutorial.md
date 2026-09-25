# Tutorial: Deploying Azure Functions to Azure Cloud

This tutorial provides step-by-step instructions on how to deploy an Azure Function project (BattleGame Backend) to Microsoft Azure Cloud.

## Prerequisites

1. **Azure Subscription**: You must have an active Azure subscription. You can create a free account at [https://azure.microsoft.com/free](https://azure.microsoft.com/free).
2. **Azure CLI**: Install Azure CLI (`az`) on your local machine. Download from [https://docs.microsoft.com/cli/azure/install-azure-cli](https://docs.microsoft.com/cli/azure/install-azure-cli).
3. **Azure Functions Core Tools**: Install Azure Functions Core Tools v4 (`func`). Download from [https://docs.microsoft.com/azure/azure-functions/functions-run-local](https://docs.microsoft.com/azure/azure-functions/functions-run-local).
4. **.NET 8 SDK**: Since our function is built on .NET 8 isolated worker, ensure you have the SDK installed.

## Project Architecture

The BattleGame project uses the following architecture:

- **Backend**: Azure Functions (.NET 8 isolated worker) with Entity Framework Core
- **Database**: SQL Server (BATTLEGAME database)
- **Frontend**: ReactJS (Vite) displaying the Player Assets Report
- **APIs**:
  - `registerplayer` (POST) - Register a new player
  - `createasset` (POST) - Create a new asset
  - `getassetsbyplayer` (GET) - Get all player assets report

## Step 1: Login to Azure

Open your terminal and run the following command to log into your Azure account:

```bash
az login
```

Follow the on-screen instructions in your web browser to authenticate.

## Step 2: Create Azure Resources

You will need a Resource Group, a Storage Account, an Azure SQL Database, and a Function App.

### 2.1. Create a Resource Group

```bash
az group create --name BattleGameResourceGroup --location eastus
```

### 2.2. Create a Storage Account

Azure Functions require a storage account for internal operations.

```bash
az storage account create \
  --name battlegamestorage123 \
  --location eastus \
  --resource-group BattleGameResourceGroup \
  --sku Standard_LRS
```

### 2.3. Create an Azure SQL Database

Create a SQL Server and database on Azure for the BATTLEGAME database:

```bash
# Create SQL Server
az sql server create \
  --name battlegame-sqlserver \
  --resource-group BattleGameResourceGroup \
  --location eastus \
  --admin-user sqladmin \
  --admin-password "YourStrong!Passw0rd"

# Allow Azure services to access the SQL Server
az sql server firewall-rule create \
  --resource-group BattleGameResourceGroup \
  --server battlegame-sqlserver \
  --name AllowAzureServices \
  --start-ip-address 0.0.0.0 \
  --end-ip-address 0.0.0.0

# Create the BATTLEGAME database
az sql db create \
  --resource-group BattleGameResourceGroup \
  --server battlegame-sqlserver \
  --name BATTLEGAME \
  --service-objective S0
```

### 2.4. Create the Function App

```bash
az functionapp create \
  --resource-group BattleGameResourceGroup \
  --consumption-plan-location eastus \
  --runtime dotnet-isolated \
  --runtime-version 8.0 \
  --functions-version 4 \
  --name BattleGameFunctionApp \
  --storage-account battlegamestorage123
```

## Step 3: Configure Application Settings

Configure the SQL connection string so the Azure Function can connect to the Azure SQL Database:

```bash
az functionapp config appsettings set \
  --name BattleGameFunctionApp \
  --resource-group BattleGameResourceGroup \
  --settings "SqlConnectionString=Server=tcp:battlegame-sqlserver.database.windows.net,1433;Database=BATTLEGAME;User Id=sqladmin;Password=YourStrong!Passw0rd;Encrypt=True;TrustServerCertificate=False;"
```

## Step 4: Deploy the Code

Navigate to the `Backend` directory containing the Azure Function project. Use the Azure Functions Core Tools to deploy:

```bash
cd Backend
func azure functionapp publish BattleGameFunctionApp
```

After deployment, the terminal will output the HTTP trigger URLs for each function:

- `https://battlegamefunctionapp.azurewebsites.net/api/registerplayer`
- `https://battlegamefunctionapp.azurewebsites.net/api/createasset`
- `https://battlegamefunctionapp.azurewebsites.net/api/getassetsbyplayer`

## Step 5: Test the Deployment

You can verify the deployment using `curl` or Postman:

### Test registerplayer (POST)

```bash
curl -X POST https://battlegamefunctionapp.azurewebsites.net/api/registerplayer \
  -H "Content-Type: application/json" \
  -d '{"playerName":"Player 1","fullName":"Test Player","age":"20","level":10,"email":"player1@test.com"}'
```

### Test createasset (POST)

```bash
curl -X POST https://battlegamefunctionapp.azurewebsites.net/api/createasset \
  -H "Content-Type: application/json" \
  -d '{"assetName":"Hero 1","levelRequire":5}'
```

### Test getassetsbyplayer (GET)

```bash
curl https://battlegamefunctionapp.azurewebsites.net/api/getassetsbyplayer
```

Expected response format:

```json
[
  {
    "playerName": "Player 1",
    "level": 10,
    "age": "20",
    "assetName": "Hero 1"
  }
]
```

## Running Locally with Docker Compose

For local development, the project includes a `docker-compose.yml` that sets up all services:

```bash
docker compose up --build
```

This starts:
- **SQL Server** on port `1433`
- **Azurite** (Azure Storage emulator) on port `10000`
- **Backend** (Azure Functions) on port `8080`
- **Frontend** (ReactJS) on port `3000`

Access the frontend at `http://localhost:3000` and the API at `http://localhost:8080/api/getassetsbyplayer`.
