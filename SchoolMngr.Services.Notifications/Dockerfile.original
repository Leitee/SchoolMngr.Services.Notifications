#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["microservices/notifications/SchoolMngr.Services.Notifications/SchoolMngr.Services.Notifications.csproj", "microservices/notifications/SchoolMngr.Services.Notifications/"]
COPY ["infrastructure/shared/SchoolMngr.Infrastructure.Shared.csproj", "infrastructure/shared/"]
COPY ["../../Codeit.NetStdLibrary.Base/Codeit.NetStdLibrary.Base/Codeit.NetStdLibrary.Base.csproj", "../../Codeit.NetStdLibrary.Base/Codeit.NetStdLibrary.Base/"]
COPY ["../../Codeit.NetStdLibrary.Base/Codeit.NetStdLibrary.Base.Abstractions/Codeit.NetStdLibrary.Base.Abstractions.csproj", "../../Codeit.NetStdLibrary.Base/Codeit.NetStdLibrary.Base.Abstractions/"]
RUN dotnet restore "microservices/notifications/SchoolMngr.Services.Notifications/SchoolMngr.Services.Notifications.csproj"
COPY . .
WORKDIR "/src/microservices/notifications/SchoolMngr.Services.Notifications"
RUN dotnet build "SchoolMngr.Services.Notifications.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SchoolMngr.Services.Notifications.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SchoolMngr.Services.Notifications.dll"]