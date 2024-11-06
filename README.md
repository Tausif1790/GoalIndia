# ECommerce Website Project

An advanced, proof-of-concept e-commerce platform built with **.NET Core** and **Angular**, integrating **Stripe** for secure payment processing and optimized for scalability, performance, and user-friendly interactions. This project demonstrates a full-stack approach to modern web development, incorporating industry-standard practices and design patterns.

## Project Overview

This application serves as a complete e-commerce solution, showcasing essential e-commerce functionalities, including secure payment processing, user authentication, and a dynamic, responsive user interface. It emphasizes modular design, maintainable code, and high performance using caching and efficient data handling techniques.

## Key Features

- **Full-Stack Development**: Leveraged **Angular 18** for a dynamic front-end and **.NET 8** for a scalable back-end API, creating a seamless user experience.
- **Secure Payment Processing**: Integrated **Stripe** with 3D Secure compliance to handle payment processing, enhancing user trust and data security.
- **Efficient Data Management**: Utilized **Repository** and **Unit of Work** patterns for organized data access, along with the **Specification Pattern** for handling complex queries.
- **High Performance with Caching**: Employed **Redis** to cache shopping cart data, reducing server load and improving response times.
- **Authentication and Authorization**: Implemented **ASP.NET Identity** for role-based access control, allowing secure user login, registration, and role management.

## Technology Stack

- **Backend**: .NET Core 8, Entity Framework Core
- **Frontend**: Angular 18, Angular Material, Tailwind CSS
- **Database**: SQL Server (with Redis for caching)
- **Payment Integration**: Stripe API
- **Hosting**: Azure
- **Version Control**: Git

## Project Highlights

- **Responsive UI**: Built with Angular and Tailwind CSS for a modern, mobile-friendly interface that provides an engaging shopping experience.
- **Advanced Form Handling**: Developed reusable form components and multistep wizards using Angular Reactive Forms, enhancing form usability and data validation.
- **Flexible & Scalable Architecture**: Designed with a multi-project structure, utilizing **Angular Lazy Loading** and **Multiple DbContext** for modularity and scalability.
- **Real-Time Updates**: Leveraged **SignalR** for real-time order status updates, enhancing interactivity and engagement for users.
- **Comprehensive Search and Filter**: Implemented paging, sorting, and filtering functionalities to streamline product discovery.

## Getting Started

To run this project locally, ensure you have the following installed:

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js & Angular CLI](https://nodejs.org/en/download/) (for Angular development)
- SQL Server (or configure another relational database)

### Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/yourusername/ecommerce-project.git
   cd ecommerce-project
   ```

2. Set up the **backend**:
   ```bash
   cd API
   dotnet restore
   dotnet ef database update
   dotnet run
   ```

3. Set up the **frontend**:
   ```bash
   cd ../client
   npm install
   ng serve
   ```

### Usage

- Access the application at `http://localhost:4200`.
- Explore features such as user registration, product search, shopping cart management, and secure checkout using Stripe.

## Screenshots

![image](https://github.com/user-attachments/assets/0669a90e-a638-42e0-8fd9-2f8904d05313)
  
![image](https://github.com/user-attachments/assets/b44b471c-ac6f-471a-8b6a-0310195af144)

![image](https://github.com/user-attachments/assets/883cf146-2c42-466c-84c6-59657963db78)
 

## Future Improvements

- **Enhanced Analytics**: Implement tracking and reporting for better user insights.
- **Wishlist Functionality**: Allow users to save items for future purchases.
- **Admin Dashboard**: Extend the application with an admin portal for order and inventory management.
