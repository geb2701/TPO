FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY ["Template/src/Template/Template.csproj", "./Template/src/Template/"]
COPY ["SharedKernel/SharedKernel.csproj", "./SharedKernel/"]
RUN dotnet restore "./Template/src/Template/Template.csproj"

# Copy everything else and build
COPY . ./
RUN dotnet build "Template/src/Template/Template.csproj" -c Release -o /app/build

FROM build-env AS publish
RUN dotnet publish "Template/src/Template/Template.csproj" -c Release -o /app/out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=publish /app/out .

ENV ASPNETCORE_URLS=http://+:8080
HEALTHCHECK --interval=30s --timeout=60s CMD curl --fail http://localhost:8080/healthz || exit
EXPOSE 8080

ENTRYPOINT ["dotnet", "/app/Template.dll"]
