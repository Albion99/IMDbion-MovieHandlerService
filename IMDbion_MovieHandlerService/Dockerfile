#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["IMDbion_MovieHandlerService/IMDbion_MovieHandlerService.csproj", "IMDbion_MovieHandlerService/"]
RUN dotnet restore "IMDbion_MovieHandlerService/IMDbion_MovieHandlerService.csproj"
COPY . .
WORKDIR "/src/IMDbion_MovieHandlerService"
RUN dotnet build "IMDbion_MovieHandlerService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "IMDbion_MovieHandlerService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IMDbion_MovieHandlerService.dll"]