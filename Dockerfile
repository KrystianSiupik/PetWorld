FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY src/PetWorld.Domain/PetWorld.Domain.csproj src/PetWorld.Domain/
COPY src/PetWorld.Application/PetWorld.Application.csproj src/PetWorld.Application/
COPY src/PetWorld.Infrastructure/PetWorld.Infrastructure.csproj src/PetWorld.Infrastructure/
COPY src/PetWorld.Web/PetWorld.Web.csproj src/PetWorld.Web/
RUN dotnet restore src/PetWorld.Web/PetWorld.Web.csproj

COPY . .
RUN dotnet publish src/PetWorld.Web/PetWorld.Web.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish ./

EXPOSE 5000
ENV ASPNETCORE_URLS=http://+:5000

ENTRYPOINT ["dotnet", "PetWorld.Web.dll"]
