# SOLUTION.md

## Key Design Decisions

### Architecture
- Layered: API/Controllers, Services, Repositories, Domain
- Dependency Injection for testability
- BackgroundService for generation and consumption

### Concurrency
- ConcurrentQueue for in-memory store
- Lock for spill logic where needed

### Spill Logic
- Age-based (5s) + capacity-based spill

### Complex Analysis
- Simulated Fourier Transform: simple harmonic analysis using sine wave approximation for demonstration.

### Scalability
- Ready for message queues (RabbitMQ/Kafka), multiple consumers, sharding.

### Trade-offs
- In-memory + DB for simplicity vs pure queue.

## Tests
- Unit tests for FIFO and analysis.

Prompts used: [to be filled]