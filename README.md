
# Fluxo de Caixa Diário - Microserviços

## Introdução

Este repositório abriga uma solução de microserviços para gerenciar o fluxo de caixa diário, permitindo o registro de entradas e saídas financeiras. Projetado com ASP.NET Core e um banco de dados PostgreSQL, este sistema é uma demonstração robusta e escalável de uma aplicação de microserviços.

A escolha do Kubernetes deve-se a várias de suas características inerentes, como:

- **Gerenciamento Automatizado**: Kubernetes automatiza várias tarefas manuais envolvidas na implantação, gerenciamento e escalonamento de aplicações em contêineres, permitindo que as equipes se concentrem no desenvolvimento do produto em vez da infraestrutura.
- **Escalabilidade**: Ele suporta tanto a escalabilidade horizontal quanto a vertical, tornando mais fácil o ajuste de aplicações de acordo com as flutuações na demanda.
- **Recuperação de Falhas**: Kubernetes proporciona alta disponibilidade ao reiniciar automaticamente contêineres que falham, substituir e reescalonar contêineres quando os nós morrem, e matar contêineres que não respondem ao health check definido pelo usuário, garantindo que a aplicação não seja interrompida.
- **Service Discovery e Load Balancing**: Kubernetes pode expor um contêiner usando o nome DNS ou o próprio IP e pode equilibrar a carga de tráfego de forma inteligente, garantindo a distribuição da carga de trabalho.
- **Atualizações e Rollbacks Automáticos**: Facilita atualizações e rollbacks contínuos para a aplicação com estratégias como rolling updates, sem downtime para os usuários.

Graças a estas vantagens, Kubernetes é ideal para aplicações que requerem rápida iteração e escalabilidade eficiente, como é o caso deste projeto de fluxo de caixa diário.


## Premissas

### Controle de Saldo
Foi assumido que as contas não devem ficar negativas, reforçando a integridade financeira do sistema.

### Autenticação
Implementamos um serviço de autenticação utilizando ASP.NET Identity Core para segurança e gestão de acessos, apesar de não ser um requisito inicial.

### Dockerização
A solução está disponível como um Docker-compose para facilitar a execução e compreensão, ideal para ambientes de desenvolvimento e testes.

### Design e Arquitetura
O serviço de controle de contas foi desenvolvido com Domain-Driven Design (DDD) devido à sua complexidade. Todos os serviços são configurados com variáveis de ambiente para maior segurança.

## Ponto Importante

Testes unitários foram implementados na camada principal, garantindo a confiabilidade do código central.

## Padrões de Arquitetura e Recursos Utilizados

Detalhamos os padrões e recursos utilizados para projetar uma arquitetura que é ao mesmo tempo resiliente e escalável:

- **CORS**: Para permitir requisições de diferentes origens.
- **Bloqueio Pessimista**: Para gerenciar o acesso concorrente às contas.
- **DDD**: Usado no serviço de contas para modelar a complexidade do domínio.
- **Outbox Pattern**: Para manter a consistência das transações entre serviços.
- **Message Bus Service**: Para comunicação assíncrona entre os serviços.
- **Cache**: Para melhorar a resposta do serviço de relatórios.
- **Factory Pattern**: No serviço de contas, para criar operações de transação.
- **Singleton Pattern**: No Outbox Publisher para garantir uma única instância operacional.
- **Strategy Pattern**: No serviço de relatórios, permitindo a substituição do mecanismo de cache.

Kubernetes foi escolhido para facilitar a escalabilidade e resiliência dos serviços.

## Serviços

### 1. API Gateway - Roteamento e Segurança com Ocelot

O API Gateway é a porta de entrada para os microserviços, encarregado de rotear as chamadas de API para os serviços corretos e validar os tokens JWT fornecidos para autenticação. Escolhemos Ocelot por ser uma solução de mercado comprovada que oferece facilidade de integração, recursos robustos de roteamento e uma comunidade ativa de suporte.

#### Roteamento Inteligente
O API Gateway, utilizando Ocelot, gerencia o tráfego de entrada, direcionando as solicitações para os serviços apropriados com base nas rotas configuradas, otimizando assim a comunicação dentro da arquitetura de microserviços.

#### Segurança de Tokens JWT
A segurança é uma preocupação primordial, e o API Gateway desempenha um papel crucial na validação dos tokens JWT. Todas as chamadas de API, com exceção das solicitações de autenticação, devem ser acompanhadas de um token válido, assegurando que apenas usuários autenticados possam acessar os recursos.

### 2. Identity Control - Autenticação Centralizada e Gerenciamento de Identidade

Identity Control é o microserviço responsável pela autenticação dos usuários e pela validação dos tokens. Ele opera de forma independente com seu próprio banco de dados, isolando e protegendo as informações de identidade dos usuários.

#### Gestão de Autenticação
Este serviço utiliza ASP.NET Identity Core para registrar usuários e gerenciar o processo de login, emitindo tokens JWT que são essenciais para acessar os serviços protegidos dentro da arquitetura.

#### Isolamento de Dados de Segurança
Com um banco de dados dedicado para a gestão de identidades, garantimos que as credenciais dos usuários sejam armazenadas de forma segura e separada dos outros dados operacionais, aderindo às melhores práticas de segurança.

