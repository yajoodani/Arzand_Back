FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY *.sln .
COPY tests/Arzand.Tests/Arzand.Tests.csproj tests/Arzand.Tests/Arzand.Tests.csproj
COPY src/Modules/Payment/Payment.csproj src/Modules/Payment/Payment.csproj
COPY src/Modules/Identity/Identity.csproj src/Modules/Identity/Identity.csproj
COPY src/Modules/Ordering/Ordering.Domain/Ordering.Domain.csproj src/Modules/Ordering/Ordering.Domain/Ordering.Domain.csproj
COPY src/Modules/Ordering/Ordering.Infrastructure/Ordering.Infrastructure.csproj src/Modules/Ordering/Ordering.Infrastructure/Ordering.Infrastructure.csproj
COPY src/Modules/Ordering/Ordering.Application/Ordering.Application.csproj src/Modules/Ordering/Ordering.Application/Ordering.Application.csproj
COPY src/Modules/Catalog/Catalog.Application/Catalog.Application.csproj src/Modules/Catalog/Catalog.Application/Catalog.Application.csproj
COPY src/Modules/Catalog/Catalog.Domain/Catalog.Domain.csproj src/Modules/Catalog/Catalog.Domain/Catalog.Domain.csproj
COPY src/Modules/Catalog/Catalog.Infrastructure/Catalog.Infrastructure.csproj src/Modules/Catalog/Catalog.Infrastructure/Catalog.Infrastructure.csproj
COPY src/Modules/Cart/Cart.csproj src/Modules/Cart/Cart.csproj
COPY src/Shared/Shared.csproj src/Shared/Shared.csproj
COPY src/Api/Api.csproj src/Api/Api.csproj

RUN dotnet restore

COPY tests tests
COPY src/Api/ src/Api/
COPY src/Modules/ src/Modules/
COPY src/Shared/ src/Shared/

RUN dotnet publish src/Api/Api.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Api.dll"]
