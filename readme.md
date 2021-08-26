# Azure Functions with OpenAPI/Swagger

```
func init swaggerAzFunc1 --dotnet
dotnet add package Microsoft.Azure.Functions.Extensions
dotnet add package Microsoft.Azure.WebJobs.Extensions.OpenApi --version 0.8.1-preview
func new --name Function1 --template "HTTP Trigger" --authLevel "anonymous"
```
