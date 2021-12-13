FROM docker-registry01.localhost/dotnet/aspnet:5.0-alpine3.13-amd64 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM docker-registry01.localhost/dotnet/sdk:5.0-alpine3.13-amd64 AS build
WORKDIR /src
COPY ["Bt.Web.Service.Auth/Bt.Web.Service.Auth.csproj", "Bt.Web.Service.Auth/"]
RUN dotnet restore "Bt.Web.Service.Auth/Bt.Web.Service.Auth.csproj"
COPY . .
WORKDIR "/src/Bt.Web.Service.Auth"
RUN dotnet build "Bt.Web.Service.Auth.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Bt.Web.Service.Auth.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN chown -R aspnet:aspnet /app
USER aspnet
ENTRYPOINT ["dotnet", "Bt.Web.Service.Auth.dll"]
