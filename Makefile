include .env

# Start SQL Server
up:
	docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=$(SA_PASSWORD)" -p $(DB_PORT):1433 --name $(CONTAINER_NAME) -h $(CONTAINER_NAME) -d $(SQL_SERVER_IMAGE)
	@echo Waiting for SQL Server to start...
	@timeout /t 10 >nul
	@echo SQL Server should be ready!

# Stop SQL Server
down:
	docker-compose down

# Restart SQL Server
restart: down up

# Create database
create-db:
	docker exec -it $(CONTAINER_NAME) /opt/mssql-tools18/bin/sqlcmd -S localhost -U $(DB_USER) -P $(SA_PASSWORD) -C -Q "CREATE DATABASE [$(DB_NAME)]"
	@echo Database $(DB_NAME) created!

# Run migrations (create tables)
migrate:
	@echo Running migrations...
	docker cp sql-scripts/migrations.sql $(CONTAINER_NAME):/tmp/migrations.sql
	docker exec -i $(CONTAINER_NAME) /opt/mssql-tools18/bin/sqlcmd -S localhost -U $(DB_USER) -P $(SA_PASSWORD) -C -i /tmp/migrations.sql
	@echo Migrations completed!

# Seed database with sample data
seed:
	@echo Seeding database...
	docker cp sql-scripts/seed.sql $(CONTAINER_NAME):/tmp/seed.sql
	docker exec -i $(CONTAINER_NAME) /opt/mssql-tools18/bin/sqlcmd -S localhost -U $(DB_USER) -P $(SA_PASSWORD) -C -i /tmp/seed.sql
	@echo Seeding completed!

# Full setup: create db + migrate + seed
setup: create-db migrate seed
	@echo Full database setup completed!

# Copy SQL files to container (now automatic)
copy-sql:
	docker cp sql-scripts/migrations.sql $(CONTAINER_NAME):/tmp/migrations.sql
	docker cp sql-scripts/seed.sql $(CONTAINER_NAME):/tmp/seed.sql
	@echo SQL files copied to container

# Show connection string
connection:
	@echo.
	@echo === CONNECTION STRING FOR .NET CORE 3.1 ===
	@echo Server=$(DB_HOST),$(DB_PORT);Database=$(DB_NAME);User Id=$(DB_USER);Password=$(SA_PASSWORD);
	@echo.

# Check status
status:
	docker ps
	@echo.
	docker exec -it $(CONTAINER_NAME) /opt/mssql-tools18/bin/sqlcmd -S localhost -U $(DB_USER) -P $(SA_PASSWORD) -C -Q "SELECT name FROM sys.databases WHERE name = '$(DB_NAME)'"

# Show data
show-data:
	@echo === PEOPLE TABLE ===
	docker exec -it $(CONTAINER_NAME) /opt/mssql-tools18/bin/sqlcmd -S localhost -U $(DB_USER) -P $(SA_PASSWORD) -C -Q "USE $(DB_NAME); SELECT TOP 5 * FROM People"

# Clean everything
clean:
	docker-compose down
	docker container prune -f
	docker system prune -f

# Help
help:
	@echo Available commands:
	@echo   up         - Start SQL Server
	@echo   down       - Stop SQL Server  
	@echo   restart    - Restart SQL Server
	@echo   create-db  - Create the database
	@echo   migrate    - Run migrations (create tables)
	@echo   seed       - Seed database with sample data
	@echo   setup      - Full setup (create-db + migrate + seed)
	@echo   copy-sql   - Copy SQL files to container
	@echo   connection - Show connection string
	@echo   status     - Check if everything is running
	@echo   show-data  - Show sample data from tables
	@echo   clean      - Clean up everything