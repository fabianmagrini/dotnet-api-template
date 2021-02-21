# Use Alpine base image
# remove the build stage
FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine AS publish
WORKDIR /src
COPY Template.Api.csproj ./
# specify target runtime
RUN dotnet restore "./Template.Api.csproj" --runtime alpine-x64
COPY . .

# optimise dotnet publish
# add --no-restore 
RUN dotnet publish "Template.Api.csproj" -c Release -o /app/publish \
  --no-restore \  
  --runtime alpine-x64 \
  --self-contained true \
  /p:PublishTrimmed=true \
  /p:PublishSingleFile=true

# use runtime deps image
FROM mcr.microsoft.com/dotnet/runtime-deps:5.0-alpine AS final

# create user and change ownership
RUN adduser --disabled-password \
  --home /app \
  --gecos '' dotnetuser && chown -R dotnetuser /app

# impersonate 
USER dotnetuser
WORKDIR /app

# expose as 8080 as only root can use below 1024 
EXPOSE 8080
COPY --from=publish /app/publish .
ENTRYPOINT ["./Template.Api", "--urls", "http://+:8080"]