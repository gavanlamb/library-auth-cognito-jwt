# Expensely.Authentication.Cognito.Jwt

[![Build Status](https://dev.azure.com/expensely-au/Expensely/_apis/build/status/Expensely.library-auth-cognito-jwt?branchName=main)](https://dev.azure.com/expensely-au/Expensely/_build/latest?definitionId=39&branchName=main)

| View       | Badge                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               |
|:-----------|:--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| Prerelease | [![Expensely.Authentication.Cognito.Jwt package in expensely-au@Prerelease feed in Azure Artifacts](https://feeds.dev.azure.com/expensely-au/_apis/public/Packaging/Feeds/4634f7ff-ee1a-49bd-b3de-2f19eb18d3e1@0b477f7e-e363-4441-97f7-bf3189253564/Packages/0205fb37-f302-495e-bf20-2038bcb1c5e1/Badge)](https://dev.azure.com/expensely-au/Expensely/_packaging?_a=package&feed=4634f7ff-ee1a-49bd-b3de-2f19eb18d3e1%400b477f7e-e363-4441-97f7-bf3189253564&package=0205fb37-f302-495e-bf20-2038bcb1c5e1&preferRelease=true) |
| Release    | [![Expensely.Authentication.Cognito.Jwt package in expensely-au@Release feed in Azure Artifacts](https://feeds.dev.azure.com/expensely-au/_apis/public/Packaging/Feeds/4634f7ff-ee1a-49bd-b3de-2f19eb18d3e1@f9bccf78-9a6f-4e24-bcd7-b5f77186974c/Packages/0205fb37-f302-495e-bf20-2038bcb1c5e1/Badge)](https://dev.azure.com/expensely-au/Expensely/_packaging?_a=package&feed=4634f7ff-ee1a-49bd-b3de-2f19eb18d3e1%40f9bccf78-9a6f-4e24-bcd7-b5f77186974c&package=0205fb37-f302-495e-bf20-2038bcb1c5e1&preferRelease=true)    |


## How to Use  
### Configuration  
| Property Name | Description              |
|:--------------|:-------------------------|
| Auth          | An object of auth config |

Add Configuration
``` json
{
  "Cognito": {
    "Issuer": "url",
    "JwtKeySetUrl": "url",
    "Scopes": [
      "scope1"
    ]
  }
}
```


Add Authentication with configuration
``` csharp
public void ConfigureServices(IServiceCollection services)
{
    ...
    services.AddCognitoJwt(Configuration);
    ...
}

public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    ...
    app.UseAuth();
    ...
}
```

## Development
### Build, Package & Release
#### Locally
```
// Step 1: Authenticate
dotnet build --configuration release 

// Step 2: Pack
dotnet pack --configuration release 

// Step 3: Publish
dotnet nuget push "Expensely.Logging.Serilog.*.nupkg" -Source "github"
```
