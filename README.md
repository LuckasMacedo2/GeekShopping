# GeekShopping

Repositório com códigos referente sao projeto GeekShopping que busca empregar a arquitetura de microsserviços em uma loja virtual.

## Tecnologias utilizadas
- Identity service: Prover a autenticação e um serviço de login. Instalação:
1. dotnet new --install Duende.IdentityServer.Templates
2. dotnet new isui
- RabbitMQ: Realiza a comunicação assíncrona entre os microsserviços a partir de mensagens. Instalação:
1. Inicilizar o docker service
2. docker run -d --hostname my-rabbit --name some-rabbit -p 15672:15672 -p 5672:5672 rabbitmq:3-management
3. Login: guest
4. Senha: guest
- Ocelot: API Gateway, centraliza as requisições para os microsserviços da aplicação. Deve-se realizar a configuração do redirectionamento no appsettings.json do projeto do ocelot para cada rota da aplicação. Exemplo de configuração:
```
{
      "DownstreamPathTemplate": "",
      "DownstreamScheme": "",
      "DownstreamHostAndPorts": [
        {
          "Host": "",
          "Port": 
        }
      ],
      "UpstreamPathTemplate": "",
      "UpstreamHttpMethod": [ "" ],
      "AuthenticationOptions": {
        "AuthenticationProviderKey": "",
        "AllowedScopes": [ "" ]
      }
}
```
## Conceitos utilizados
- Microsserviços: Estilo arquitetural para a construção de software. O software é construído por meio de pequenos serviços independentes que se comunicam por meio de um protocolo, como, o HTTP.
- Repository: Abstrai o acesso a bancos de dados, arquivos em disco, outros serviços, etc
