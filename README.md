# Welcome to Crypro Currency Project!

if database not exist please use this command for create database
select project Cryptocurrency.Infrastructure if use visual studio PackageManagerTerminal 
**dotnet ef database update --project Cryptocurrency.Infrastructure --startup-project Cryptocurrency.API**

For test api first use login api with data id database data exist

    {
    	"username":  "admin",
    	"password":  "Admin@123"
    }

If no data for test use **SqLite** database viewer and add 
to user table this data

    INSERT  INTO  Users (UserId, UserName, Password)
    VALUES (1,"admin","6G94qKPK8LYNjnTllCqm2G3BUM08AzOK7yW30tfjrMc");

### use postman collection in project for test get crypto exchange rate and history
