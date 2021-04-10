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

See <https://docs.github.com/en/packages/guides/pushing-and-pulling-docker-images> for how you can store and manage Docker images in GitHub Container Registry.

### Publish Container using GitHub Actions

Authenticate to the GitHub Container Registry using a personal access token (PAT). Create a new PAT with the appropriate scopes (read:packages, write:packages, delete:packages)and store the PAT as a repository secret named CR_PAT.

Create a workflow using this <https://github.com/docker/build-push-action> action.

### Using a private package from your GitHub Container Registry

Integrate with a Kubernetes cluster by creating a kubernetes secret of type docker-registry:

```sh
kubectl create secret docker-registry ghcr-secret \
    --docker-server=ghcr.io \
    --docker-username=$USERNAME \
    --docker-password=$GHCR_PAT \
    --namespace=$NAMESPACE
```

Refer to this secret in the imagePullSecrets for the container. It is scoped to the namespace.

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

## Helm

Install <https://helm.sh/docs/intro/install/>.

### Create your Helm chart

```sh
mkdir charts
cd charts
helm create template-api
```

Update template-api/values.yaml:

* Change image.repository to ghcr.io/fabianmagrini/dotnet-api-template
* Change imagePullSecrets to name: ghcr-secret. See [above](###using-a-private-package-from-your-github-container-registry) on creating the secret.
* Change service.type to LoadBalancer
* Change service.port to 9000

Update template-api/Chart.yaml:

* Change appVersion to "0.0.1

Update template-api/templates/deployment.yaml:

* Change spec.template.spec.containers.livenessProbe.httpGet.path to /weatherforecast
* Change spec.template.spec.containers.readinessProbe.httpGet.path to /weatherforecast

```sh
helm upgrade --install template-api . --debug
```

List all pods and services in all namespaces

```sh
kubectl get pods --all-namespaces
kubectl get services --all-namespaces 
```

Note the EXTERNAL-IP for the service if there is an EXTERNAL-IP.

Test using postman when running:
<http://localhost:9000/weatherforecast> or <http://EXTERNAL-IP:9000/weatherforecast> if there is an EXTERNAL-IP,
