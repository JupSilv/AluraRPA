# coe-templates

## Comandos para gerar o template:

> Precisa estar na raiz do projeto template (mesma pasta que contiver o .csproj e a pasta '/templates')

```
> dotnet pack -c Release
```

## Instalando/desistalando o template na maquina local:

> Precisa estar na raiz do projeto template (mesma pasta que contiver o .csproj e a pasta '/templates')

```
> dotnet new --install .
```

```
> dotnet new --uninstall .
```

## Criando um novo projeto/solucao com base no template:

```
> dotnet new coe -o "COE000.RPA.MeuProjetoCOE"
```

## Estrutura da solucao COE template padrão:

> COE000.RPA.MeuProjetoCOE neste caso servirá apenas para exemplificar abaixo:

```
[COE000.RPA.MeuProjetoCOE]
├── docs
├── mocks
├── queries
├── mocks
├── src
│   ├── COE000.RPA.MeuProjetoCOE.Application
│   ├── COE000.RPA.MeuProjetoCOE.Domain
│   ├── COE000.RPA.MeuProjetoCOE.Infrastructure
│   └── COE000.RPA.MeuProjetoCOE.Worker
├── tests
│   ├── COE000.RPA.MeuProjetoCOE.Application.UnitTests
│   └── COE000.RPA.MeuProjetoCOE.Domain.UnitTests
├── .gitignore
├── COE000.RPA.MeuProjetoCOE.sln
└── SetupServicoWindows.bat
```
