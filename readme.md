# Azure Functions with OpenAPI/Swagger

```
func init swaggerAzFunc1 --dotnet
dotnet add package Microsoft.Azure.Functions.Extensions
dotnet add package Microsoft.Azure.WebJobs.Extensions.OpenApi --version 0.8.1-preview
func new --name Function1 --template "HTTP Trigger" --authLevel "anonymous"
```

## Run local Azure Storage Emulator: Amurite
https://docs.microsoft.com/en-us/azure/storage/common/storage-use-azurite?tabs=visual-studio-code

Start Azurite from VS Code via F1 + "Azurite: Start"

or manually:

```
azurite --silent --location c:\azurite --debug c:\azurite\debug.log
```

## Notes
Note that Microsoft.Extensions.Configuration v5.0.0 is *broken*! Use v3.1.0 for all nuget assemblies.s
