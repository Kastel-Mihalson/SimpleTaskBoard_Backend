FROM mcr.microsoft.com/dotnet/aspnet:6.0 as base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 as build
WORKDIR /src
COPY ["Content.API/Content.API.csproj", "Content.API/"]
RUN dotnet restore "Content.API/Content.API.csproj"
COPY . .
WORKDIR "/src/Content.API"
RUN dotnet build "Content.API.csproj" -c Release -o /app/build

FROM build as publish
RUN dotnet publish "Content.API.csproj" -c Release -o /app/publish

FROM base as final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Content.API.dll"]