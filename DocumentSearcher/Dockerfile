#Build
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /source

COPY . .
RUN dotnet restore "./DocumentSearcher.csproj" --disable-parallel
RUN dotnet publish "./DocumentSearcher.csproj" -c release -o /app --no-restore

#Serve

FROM mcr.microsoft.com/dotnet/sdk:6.0
WORKDIR /app
COPY --from=build /app ./

EXPOSE 5000

ENTRYPOINT ["dotnet", "DocumentSearcher.dll"]