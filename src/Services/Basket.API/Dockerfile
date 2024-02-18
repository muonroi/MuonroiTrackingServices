FROM mcr.microsoft.com/dotnet/aspnet:6.0 as base
EXPOSE 80
WORKDIR /app
FROM mcr.microsoft.com/dotnet/sdk:6.0 as build
WORKDIR /src
COPY ["Services/Basket.API/Basket.API.csproj","Services/Basket.API/"]
COPY ["BuildingBlocks/Common.Logging/Common.Logging.csproj","BuildingBlocks/Common.Logging/"]
COPY ["BuildingBlocks/Contracts/Contracts.csproj","BuildingBlocks/Contracts/"]
COPY ["BuildingBlocks/Infrastructure/Infrastructure.csproj","BuildingBlocks/Infrastructure/"]
COPY ["BuildingBlocks/Shared/Shared.csproj","BuildingBlocks/Shared/"]

RUN dotnet restore "Services/Basket.API/Basket.API.csproj"
COPY . .
WORKDIR "/src/Services/Basket.API"
RUN dotnet build "Basket.API.csproj" -c Release -o /app/build
FROM build as publish

RUN dotnet publish "Basket.API.csproj" -c Release -o /app/publish

FROM base as final 
WORKDIR /app

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Basket.API.dll"]