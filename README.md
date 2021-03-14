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

<https://github.com/fabianmagrini/dotnet-samples/blob/master/docker-aspnetcore/HelloWebApi/dockerfiles/optimise/Dockerfile>

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

## GitHub Container Registry

* <https://docs.github.com/en/packages/guides/pushing-and-pulling-docker-images>

## Kubernetes

Enable a local kubernetes cluster using Docker Desktop. Use the Kubernetes extension in Visual Studio Code to help author the deployment and service yaml.

```sh
kubectl config get-contexts # check that the docker desktop is the current context
kubectl apply -f deployment.yaml
kubectl get deployments
kubectl get pods
kubectl logs template-api-76f94b7cbc-hw4wc # use pod name from previous command
kubectl apply -f service.yaml
kubectl get services
```

Test using postman when running:
<http://localhost:8080/weatherforecast>

### Cleaning up

```sh
kubectl delete services template-api
kubectl delete deployment template-api
```

### Deploying the Dashboard UI

Let's install the Dahsboard:  <https://kubernetes.io/docs/tasks/access-application-cluster/web-ui-dashboard/>

```sh
kubectl apply -f https://raw.githubusercontent.com/kubernetes/dashboard/v2.0.0/aio/deploy/recommended.yaml
```

Accessing the Dashboard UI

```sh
kubectl proxy
```

Dashboard will be available at <http://localhost:8001/api/v1/namespaces/kubernetes-dashboard/services/https:kubernetes-dashboard:/proxy/>.
