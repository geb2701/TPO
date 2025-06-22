FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY ["Tpo/src/Tpo/Tpo.csproj", "./Tpo/src/Tpo/"]
COPY ["SharedKernel/SharedKernel.csproj", "./SharedKernel/"]
RUN dotnet restore "./Tpo/src/Tpo/Tpo.csproj"

# Copy everything else and build
COPY . ./
RUN dotnet build "Tpo/src/Tpo/Tpo.csproj" -c Release -o /app/build

FROM build-env AS publish
RUN dotnet publish "Tpo/src/Tpo/Tpo.csproj" -c Release -o /app/out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=publish /app/out .

ENV ASPNETCORE_URLS=http://+:8080
HEALTHCHECK --interval=30s --timeout=60s CMD curl --fail http://localhost:8080/healthz || exit
EXPOSE 8080

ENTRYPOINT ["dotnet", "/app/Tpo.dll"]
