# Microservice Design Patterns

This repository contains a collection of microservice design patterns implemented using .NET 9. The goal is to demonstrate various architectural patterns and best practices for building scalable and maintainable microservices.

## Table of Contents

- [Introduction](#introduction)
- [Technologies](#technologies)
- [Patterns Implemented](#patterns-implemented)
- [Branches](#branches)
- [Getting Started](#getting-started)
- [Usage](#usage)
- [License](#license)

## Introduction

Microservice Design Patterns is a project that showcases different design patterns used in microservice architecture. Each pattern is implemented as a separate service, demonstrating how to solve common challenges in microservice-based systems.

## Technologies

- .NET 9
- MassTransit
- RabbitMQ
- Entity Framework Core
- Docker
- Swagger

## Patterns Implemented

- Saga Pattern
- CQRS (Command Query Responsibility Segregation)
- Event Sourcing
- Circuit Breaker

## Branches

This repository uses branches to separate different design patterns and implementations. Below are the main branches and their purposes:

- `master`: The main branch containing the core setup and common utilities.
- `SagaChoreographyPattern`: Implementation of the Choreography based Saga Pattern.
- `SagaOrchestrationPattern`:  Implementation of the Orchestration based Saga Pattern.

You can switch to a specific branch to explore the corresponding pattern implementation.

## Getting Started

To get a local copy up and running, follow these steps.

### Prerequisites

- .NET 9 SDK
- Docker
- RabbitMQ
- SQL Server

### Installation

1. Clone the repository:
```
git clone https://github.com/mirkankacan/MicroserviceDesignPatterns.git
```
2. Navigate to the project directory:
```
cd MicroserviceDesignPatterns
```
3. Checkout the branch you are interested in:
```
git checkout <branch-name>
```
4. Build the Docker containers:
```
docker run -d --hostname rabbitmq --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3-management
docker-compose up --build
```

## Usage

Each microservice can be run independently. You can use tools like Postman or Swagger to interact with the APIs.

### Running the Services

1. Start the services using Docker Compose:
```
docker-compose up
```
2. Access the services via the API Gateway at `http://localhost:5000`.

### Swagger

Each service has its own Swagger documentation available at `http://localhost:<port>/swagger`.

## License

Distributed under the MIT License. See `LICENSE` for more information.
