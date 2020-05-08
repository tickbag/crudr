# Introduction 
A simple, general purpose CRUD API to store, well, anyhing really.

This has been developed as part of the Tickbag startup project and should be good for Rapid App Development and Prototyping.

Simply `POST` the Json you want to store, then `GET` the data back from the same URI you `POST`ed to.

`PUT` will update the Json if it's there. The first level Json structure/schema must match what was posted.

Finally, `DELETE` to get rid of the Json.

Easy!

## Features
Currently supported features:
* Supports MongoDb and CosmosDb databases (via Mongo wire protocol).
* Accepts any JSON via `POST` method.
* Supports both JSON arrays and objects as the root object.
* Supports JSON schema validation:
  * Validates updates (`PUT`) against JSON schema of data already stored, but only at the root level.
* Support data concurrency/data revision checking via `ETag` and `If-Match` headers.
* Supports Jwt Bearer Token authentication and claim based authorisation
  * Access can be configured per HTTP method
  * Options include:
    * AllowAnonymous (no auth required)
    * Allow anyone who's authenticated
    * Allow if the user has a specific claim
    * Allow if the user has a specific claim value
* Docker Hub images for `amd64`, `arm` and `arm64` Linux.
  * Works just fine on a Raspberry Pi 3+ with Docker installed.

## Roadmap
* HTTPS / SSL support
* Healthcheck endpoints
* Configurable JSON schema validation depth
* Data level access control lists (ACL)

# Getting Started
There are several ways to get going with CrudR on your project.

## Pull a ready made container from Docker Hub (Recommended)
This is probably the simpliest way if you've got Docker installed and just want to use CrudR.
You can be up and running in minutes:

Ensure you have a Mongo container or local instance running and available. You'll need to know the address for it. It should take the form `mongodb://{address}:27017` or similar (the 27017 is the default port number for Mongo. You may have configured it to an alternative port so do check).

From bash / command prompt / powershell:
```
> docker run -p 5000:80 --env DatabaseOptions__ConnectionString={mongo address} tickbag/crudr
```
Docker will pull the CrudR image and run it with the Connection String you provided to connect to the database. There are a number of configurations available that are detailed further down.

You can view the API by visiting
```
http://localhost:5000/swagger
```
in your browser, or by uing Postman against `http://localhost:5000/{any uri}`.

### Create your own docker-compose file (optional)
If you don't want to keep messing around with Mongo connection strings and dependencies then you can create a docker-compose file.
You'll need Docker Compose installed on your machine. If you're using Docker for Windows it should installed by default.

In a suitable directory on your computer create a text file named `docker-compose.yml` with the following:
```yaml
version: '3.4'

services:
 mongo:
  image: mongo
  restart: always
  ports:
   - "27017:27017"

 mongo-express:
  image: mongo-express
  restart: always
  ports:
   - "5001:8081"
  depends_on:
   - mongo

 crudr:
  image: tickbag/crudr
  environment:
   - DatabaseOptions__ConnectionString=mongodb://mongo:27017
   - DatabaseOptions__DatabaseName=crudr
   - ApplicationOptions__EnableSwaggerUI=true
   - ApplicationOptions__RequireRevisionMatching=false
   - ApplicationOptions__BaseControllerName=
   - ApplicationOptions__UseAuthentication=false
  ports:
   - "5000:80"
  links:
   - mongo
  depends_on:
   - mongo
```
*Note that the indentation and spacing is important in yaml files.*

Now just type:
```
> docker-compose up
```

Docker compose will pull Mongo, MongoExpress and CrudR, start them all up and ensure they are configured for you to use.

You can access the CrudR API as mentioned above.

MongoExpress will also be available on `http://localhost:5001/` for you to see what's happening in the database.

## Build and run the source code yourself
This option allows you to make changes, contribute and play around with the source code.

First off, clone the Repo from GitHub.
```
> git clone https://github.com/tickbag/crudr.git
```

### Using Docker

Head to the root of directory of the Repo and type
```
> docker-compose up
```
That's it. There's already a `docker-compose.yml` file in the repository to use.

You can access the API at `http://localhost:5000/`, or `http://localhost:5000/swagger` to use a nice UI to check it out.

You can also see what's happening in Mongo using MongoExpress at `http://localhost:5001/`

### Using DotNet CLI

Make sure you configure the Mongo database settings in `appsetting.json` in `/src/CrudR.Api/` directory.

Then:
```
> cd src\CrudR.Api\

> dotnet run
```

# Configuration options
CrudR can be configured either via the `appsettings.json` file, or more usefully via environment variables.

