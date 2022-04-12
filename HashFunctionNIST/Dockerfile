FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["HashFunctionNIST/HashFunctionNIST.csproj", "HashFunctionNIST/"]
RUN dotnet restore "HashFunctionNIST/HashFunctionNIST.csproj"
COPY . .
WORKDIR "/src/HashFunctionNIST"
RUN dotnet build "HashFunctionNIST.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HashFunctionNIST.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HashFunctionNIST.dll"]
