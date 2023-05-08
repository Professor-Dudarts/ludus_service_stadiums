FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR .

RUN apt-get update \
    && apt-get install -y --no-install-recommends \
       default-mysql-client \
    && rm -rf /var/lib/apt/lists/*

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR .
COPY ["Ludus Stadium.csproj"]
RUN dotnet restore "Ludus Stadium.csproj"
COPY . .
WORKDIR "/."
RUN dotnet build "Ludus Stadium.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Ludus Stadium.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR .
COPY --from=publish /app/publish .
EXPOSE 80
ENTRYPOINT ["dotnet", "Ludus Stadium.dll"]
