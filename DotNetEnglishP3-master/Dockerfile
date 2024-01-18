# Installation de MSSQL Server 2019 Express Edition
# Use the official image as a parent image
FROM mcr.microsoft.com/mssql/server:2019-latest

# Set the working directory
WORKDIR /usr/src/app

EXPOSE 1433

# Run the command on container startup
CMD [ "/opt/mssql/bin/sqlservr" ]