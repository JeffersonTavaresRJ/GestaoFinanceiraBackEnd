cd Estudos\NetCore\GestaoFinanceira\Fontes\BackEnd

Passos para dockerizar a api:
1) Criar o arquivo "dockerfile" (sem a extensão) no diretório raiz da aplicação, onde está a solution;

2) Para criar SOMENTE A IMAGEM no docker, digite a linha de comando abaixo, usando o cmd dentro do mesmo diretório:  
   O "." (ponto final) indica que o contexto de build é a pasta atual (onde ele buscará todos os 8 projetos (.csproj)).
   "docker build -t gestao-financeira-api ."
   
3) Para saber se deu certo a criação da imagem, digite a linha de comando abaixo:
   "docker ps"
   
4) Criar o arquivo com o nome "docker-compose.yml" no mesmo diretório;

5) Para CRIAR A IMAGEM E SUBIR A APLICAÇÃO, use o comando: 
Comando para criar o container:
"docker-compose down" //remove o container..
"docker-compose up --build -d" //cria novamente..

   up: Sobe todos os serviços do arquivo.
   --build: Força o Docker a reconstruir a imagem da sua API (importante sempre que você mudar algo no código ou no Dockerfile).
   -d: Roda em "segundo plano" (seu terminal fica livre).
