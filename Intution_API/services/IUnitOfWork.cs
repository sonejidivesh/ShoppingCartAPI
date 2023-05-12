namespace Intution_API.services
{
    public interface IUnitOfWork
    {
        IProductRepository Products { get;  }

        IOrders Orders { get; }

        IProductOrder ProductOrder { get; }

        ICart Carts { get; }


        Task CompleteAsync();

    }
}
