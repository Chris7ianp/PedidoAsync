# 🚀 PedidoAsync

> **Processamento assíncrono de pedidos com .NET, MongoDB e RabbitMQ**

O **PedidoAsync** é uma aplicação backend desenvolvida em **C#/.NET** para estudar e demonstrar na prática conceitos de **Clean Architecture, persistência NoSQL, mensageria e processamento assíncrono**.

A aplicação simula o fluxo de criação de pedidos, onde a API persiste o pedido no **MongoDB** e publica um evento no **RabbitMQ** para processamento posterior por um **Worker Service**.

---

## 🧠 O que estou praticando neste projeto?

Este projeto foi desenvolvido com foco em conceitos que fazem parte do dia a dia de aplicações backend modernas:

* 🏗️ Clean Architecture
* 🔌 APIs REST
* 🍃 MongoDB
* 🐰 RabbitMQ
* ⚙️ Worker Service
* 🐳 Docker
* 📦 Docker Compose
* 🔄 Processamento assíncrono
* 📨 Event-driven architecture
* 💉 Dependency Injection
* 🧩 Separação de responsabilidades
* 🧪 Testes automatizados *(roadmap)*

---

## 🏗️ Arquitetura

```text
                    ┌─────────────────┐
                    │     Cliente     │
                    └────────┬────────┘
                             │
                             ▼
                    ┌─────────────────┐
                    │   PedidoAsync   │
                    │       API       │
                    └────────┬────────┘
                             │
                             ▼
                    ┌─────────────────┐
                    │   Application   │
                    │  PedidoService  │
                    └───────┬─┬───────┘
                            │ │
                ┌───────────┘ └────────────┐
                ▼                          ▼
        ┌──────────────┐          ┌────────────────┐
        │   MongoDB    │          │    RabbitMQ    │
        │              │          │                │
        │   Pedidos    │          │ pedidos.exchange│
        └──────────────┘          └───────┬────────┘
                                          │
                                          ▼
                                  ┌────────────────┐
                                  │ pedidos.criados│
                                  └───────┬────────┘
                                          │
                                          ▼
                                  ┌────────────────┐
                                  │     Worker     │
                                  │  Processamento  │
                                  └────────────────┘
```

### 📁 Estrutura da solução

```text
PedidoAsync
│
├── 📂 PedidoAsync.API
│   └── Controllers
│
├── 📂 PedidoAsync.Application
│   ├── DTOs
│   ├── Interfaces
│   └── Services
│
├── 📂 PedidoAsync.Domain
│   ├── Entities
│   ├── Enums
│   └── Events
│
├── 📂 PedidoAsync.Infrastructure
│   ├── Data
│   ├── Extensions
│   ├── Messaging
│   └── Repositories
│
├── 📂 PedidoAsync.Worker
│
└── 🐳 docker-compose.yml
```

---

## 🔄 Fluxo de criação do pedido

Quando um novo pedido é criado:

```text
POST /api/Pedidos
        │
        ▼
    API recebe
        │
        ▼
 PedidoService
        │
        ├──────────────► MongoDB
        │                  │
        │                  ▼
        │             Pedido salvo
        │
        ▼
PedidoCriadoEvent
        │
        ▼
    RabbitMQ
        │
        ▼
 pedidos.criados
        │
        ▼
     Worker
        │
        ▼
Pedido Processado
```

### Evento publicado

```json
{
  "pedidoId": "7ccc1417-9b6d-4b88-af4d-96acc04436de",
  "cliente": "Cliente Teste",
  "valor": 99.90,
  "dataCriacao": "2026-09-22T19:59:12.692717Z"
}
```

---

## 🐰 Mensageria

O RabbitMQ está configurado utilizando:

| Componente  | Nome               |
| ----------- | ------------------ |
| Exchange    | `pedidos.exchange` |
| Tipo        | `direct`           |
| Routing Key | `pedido.criado`    |
| Queue       | `pedidos.criados`  |

