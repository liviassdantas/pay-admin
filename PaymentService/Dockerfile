#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["pay-admin.csproj", "."]
RUN dotnet restore "./pay-admin.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "pay-admin.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "pay-admin.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "pay-admin.dll"]

FROM alpine:3.19.1
RUN apk add openjdk11
COPY kafka_2.13-3.6.1.tgz .
RUN tar -xvf 'kafka_2.13-3.6.1.tgz'
RUN mv ./kafka_2.13-3.6.1 /opt/
WORKDIR /opt/kafka_2.13-3.6.1
ENV PATH /sbin:/opt/kafka_2.13-3.6.1/bin/:$PATH
CMD ["kafka-server-start.sh", "config/server.properties"]