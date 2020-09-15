FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine AS build-env
RUN apk add --no-cache git busybox

WORKDIR /app

COPY . .

RUN cd /app && \
    dotnet publish -c Release -o /Publish

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine
WORKDIR /app
RUN apk add --no-cache curl tzdata
COPY --from=build-env /Publish/ /app
VOLUME /data
CMD ["dotnet", "mediver-server.dll"]