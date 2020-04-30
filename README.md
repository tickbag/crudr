# Introduction 
A simple, general purpose CRUD API to store, well, anyhing really.

This has been developed as part of the Tickbag startup project and should be good for Rapid App Development and Prototyping.

Simply `POST` the Json you want to store, then `GET` the data back from the same URI you `POST`ed to.

`PUT` will update the Json if it's there. The first level Json structure/schema must match what was posted.

Finally, `DELETE` to get rid of the Json.

Easy!

# Getting Started
First off, clone the Repo.

## Using Docker (Recommended)

Head to the root of directory of the Repo and type
```
docker-compose up
```
That's it. You can access the API at `http://localhost:5000/Store/`, or `http://localhost:5000/swagger` to use a nice UI to check it out.

You can also see what's happening in Mongo using MongoExpress at `http://localhost:5001/`

## Using DotNet CLI

Make sure you configure the Mongo database settings in `appsetting.json` in `/src/CrudR.Api/` directory.

Then:
```
> cd src\CrudR.Api\

src\CrudR.Api> dotnet run
```

# Build and Test
COMING SOON!

# Contribute
Feel free to contribute.

Just fork the Repo, make some changes, and when you're ready do a Pull Request back to the Upstream.

Please make sure you write good tests around your code and that they all pass. This will be checked as part of the PR process.
