ARG irc7d_port=6667
ARG irc7d_fqdn=localhost

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
RUN \
    git clone https://github.com/IRC7/IRC7.git && \
    dotnet publish IRC7/Irc7d \
      --runtime linux-x64 \
      --self-contained true \
      /p:PublishTrimmed=true \
      /p:PublishSingleFile=true \
      -c Release \
      -o ./output && \
    mv ./output/Irc7d ./output/irc7

FROM alpine:latest
COPY --from=build /app/output /app/output
ARG irc7d_port
ARG irc7d_fqdn
ENV irc7d_port=${irc7d_port}
ENV irc7d_fqdn=${irc7d_fqdn}
RUN \
    apk add libstdc++ krb5-libs
CMD ./output/irc7 --ip 0.0.0.0 --port $listen_port --fqdn $irc7d_fqdn
EXPOSE ${irc7d_port}
