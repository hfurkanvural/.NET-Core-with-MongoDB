
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /app
# Copy everything and build
COPY . ./
RUN dotnet restore "./docker-api.csproj"
RUN dotnet publish "docker-api.csproj" -c Release -o knowledge_api

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build /app/knowledge_api .
EXPOSE 80
ENTRYPOINT ["dotnet", "docker-api.dll"]