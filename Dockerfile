FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
ENV ASPNETCORE_ENVIRONMENT=Production

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY FinBackend/FinBackend.csproj FinBackend/
WORKDIR /src/FinBackend
RUN dotnet restore

COPY FinBackend/. . 
RUN dotnet publish -c Release --no-restore -o /app/publish

FROM node:20 AS frontend
WORKDIR /app

COPY FinDaily/package*.json ./
RUN npm ci

COPY FinDaily/. . 
RUN npm run build

FROM base AS final
WORKDIR /app

ENV DATABASE_URL=$DATABASE_URL
ENV JWT_SECRET=$JWT_SECRET

COPY --from=build /app/publish . 
COPY --from=frontend /app/dist ./wwwroot

RUN echo "DATABASE_URL=$DATABASE_URL"
RUN echo "JWT_SECRET is set"

CMD ["dotnet", "FinBackend.dll"]
