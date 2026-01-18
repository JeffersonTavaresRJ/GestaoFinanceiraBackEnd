FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

# Dentro do container, criamos uma pasta chamada 'app' para organizar
WORKDIR /app

# Adicione esta linha abaixo para limpar o lixo de cache
RUN dotnet nuget locals all --clear

# 1. Copia todos os arquivos .sln e .csproj da raiz e subpastas
# Isso mantém a estrutura de pastas para que as referências funcionem

COPY ["GestaoFinanceira.sln", "./"]
COPY ["GestaoFinanceira.Service.Api/GestaoFinanceira.Service.Api.csproj", "GestaoFinanceira.Service.Api/"]
COPY ["GestaoFinanceira.Application/GestaoFinanceira.Application.csproj", "GestaoFinanceira.Application/"]
COPY ["GestaoFinanceira.Domain/GestaoFinanceira.Domain.csproj", "GestaoFinanceira.Domain/"]
COPY ["GestaoFinanceira.Infra.Caching/GestaoFinanceira.Infra.Caching.csproj", "GestaoFinanceira.Infra.Caching/"]
COPY ["GestaoFinanceira.Infra.CrossCutting/GestaoFinanceira.Infra.CrossCutting.csproj", "GestaoFinanceira.Infra.CrossCutting/"]
COPY ["GestaoFinanceira.Infra.Data/GestaoFinanceira.Infra.Data.csproj", "GestaoFinanceira.Infra.Data/"]
COPY ["GestaoFinanceira.Infra.Ioc/GestaoFinanceira.Infra.Ioc.csproj", "GestaoFinanceira.Infra.Ioc/"]
COPY ["GestaoFinanceira.Infra.Reports.Excel/GestaoFinanceira.Infra.Reports.Excel.csproj", "GestaoFinanceira.Infra.Reports.Excel/"]

# 2. Restaura as dependências baseando-se na Solution
# O Docker salvará o resultado desta camada. 
# Se você mudar o código, ele não baixará os pacotes de novo.

RUN dotnet restore "GestaoFinanceira.sln"

# 3. Agora copia todo o resto dos arquivos (.cs, appsettings, etc)

COPY . .

# 4. Publica o projeto principal

WORKDIR "/app/GestaoFinanceira.Service.Api"
RUN dotnet publish "GestaoFinanceira.Service.Api.csproj" -c Release -o /app/publish

# --- Estágio de Runtime ---
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "GestaoFinanceira.Service.Api.dll"]

#Mesmo na raiz, o Dockerfile é como uma "receita de bolo". Você precisa dizer:
#"Pegue os ingredientes (arquivos .csproj) da mesa (sua pasta) e coloque na tigela (container)".
#"Bata os ingredientes (dotnet restore)".
#"Coloque o resto (código .cs) e asse (dotnet publish)".
#Se você não der o COPY, a tigela continua vazia e o comando dotnet publish vai dizer que não encontrou nenhum projeto.