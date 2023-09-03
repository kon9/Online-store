# OnlineStore

The OnlineStore solution represents an advanced e-commerce platform built on the .NET Core framework. By leveraging a microservice architecture, it distributes various responsibilities among distinct services, all orchestrated under a central API service.

## Project Structure

- **OnlineStore.ApiService**
  - Central API service for the entire solution.
  - Consists of controllers such as Articles, Orders, Users, Roles, etc.
  - Features include client authorization and integration with the different microservices.

- **OnlineStore.ArticleService**
  - Focuses on article management and pricing.
  - Includes health checks to ensure service monitoring.

- **OnlineStore.AspIdentityServer**
  - Responsible for user identity and token management.
  - Ensures secure API interactions by providing service tokens.

- **OnlineStore.Library**
  - A shared library serving all microservices.
  - Encompasses models, repositories, client configurations, and more.
  - Designed to cater to various domains including Articles, Orders, Users, and Roles.

- **OnlineStore.OrdersService**
  - Dedicated to order-related functionalities.
  - Integrated health checks ensure service availability and monitoring.

- **OnlineStore.UserManagementService**
  - Manages user roles and associated functionalities.
  - Includes health checks for constant service monitoring.

## Key Interfaces & Services

- **IRepoClient<T>**: Standardizes CRUD operations for any entity.
- **IAspIdentityClient**: Designed for the AspIdentity client, primarily for API token generation.
- **IRepo<T>**: Employs the repository pattern to standardize database operations.
- **AuthenticationServiceTest**: Offers comprehensive testing for the authentication mechanism, focusing on identity server integration and user management.

## Dependency Injection

The architecture prioritizes clean design and modularity, achieved primarily through extensive use of Dependency Injection. Some of the DI highlights include:

- HttpClient instances tailored for various client interactions.
- Transient services targeting roles, users, articles, and more.
- Configuration provisions for both API and microservice integrations.
- JWT-based authentication for ensuring secure user sessions.
- Authorization strategies founded on user scopes.

# ToDo
- Add docker
- Fix Authentication
- Tests
- Logger


