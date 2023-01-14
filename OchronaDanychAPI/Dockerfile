FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /App

# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore
# Build and publish a release
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /App
COPY --from=build-env /App/out .
# ENV DOTNET_EnableDiagnostics=0
ENV ASPNETCORE_URLS="https://+;http://+"
ENV ASPNETCORE_HTTPS_PORT=8001
ENV ASPNETCORE_Kestrel__Certificates__Default__Password="haslo123"
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/https/aspnetapp.pfx

COPY https/aspnetapp.pfx /https/aspnetapp.pfx

ENTRYPOINT ["dotnet", "OchronaDanychAPI.dll"]