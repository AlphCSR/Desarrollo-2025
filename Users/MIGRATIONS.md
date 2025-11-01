dotnet ef migrations add InitialCreate --project UsersMS.Infrastructure --startup-project UsersMS

dotnet ef database update --project UsersMS.Infrastructure --startup-project UsersMS