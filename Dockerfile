# Build stage
FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build
WORKDIR /SOURCE
COPY . .
RUN dotnet restore "./KindleToolAPI/KindleToolAPI/KindleToolAPI.csproj" --disable-parallel
RUN dotnet publish "./KindleToolAPI/KindleToolAPI/KindleToolAPI.csproj" -c release -o /app --no-restore

# Serve stage
FROM mcr.microsoft.com/dotnet/sdk:6.0-focal
WORKDIR /app
COPY --from=build /app ./

#heroku
CMD ASPNETCORE_URLS=http://*:$PORT dotnet KindleToolAPI.dll

#docker
#EXPOSE 5000
#ENTRYPOINT ["dotnet", "KindleToolAPI.dll"]