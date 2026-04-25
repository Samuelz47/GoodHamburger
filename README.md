# 🍔 Good Hamburger - Desafio Técnico

![.NET](https://img.shields.io/badge/.NET-8.0-blue)
![Blazor](https://img.shields.io/badge/Blazor-WebAssembly-purple)
![Docker](https://img.shields.io/badge/Docker-Enabled-blue)
![SQLite](https://img.shields.io/badge/SQLite-Database-lightgrey)

Bem-vindo ao repositório do desafio técnico da **Good Hamburger**! Este projeto consiste em um sistema completo de gestão de pedidos de lanchonete, incluindo uma **API REST** para backend e um **Frontend em Blazor WebAssembly**, totalmente orquestrados via **Docker Compose**.

---

## 🚀 Como Executar o Projeto

A maneira mais simples de rodar todo o ecossistema (Backend, Frontend e Banco de Dados) é utilizando o **Docker Compose**.

### Pré-requisitos
- Docker e Docker Compose instalados.

### Passos
1. Clone o repositório em sua máquina:
   ```bash
   git clone https://github.com/seu-usuario/goodhamburger.git
   cd goodhamburger
   ```

2. Na raiz do projeto, execute o comando:
   ```bash
   docker-compose up --build -d
   ```

3. Acesse os serviços:
   - 🌐 **Frontend (Blazor):** [http://localhost:8081](http://localhost:8081)
   - 🔌 **API REST (Swagger):** [http://localhost:8080/swagger](http://localhost:8080/swagger)

> **Nota:** O banco de dados SQLite será criado e populado automaticamente pela API no momento da inicialização (via *Auto-Migration* no `Program.cs`), sem necessidade de rodar scripts manuais.

---

## 🧪 Rodando os Testes Automatizados

O projeto conta com uma robusta suíte de testes unitários focada nas **regras de negócio de descontos**, utilizando `xUnit`, `Moq` e `FluentAssertions`.

Na raiz da solução (`GoodHamburger.sln`), execute:
```bash
dotnet test GoodHamburger.Tests/GoodHamburger.Tests.csproj
```

---

## 📐 Decisões de Arquitetura

O backend foi estruturado baseado nos princípios da **Clean Architecture** (ou arquitetura em camadas), dividida em:

- **GoodHamburger.API:** Ponto de entrada (Controllers, Configuração de Injeção de dependência e middlewares de exceção global).
- **GoodHamburger.Application:** Casos de uso da aplicação, DTOs, interfaces de repositórios e serviços de lógica (ex: `PedidoService`).
- **GoodHamburger.Domain:** Núcleo do sistema. Contém as Entidades (`Pedido`, `Produto`) e Enums (fortemente tipados). 
- **GoodHamburger.Infrastructure:** Detalhes de implementação externa (EF Core, `AppDbContext`, repositórios e implementações do padrão `Unit Of Work`).

### Destaques Técnicos implementados:
1. **Padrão Unit of Work / Repository:** Garante a consistência nas transações de gravação do Entity Framework.
2. **Global Exception Handler:** Para remover blocos de `try-catch` lixosos dos services e controllers. Lança exceções semânticas como `ArgumentException` que são interceptadas e convertidas em um belo JSON HTTP 400 (Bad Request).
3. **Validação de Regras de Negócio Puras:** A restrição de **um item por categoria** é feita de forma nativa e extremamente otimizada no `PedidoService.cs`, garantindo consistência com `O(1)` usando estruturas como `HashSet`.
4. **Padrão DTO (Data Transfer Object):** Desacoplamento entre os modelos do banco e as respostas expostas para o frontend na API.

---

## ⚠️ O Que Deixei de Fora (Trade-offs)

Dado o escopo do desafio técnico, algumas decisões pragmáticas foram tomadas:

1. **Modelo de Domínio Anêmico vs Rico:** 
   O cálculo dos totais e validações hoje residem em Domain Services (`PedidoService.cs`). Em um cenário complexo de *Domain-Driven Design (DDD)* real, a entidade `Pedido` seria rica, expondo comportamentos como `pedido.AdicionarProduto(produto)` e protegendo suas propriedades, e as regras de combo seriam modeladas no padrão `Strategy`. Mantive um modelo mais simples e direto devido ao tamanho do requerimento.

2. **N+1 Optimization:** 
   Especialmente ao calcular o pedido, fiz buscas iterativas de produtos no método `CalcularPedidosAsync`. Como a regra impõe limite de 3 categorias, o custo de N+1 é microscópico `(O(3))`. Se permitisse milhares de itens por pedido, eu inverteria a carga passando um array para um método `GetProdutosByIds(IEnumerable<int>)` reduzindo a carga do DB para uma única query.

---
🚀 *Obrigado pela oportunidade de realizar este desafio!*

