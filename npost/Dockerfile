FROM mcr.microsoft.com/dotnet/aspnet:8.0

# Vindos da pipeline
ARG JWT_SECRETKEY    
ARG PGHOST
ARG PGUSER
ARG PGPASSWORD
ARG PGPORT

#Variaveis de ambiente para a aplicação
ENV JWT_SECRETKEY=$JWT_SECRETKEY
ENV PGHOST=$PGHOST
ENV PGUSER=$PGUSER
ENV PGPASSWORD=$PGPASSWORD
ENV PGPORT=$PGPORT

WORKDIR /app

# Copia os arquivos gerados pelo dotnet publish no pipeline
COPY ./publish ./

EXPOSE 8060

ENTRYPOINT ["dotnet", "npost.dll"]

# Argumentos padrões
CMD ["--urls", "http://*:8060","--environment", "Production", "--verbosity", "d"]