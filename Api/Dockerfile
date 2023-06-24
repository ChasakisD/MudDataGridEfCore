FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

COPY ["MudDataGridEfCore.csproj", "Api/"]

RUN dotnet restore "Api/MudDataGridEfCore.csproj"
WORKDIR "/src/Api"
COPY . .
RUN dotnet build "MudDataGridEfCore.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MudDataGridEfCore.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MudDataGridEfCore.dll"]