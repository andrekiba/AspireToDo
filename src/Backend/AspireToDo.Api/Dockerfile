FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG TARGETARCH
WORKDIR /src
COPY ["AzureToDo.Api.csproj", "Api/"]
RUN dotnet restore "Api/AzureToDo.Api.csproj" -a $TARGETARCH
WORKDIR "/src/Api"
COPY . .
RUN dotnet build "AzureToDo.Api.csproj" -a $TARGETARCH -c Release --no-restore -o /app/build

FROM build AS publish
RUN dotnet publish "AzureToDo.Api.csproj" -a $TARGETARCH -c Release --no-restore -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AzureToDo.Api.dll"]