This Azure funtion app have a endpoint "api/standardizedDate" which converts any date in "dd/mm/yyyy" format.

supports multi-language, multi-culture(just need to add the culture in the list, for now supports english and french)

## Request Payoad

```json
{
  "inputDate": "sunday 26 jan 2025"
}
```

## Local setup

In order to run this locally you would need 'local.settings.json'

```json
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet-isolated"
  }
}
```
