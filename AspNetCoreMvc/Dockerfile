FROM mcr.microsoft.com/dotnet/sdk:7.0
WORKDIR /app
COPY . .
RUN dotnet restore
RUN dotnet publish AspNetCoreMvc.csproj -c Release -o out
WORKDIR out
ENV ASPNETCORE_URLS="http://*:4500"
ENTRYPOINT ["dotnet", "AspNetCoreMvc.dll"]