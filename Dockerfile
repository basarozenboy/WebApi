FROM mcr.microsoft.com/dotnet/aspnet
WORKDIR /app
COPY bin/Release/net7.0/publish /app/
ENTRYPOINT ["dotnet", "WebApi.dll"]