### 3. Accounts
Este serviço, implementado com Domain-Driven Design (DDD), é o coração do nosso sistema de fluxo de caixa. Ele gerencia não só as contas e seus saldos, mas também registra cada operação financeira. Aqui está o detalhamento das escolhas técnicas:

#### Domain-Driven Design (DDD)
Optamos por DDD devido à complexidade inerente ao gerenciamento de contas. O DDD nos ajuda a modelar o domínio financeiro de maneira clara e a manter a lógica de negócio organizada, permitindo que evoluamos o sistema com menos fricção e maior aderência às regras do negócio.

#### Bloqueio Pessimista
Dada a natureza concorrente do acesso às contas, um bloqueio pessimista foi implementado para prevenir condições de corrida. Este bloqueio garante que, quando um usuário estiver realizando uma operação em uma conta, outros acessos concorrentes sejam bloqueados até que a operação inicial seja concluída. Inicialmente, isso foi implementado diretamente no PostgreSQL, aproveitando suas capacidades de bloqueio de transação, mas o design permite a substituição por ferramentas especializadas no futuro, se necessário.

#### Padrão Outbox
Para assegurar a atomicidade das transações e a confiabilidade na comunicação entre serviços, adotamos o padrão Outbox. Quando uma transação é realizada, ela é primeiramente registrada em uma tabela 'Outbox' dentro da mesma transação de banco de dados que atualiza a conta. Isso garante que, mesmo que a comunicação com o serviço de mensageria falhe, a informação da operação permanece segura e pode ser retentada sem perda de dados.

#### Factory Pattern
Para a criação de transações e operações de conta, utilizamos o Factory Pattern. Isso nos permite encapsular a lógica de criação, fornecendo uma camada de abstração que facilita a manutenção e a extensibilidade do código.

Com estas escolhas, o serviço "Accounts" é robusto e confiável, capaz de gerenciar as operações financeiras de maneira segura e eficiente, mesmo em um ambiente com alta concorrência e demanda.

### 4. Outbox Publisher
O Outbox Publisher é essencial para a integridade e a confiabilidade do sistema de mensagens. Ele segue o padrão CQRS (Command Query Responsibility Segregation) para separar as operações de escrita das operações de leitura, otimizando o tráfego de dados e permitindo uma arquitetura mais escalável.

#### Execução Singleton no Kubernetes
Este serviço é projetado para executar como um singleton dentro do Kubernetes, garantindo que haja apenas uma instância responsável por processar as mensagens da fila Outbox, evitando duplicidades e potenciais conflitos.

#### Processamento de Mensagens
O Outbox Publisher se conecta ao serviço Accounts para buscar todas as mensagens pendentes na tabela Outbox. Ele processa cada mensagem sequencialmente, assegurando que as operações sejam enviadas ao Message Bus Service e, após a confirmação de entrega, as mensagens são removidas da fila Outbox, garantindo que cada operação seja processada uma única vez.

#### Message Bus Service
Ao utilizar um Message Bus Service, este serviço permite a comunicação assíncrona e a notificação imediata para os serviços consumidores, como o Reports, melhorando a eficiência e a velocidade da comunicação entre serviços.

### 5. Reports
O serviço Reports é projetado para atuar como o consumidor final no nosso fluxo de mensagens, recebendo dados consolidados e gerando insights valiosos a partir das transações processadas.

#### Banco de Dados Próprio
Para armazenar e analisar os dados, o Reports possui seu próprio banco de dados, isolando as operações de leitura e relatório do resto do sistema e permitindo um desempenho otimizado para consultas analíticas.

#### Consumo de Mensagens e Cache de IDs
Ao receber mensagens do Outbox Publisher, o serviço Reports registra apenas o identificador da mensagem em um cache, proporcionando uma maneira eficiente de validar se uma mensagem foi processada anteriormente, evitando assim o processamento redundante.

#### Flexibilidade com Strategy Pattern
A utilização do Strategy Pattern para o gerenciamento de cache oferece flexibilidade para futuras mudanças ou melhorias no mecanismo de cache, permitindo que o serviço se adapte rapidamente a novas tecnologias ou requisitos de negócio.

Com estas abordagens, garantimos que o serviço Reports seja capaz de oferecer relatórios atualizados e confiáveis, enquanto mantém a capacidade de escalar e evoluir conforme as necessidades do sistema.

## Instalação e Configuração

### Pré-requisitos
Para executar esta aplicação, você precisará ter o Docker e o Docker Compose instalados em sua máquina. Estes são necessários para criar e gerenciar os contêineres que executarão os microserviços.

### Executando com Docker Compose
Um arquivo `docker-compose.yml` está disponível na pasta principal do projeto e é responsável por definir e rodar os serviços do nosso aplicativo. Para iniciar a aplicação, siga os passos abaixo:

1. Abra o terminal ou prompt de comando.
2. Navegue até a pasta raiz do projeto onde o arquivo `docker-compose.yml` está localizado.
3. Execute o seguinte comando para iniciar todos os serviços definidos:
```sh
docker-compose up -d

## Uso da Ferramenta

Uma seção "Como Usar" inclui exemplos práticos para realizar operações comuns e orienta os usuários na interação com o sistema.

## Contribuições

Diretrizes para contribuir para o projeto estão disponíveis para qualquer desenvolvedor que deseje colaborar na melhoria contínua da ferramenta.
