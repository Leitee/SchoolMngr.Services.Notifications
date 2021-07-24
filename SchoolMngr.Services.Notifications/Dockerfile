#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/trainers/notification/SchoolMngr.Services.Notification/SchoolMngr.Services.Notification.csproj", "src/trainers/notification/SchoolMngr.Services.Notification/"]
COPY ["src/core/base/Codeit.NetStdLibrary.Base/Codeit.NetStdLibrary.Base.csproj", "src/core/base/Codeit.NetStdLibrary.Base/"]
COPY ["src/core/shared/SchoolMngr.Services.Shared.csproj", "src/core/shared/"]
RUN dotnet restore "src/trainers/notification/SchoolMngr.Services.Notification/SchoolMngr.Services.Notification.csproj"
COPY . .
WORKDIR "/src/src/trainers/notification/SchoolMngr.Services.Notification"
RUN dotnet build "SchoolMngr.Services.Notification.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SchoolMngr.Services.Notification.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SchoolMngr.Services.Notification.dll"]