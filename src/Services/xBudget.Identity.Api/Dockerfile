FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build

WORKDIR /src
COPY . .

RUN dotnet restore
RUN dotnet publish -c Release -o /app


FROM mcr.microsoft.com/dotnet/aspnet:3.1

WORKDIR /app
COPY --from=build /app .

ENTRYPOINT [ "dotnet", "xBudget.Identity.Api.dll" ]
