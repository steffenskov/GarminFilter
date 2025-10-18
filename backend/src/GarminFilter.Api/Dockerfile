# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy everything from parent directory (all projects)
COPY . .
RUN dotnet restore "src/GarminFilter.Api/GarminFilter.Api.csproj"

# Build
WORKDIR "/src/src/GarminFilter.Api"
RUN dotnet build "GarminFilter.Api.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "GarminFilter.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 8080
# Copy published app
COPY --from=publish /app/publish .

# Set default environment variables
ENV DatabasePath=data
ENV Origin=http://localhost:3000

ENTRYPOINT ["dotnet", "GarminFilter.Api.dll"]