FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["src/ToDoApi.Api/ToDoApi.Api.csproj", "ToDoApi.Api/"]
COPY ["src/ToDoApi.Application/ToDoApi.Application.csproj", "ToDoApi.Application/"]
COPY ["src/ToDoApi.Domain/ToDoApi.Domain.csproj", "ToDoApi.Domain/"]
COPY ["src/ToDoApi.Infrastructure/ToDoApi.Infrastructure.csproj", "ToDoApi.Infrastructure/"]

RUN dotnet restore "ToDoApi.Api/ToDoApi.Api.csproj"
COPY src/ToDoApi.Api/ ./ToDoApi.Api/
COPY src/ToDoApi.Application/ ./ToDoApi.Application/
COPY src/ToDoApi.Domain/ ./ToDoApi.Domain/
COPY src/ToDoApi.Infrastructure/ ./ToDoApi.Infrastructure/

WORKDIR /src/ToDoApi.Api

RUN dotnet build "ToDoApi.Api.csproj" -c Release -o /app/build
RUN dotnet publish "ToDoApi.Api.csproj" -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080
EXPOSE 8081

ENTRYPOINT ["dotnet", "ToDoApi.Api.dll"]