FROM microsoft/dotnet:1.1-sdk-msbuild
RUN mkdir -p /usr/src/zulily
COPY . /usr/src/zulily
WORKDIR /usr/src/zulily
RUN cd /usr/src/zulily
RUN dotnet restore
EXPOSE 5000
ENV ASPNETCORE_URLS http://+:5000
CMD [ "dotnet", "run" ]
