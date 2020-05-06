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
* Docker Hub images for `amd64`, `arm` and `arm64` Linux.
  * Works just fine on a Raspberry Pi 3+ with Docker installed.

## Roadmap
* HTTPS / SSL support
* JWT Bearer Token authentication and authorisation support
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
|ApplicationOptions__BaseUri|*[empty]*|Sets the base uri for all http requests to the CrudR service. For instance, a value of **"api/v1"** here would mean service calls must be made to `http://{address}/api/v1/`. The default is no base uri, which mean you can any uri at `http://{address}/`|
|DatabaseOptions__ConnectionString|***Required***|The Mongo connection string to use|
|DatabaseOptions__DatabaseName|*crudr*|The name CrudR should give to the database it creates in Mongo|

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
