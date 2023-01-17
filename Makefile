migration:
	dotnet ef migrations add $(name)
reset-db:
	dotnet ef database update 0
apply-migrations:
	dotnet ef database update
run:
	dotnet watch run