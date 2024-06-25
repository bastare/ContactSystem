FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS server-builder
WORKDIR /source-server
COPY . .
RUN dotnet publish -c Release -o ./build

FROM node:18-alpine as client-builder
WORKDIR /source-client
COPY src/client/ .
RUN npm install --progress=true --loglevel=silent
RUN npm install -g @angular/cli
ARG DOCKER_ENVIRONMENT
RUN ng build --configuration=${DOCKER_ENVIRONMENT}

FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine
WORKDIR /app
COPY --from=server-builder /source-server/build/ .
COPY --from=client-builder /source-client/dist/contact-search/browser ./webroot/

RUN dotnet dev-certs https -ep /https/aspnetapp.pfx
RUN dotnet dev-certs https --trust

ARG DOCKER_ENVIRONMENT
ENV ASPNETCORE_ENVIRONMENT=${DOCKER_ENVIRONMENT}
ENV ASPNETCORE_URLS=https://+:5003;http://+:8080
ENV ASPNETCORE_HTTPS_PORT=5003
ENV ASPNETCORE_Kestrel__Certificates__Default__Password=
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/https/aspnetapp.pfx

EXPOSE 8080
EXPOSE 5003

CMD ["dotnet", "ContactSystem.Api.dll"]