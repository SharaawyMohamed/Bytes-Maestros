# Bytes Maestros

Welcome to **Bytes Maestros**, a platform built to simplify the development, collaboration, and enhancement of coding projects.

## 📌 Overview

This repository contains the source code and documentation for the Bytes Maestros project. It is designed with clean architecture principles and emphasizes maintainability, scalability, and modularity.

## 🏗️ Project Structure

- **Backend**: ASP.NET Core (.NET 8)  
- **Database**: SQL Server  
- **Design Pattern**: Clean Architecture with MediatR and Unit of Work  

## Features ✨

- **Product Management**:
  - Categorize products by type (In-Stock, Fresh Food, External)
  - Track inventory levels
  - Manage product details and images

- **Order Processing**:
  - Customer order creation
  - Automatic stock deduction
  - Order status tracking

- **Smart Delivery Scheduling**:
  - Automatic time slot generation
  - Best delivery time calculation
  - Type-specific delivery rules

- **Customer Experience**:
  - Order history tracking
  - Delivery time management
  - Shopping cart functionality



## 🛠️ Technologies Used

- 📦 Modular project layers
- 🧪 Unit testing with xUnit and FluentAssertions
- 📄 Clean, documented Resful API 

| Technology            | Description                  |
|-----------------------|------------------------------|
| ASP.NET Core          | Backend framework            |
| Entity Framework Core | ORM for data access          |
| SQL Server            | Relational database          |
| MediatR               | CQRS and mediator pattern    |
| Mapester              | Object mapping               |

## 🧪 Testing

Unit tests are written using:

- `xUnit`
- `FluentAssertions`

To run tests:
Live Demo: http://bytesmaestros.runasp.net/swagger/index.html
dotnet test
