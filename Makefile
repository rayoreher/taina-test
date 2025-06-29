include .env

# Start SQL Server
up:
	docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=$(SA_PASSWORD)" -p $(DB_PORT):1433 --name $(CONTAINER_NAME) -h $(CONTAINER_NAME) -d $(SQL_SERVER_IMAGE)
	@echo Waiting for SQL Server to start...
	@timeout /t 10 >nul
	@echo SQL Server should be ready!

# Stop SQL Server
down:
	docker stop $(CONTAINER_NAME) || true
	docker rm $(CONTAINER_NAME) || true

# Restart SQL Server
restart: down up

# Create database
create-db:
	docker exec -it $(CONTAINER_NAME) /opt/mssql-tools18/bin/sqlcmd -S localhost -U $(DB_USER) -P $(SA_PASSWORD) -C -Q "CREATE DATABASE [$(DB_NAME)]"
	@echo Database $(DB_NAME) created!

# Run migrations (create tables)
migrate: create-db
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
setup: migrate seed
	@echo Full database setup completed!

# Clean everything
clean:
	docker stop $(CONTAINER_NAME) || true
	docker rm $(CONTAINER_NAME) || true