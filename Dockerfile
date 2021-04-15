ARG project

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
COPY deploy .

FROM base AS api
ENV APP_FILE=/app/Idigis.Api

FROM ${project} AS final
CMD ASPNETCORE_URLS=http://*:$PORT ${APP_FILE}