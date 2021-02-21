# Containerising an Asp.Net Core 5.0 Web Api

Containerising an Asp.Net Core 5.0 Web Api.

## Prerequisites

* .Net 5.0
* Docker Desktop
* Visual Studio Code
* Postman

## New webapi

This is intended as a template api but it is not ready for production just yet!

```sh
dotnet new webapi -o Template.Api --no-https
cd Template.Api/
code .
```

## Dockerfile

Create a Dockerfile based on my sample here:

<https://github.com/fabianmagrini/dotnet-samples/tree/master/docker-aspnetcore/HelloWebApi/dockerfiles/nonroot>

Then:

```sh
docker build -t template-api:0.0.1 .
docker image ls | grep template-api # to verify image is built
docker run -it --rm -p 8080:8080 template-api:0.0.1
```

Test using postman when running:
<http://localhost:8080/weatherforecast>

Ctrl-c to exit.

Scan image for vulnerabilities

```sh
docker scan template-api:0.0.1
```
