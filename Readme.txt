ASP.NET Core 2.1 - MVC App

MS SQL - generated via package manager console:
Add-Migration Initial
Update-Database

Connection string is in appsettings.json


API:
Authorization header
Authorization:Basic dGVzdDp0ZXN0

GET api/unpaid
GET api/pay/{id}
PATCH api/edit/{id}