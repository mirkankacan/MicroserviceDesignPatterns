# Microservice Design Patterns

This repository contains a collection of microservice design patterns implemented using .NET 9. The goal is to demonstrate various architectural patterns and best practices for building scalable and maintainable microservices.

## Table of Contents

- [Introduction](#introduction)
- [Technologies](#technologies)
- [Patterns Implemented](#patterns-implemented)
- [Branches](#branches)
- [Getting Started](#getting-started)
- [License](#license)

## Introduction

Microservice Design Patterns is a project that showcases different design patterns used in microservice architecture. Each pattern is implemented as a separate service, demonstrating how to solve common challenges in microservice-based systems.

## Technologies

- .NET 9
- MassTransit
- RabbitMQ
- MS SQL
- Event Store
- Entity Framework Core
- Docker
  
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
- `EventSourcingPattern`:  Implementation of the Event Sourcing Pattern.

You can switch to a specific branch to explore the corresponding pattern implementation.

## Getting Started

To get a local copy up and running, follow these steps.

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


## License

Distributed under the MIT License. See `LICENSE` for more information.
