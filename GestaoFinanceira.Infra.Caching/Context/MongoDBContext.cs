using MongoDB.Driver;
using GestaoFinanceira.Domain.DTOs;
using GestaoFinanceira.Infra.Caching.Settings;

namespace GestaoFinanceira.Infra.Caching.Context
{
    public class MongoDBContext
    {
        private readonly MongoDBSettings mongoDBSettings;
        private IMongoDatabase mongoDataBase;

        public MongoDBContext(MongoDBSettings mongoDBSettings)
        {
            this.mongoDBSettings = mongoDBSettings;
            Initialize();
        }

        //método para inicializar uma conexão com o banco de dados..
        private void Initialize()
        {
            var settings = MongoClientSettings.FromUrl(new MongoUrl(mongoDBSettings.ConnectionString));

            if (mongoDBSettings.IsSSL)
            {
                settings.SslSettings = new SslSettings { EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12 };
            };

            var client = new MongoClient(settings);
            mongoDataBase = client.GetDatabase(mongoDBSettings.DataBaseName);
         }

        //Configurar as Collections que serão criadas no MongoDB
        public IMongoCollection<CategoriaDTO> Categorias { 
            get { return mongoDataBase.GetCollection<CategoriaDTO>("Categorias"); } 
        }

        public IMongoCollection<ContaDTO> Contas
        {
            get { return mongoDataBase.GetCollection<ContaDTO>("Contas"); }
        }

        public IMongoCollection<FormaPagamentoDTO> FormasPagamento
        {
            get { return mongoDataBase.GetCollection<FormaPagamentoDTO>("FormasPagamento"); }
        }

        public IMongoCollection<ItemMovimentacaoDTO> ItensMovimentacao
        {
            get { return mongoDataBase.GetCollection<ItemMovimentacaoDTO>("ItensMovimentacao"); }
        }

        public IMongoCollection<MovimentacaoPrevistaDTO> MovimentacoesPrevistas
        {
            get { return mongoDataBase.GetCollection<MovimentacaoPrevistaDTO>("MovimentacoesPrevistas"); }
        }

        public IMongoCollection<MovimentacaoRealizadaDTO> MovimentacoesRealizadas
        {
            get { return mongoDataBase.GetCollection<MovimentacaoRealizadaDTO>("MovimentacoesRealizadas"); }
        }

        public IMongoCollection<SaldoDiarioDTO> SaldosDiario
        {
            get { return mongoDataBase.GetCollection<SaldoDiarioDTO>("SaldosDiario"); }
        }

    }
}