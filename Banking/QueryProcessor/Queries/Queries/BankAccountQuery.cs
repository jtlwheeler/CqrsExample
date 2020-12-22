namespace Banking.QueryProcessor.Queries.Queries
{
    public class BankAccountQuery : IQuery
    {
        public string Id { get; private set; }
        
        public BankAccountQuery(string id)
        {
            Id = id;
        }
    }
}
