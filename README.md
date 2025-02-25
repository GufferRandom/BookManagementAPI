# Description
The Book Management API is a RESTful API developed using ASP.NET Core Web API. It supports CRUD operations, creating, reading, updating, and deleting books. Additionally, it offers features like pagination.
I Have Some Mock Data If Connection String Is Configured Or Docker Is Running Correctly Then It Will Automaticly Migrate That Data Into Sql Server
####  📖 BookManagementAPI EndPoints
| Method  | Endpoint  | Description |
|---------|----------|-------------|
| **GET** | `/api/BookManagement/GetBooksByPopularityScore?PageSize={Size}&PageNumber={Number}` the default Size is 5 and The Default Number is 1 |Retrieve a paginated list of books sorted by popularity in descending order. |
| **GET** | `/api/BookManagement/GetBook/{Id}` |Fetch details of a book, including author, title, view count, popularity, etc.  |
| **POST** | `/api/BookManagement/AddBook` | Add a new book |
| **POST** | `/api/BookManagement/AddBooks` | Add multiple books or One |
| **PUT** | `/api/BookManagement/UpdateBook/{Id}` | Update a book |
| **DELETE** | `/api/BookManagement/DeleteBook/{Id}` | Soft delete a book  |
| **DELETE** | `/api/BookManagement/DeleteBooks` | Soft delete multiple books or one |

# To Run This Project

You can run this project locally, but you need SQL Server and .NET 9.0 installed.
## 1. Clone The Repository
```bash
git clone https://github.com/GufferRandom/BookManagementAPI.git
```
then enter the directory and then subdirectory 
```bash
cd BookManagementAPI/BookManagement.API
```
## 2. Set up your configuration:

- In the `appsettings.json`, change `DB_SERVER` to your SQL Server name.  
  ![image](https://github.com/user-attachments/assets/151fd13b-2359-4ed2-b430-2579b2365386)

- Next, change `DB_PASSWORD` to your SQL Server password.  
  ![image](https://github.com/user-attachments/assets/2afd3ac9-f8b9-432b-b461-128161809813)

## 2. Run the project:

Run the following commands in your terminal:

```bash
dotnet build
dotnet run
```
3.if it is  successful, visit   localhost:5000/swagger to see the API documentation.

## Second Version: Run with Docker and Docker Compose

If you prefer to run the project using Docker, follow these steps:

1. Make sure Docker and Docker Compose are installed.
2. Clone The Git Repository
```bash
git clone https://github.com/GufferRandom/BookManagementAPI.git
```
3.Enter The Directory
```bash
cd BookManagementAPI
```
4. Run the following command to build and start the container:
```bash
docker-compose -f 'docker-compose.yml' up -d --build
```
5.if it is  successful, visit localhost:100/swagger or  localhost:5000/swagger to see the API documentation.


Popularity Formula
Popularity Score = BookViews * 0.5 - YearsSincePublished * 2