A ideia é desacoplar a criação do pedido do seu processamento.

Isso permite que a API publique o evento e o processamento seja realizado de forma assíncrona pelo Worker.

---

## 🍃 Persistência

Os pedidos são armazenados no **MongoDB**.

### Banco

```text
Database: PedidoAsync
Collection: Pedidos
```

### Entidade

```text
Pedido
│
├── Id
├── Cliente
├── Valor
├── Status
├── DataCriacao
└── DataProcessamento
```

### Status

```text
🟡 Pendente
🟢 Processado
```

---

## 🐳 Infraestrutura

MongoDB e RabbitMQ são executados utilizando **Docker Compose**.

```bash
docker compose up -d
```

Verificar os containers:

```bash
docker ps
```

### Serviços

| Serviço                |   Porta |
| ---------------------- | ------: |
| 🍃 MongoDB             | `27017` |
| 🐰 RabbitMQ            |  `5672` |
| 🐰 RabbitMQ Management | `15672` |

RabbitMQ Management:

```text
http://localhost:15672
```

Credenciais padrão para ambiente local:

```text
guest / guest
```

---

## 🌐 API

### Criar pedido

```http
POST /api/Pedidos
```

```json
{
  "cliente": "Christian",
  "valor": 150.90
}
```

### Listar pedidos

```http
GET /api/Pedidos
```

### Buscar pedido

```http
GET /api/Pedidos/{id}
```

---

## 🧪 Swagger

A API possui documentação interativa através do Swagger.

```text
https://localhost:7075/swagger
```

> A porta pode variar de acordo com a configuração do ambiente.

---

## 🛠️ Tecnologias

<div align="center">

![C#](https://img.shields.io/badge/C%23-512BD4?style=for-the-badge\&logo=csharp\&logoColor=white)
![.NET](https://img.shields.io/badge/.NET-512BD4?style=for-the-badge\&logo=dotnet\&logoColor=white)
![MongoDB](https://img.shields.io/badge/MongoDB-47A248?style=for-the-badge\&logo=mongodb\&logoColor=white)
![RabbitMQ](https://img.shields.io/badge/RabbitMQ-FF6600?style=for-the-badge\&logo=rabbitmq\&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-2496ED?style=for-the-badge\&logo=docker\&logoColor=white)

</div>

---

## 📚 Roadmap

### ✅ Implementado

* [x] Estrutura inicial da solução
* [x] Clean Architecture
* [x] Entidade `Pedido`
* [x] API REST
* [x] Repository Pattern
* [x] MongoDB
* [x] Docker Compose
* [x] RabbitMQ
* [x] `PedidoCriadoEvent`
* [x] Publisher RabbitMQ
* [x] Exchange `pedidos.exchange`
* [x] Queue `pedidos.criados`
* [x] Publicação de eventos

### 🚧 Próximos passos

* [ ] Worker Service
* [ ] Consumer RabbitMQ
* [ ] Processamento assíncrono
* [ ] ACK / NACK
* [ ] Retry de mensagens
* [ ] Dead Letter Queue (DLQ)
* [ ] Testes unitários
* [ ] Testes de integração
* [ ] Containerização da API
* [ ] Containerização do Worker
* [ ] CI/CD

---

## 🎯 Objetivo

Mais do que simplesmente criar uma API CRUD, o objetivo do **PedidoAsync** é construir uma aplicação que represente um cenário mais próximo de sistemas backend reais.

A evolução do projeto busca explorar:

```text
API
 ↓
Domain
 ↓
Application
 ↓
Infrastructure
 ↓
MongoDB
 ↓
RabbitMQ
 ↓
Worker
 ↓
Processamento Assíncrono
```

---

## 👨‍💻 Desenvolvedor

**Christian Paulo**

💻 Desenvolvedor Backend .NET

🔗 **GitHub:**
https://github.com/Chris7ianp