The following table outline the configuration environment variables and their meaning:
|Env Var name|Default value|Description|
|--|--|--|
|ApplicationOptions__EnableSwaggerUI|*true*|Enables SwaggerUI at `/swagger`. This provides a nice web interface for testing the API as well as a Swagger/OpenAPI3 json description file|
|ApplicationOptions__RequireRevisionMatching|*false*|Setting this to `true` forces `PUT` and `DELETE` methods to require an `If-Match` header to be set with the correctly matching `Etag` id for that corresponding data entry. When this is set `false`, the header isn't required but can be used if you like. If you do set it, the `If-Match` value must match that on the data record.|
|ApplicationOptions__BaseUri|*[empty]*|Sets the base uri for all http requests to the CrudR service. For instance, a value of `api/v1` here would mean service calls must be made to `http://localhost:5000/api/v1/`. The default is no base uri, which mean you can any uri at `http://localhost:5000/`|
|ApplicationOptions__UseAuthentication|*false*|Turns on Authentication and Authorisation for the application. Setting this to `true` requires a minimum of `AuthOptions__Audience` and `AuthOptions__Authority` to be configured|
|DatabaseOptions__ConnectionString|***Required***|The Mongo connection string to use|
|DatabaseOptions__DatabaseName|*crudr*|The name CrudR should give to the database it creates in Mongo|

If `UseAuthentication` is set to `true`, the following configuration options become available (and necessary):

|Env Var name|Default value|Description|
|--|--|--|
|AuthOptions__Audience|***Required***|Sets the unique identifier for the CrudR Api that clients use to request authentication for at the Auth Server|
|AuthOptions__Authority|***Required***|The Authentciation / Identity server uri used for authenticating users of this Api. This should implement OAuth2 or OpenIdConnect standards and make the public signing key available at the well known discovery endpoint. If this is not available you may need to provide the public issuer signing key locally.|
|AuthOptions__UseLocalIssuerSigningKey|*false*|Set to `true` to provide a local issuer signing key for the Bearer token|
|AuthOptions__IssuerSigningKey|*[empty]*|A string version of the issuer signing key. Usefully for passing the key via environment variable / secret. This value will override the `IssuerSigningKeyFilePath` option|
|AuthOptions__IssuerSigningKeyFilePath|*[empty]*|The file path to the issuer signing key. This should be a valid X509 certificate file|

If you set `UseAuthentication` to `true` and do not setup the `AuthClaims` configuration section, the default behaviour will be to allow anyone who has authenticated to access all the Http methods available.

|Env Var name|Default value|Description|
|--|--|--|
|AuthClaims__GetAllowAnonymous|*false*|Set to `true` to allow anyone access to the `GET` method even if they've not authenticated|
|AuthClaims__GetClaim|*[empty]*|If set, this claim type must be present in the Bearer token for this user to access the `GET` method|
|AuthClaims__GetClaimValue|*[empty]*|If set, this value must be present within the `GetClaim` claim type in the Bearer token for this user to access the `GET` method|
|AuthClaims__PostAllowAnonymous|*false*|Set to `true` to allow anyone access to the `POST` method even if they've not authenticated|
|AuthClaims__PostClaim|*[empty]*|If set, this claim type must be present in the Bearer token for this user to access the `POST` method|
|AuthClaims__PostClaimValue|*[empty]*|If set, this value must be present within the `PostClaim` claim type in the Bearer token for this user to access the `POST` method|
|AuthClaims__PutAllowAnonymous|*false*|Set to `true` to allow anyone access to the `PUT` method even if they've not authenticated|
|AuthClaims__PutClaim|*[empty]*|If set, this claim type must be present in the Bearer token for this user to access the `PUT` method|
|AuthClaims__PutClaimValue|*[empty]*|If set, this value must be present within the `PutClaim` claim type in the Bearer token for this user to access the `PUT` method|
|AuthClaims__DeleteAllowAnonymous|*false*|Set to `true` to allow anyone access to the `DELETE` method even if they've not authenticated|
|AuthClaims__DeleteClaim|*[empty]*|If set, this claim type must be present in the Bearer token for this user to access the `DELETE` method|
|AuthClaims__DeleteClaimValue|*[empty]*|If set, this value must be present within the `DeleteClaim` claim type in the Bearer token for this user to access the `DELETE` method|

# Build and Test
With the repository cloned, go to the repository root directory.

Build the solution by running:
```
> dotnet build CrudR.sln
```

Test the solution by running:
```
> dotnet test CrudR.sln
```

# Contribute
Feel free to contribute.

Just fork the Repo, make some changes, and when you're ready do a Pull Request back to the Upstream.

Please make sure you write good tests around your code and that they all pass. This will be checked as part of the PR process.
