ARG DOCKER_REPO
FROM ${DOCKER_REPO}/dotnet/sdk:10.0 AS build

ARG BUILD_CONFIGURATION=Release

WORKDIR /src

COPY ["src/ToyRobot.Cli/ToyRobot.Cli.csproj", "src/ToyRobot.Cli/"]
COPY ["src/ToyRobot.Application/ToyRobot.Application.csproj", "src/ToyRobot.Application/"]
COPY ["src/ToyRobot.Domain/ToyRobot.Domain.csproj", "src/ToyRobot.Domain/"]

RUN dotnet restore "src/ToyRobot.Cli/ToyRobot.Cli.csproj"

COPY . .

RUN dotnet publish "src/ToyRobot.Cli/ToyRobot.Cli.csproj" \
    -c $BUILD_CONFIGURATION \
    -o /app/publish \
    --no-restore \
    /p:UseAppHost=false

FROM ${DOCKER_REPO}/dotnet/runtime:10.0 AS final

WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "ToyRobot.Cli.dll"]
