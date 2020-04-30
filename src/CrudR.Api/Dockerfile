FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine AS publish
WORKDIR /buildfiles
COPY . .

RUN dotnet publish "src/CrudR.Api/CrudR.Api.csproj" -r linux-musl-x64 --self-contained false -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CrudR.dll"]