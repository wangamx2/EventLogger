# 📘 EventLogger API — Solution Overview

## 🏗️ Architecture Overview

This project implements a secure, cloud-native API using **Clean Architecture** principles with **CQRS**, **EFCore**, and **MongoDB** in a **polyglot persistence** setup:

- **SQL Server (structured)**: Stores user ID, event type, and timestamp
- **MongoDB (flexible JSON)**: Stores contextual event details
- **Shared key (`eventId`)**: Links the two data stores

- **Trade-Offs**
   * MongoDB vs. Cosmos API: Used local MongoDB with Cosmos-compatible design for portability
   * CQRS for small app: Adds complexity but scales with future growth
   * MS Azure vs. AWS: I already have experience with Azure and would require more time to get up to speed with AWS.
   * EF Core vs. Dapper: I prefer Dapper for simplicity + performance but I decided to EFCore for ease of setup and I didn't have to write SQL scripts.
   
- **Security & Monitoring**
   * HTTPS enforced by default
   * Exception handling middleware (extendable)
   * Logging + monitoring hooks to be integrated via Application Insights
   
- **Cost Management**
   * SQL Server and MongoDB on Azure support autoscaling
   * Serverless Functions could replace endpoints if needed
   
 - **Future Enhancements**
    * JWT Auth with role-based policies
	* Event enrichment and indexing in MongoDB
	* Caching mechanism for performance improvements.
	* Event sourcing pattern or Kafka integration for event replay
	* Scheduled purge of expired logs using background services
	* Rate limiting and input schema validation with FluentValidation
	
```plaintext
[Client] -> [Minimal API] -> [Mediator (CQRS)] -> [SQL + Mongo Repositories]
                                       ↑
                                 Clean Separation of Concerns

