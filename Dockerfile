FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS final
WORKDIR /app
COPY deploy .
CMD ASPNETCORE_URLS=http://*:$PORT /app/Idigis.Api