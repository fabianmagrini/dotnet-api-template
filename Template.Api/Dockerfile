# Use Alpine base image
# remove the build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS publish
ARG TARGETARCH
WORKDIR /src
COPY Template.Api.csproj ./
# specify target runtime
RUN dotnet restore "./Template.Api.csproj" -a $TARGETARCH
COPY . .

# optimise dotnet publish
# add --no-restore 
RUN dotnet publish "Template.Api.csproj" -c Release -o /app/publish \
  --no-restore \  
  -a $TARGETARCH

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS final
WORKDIR /app
EXPOSE 80
COPY --from=publish /app/publish .

ENTRYPOINT ["./Template.Api"]