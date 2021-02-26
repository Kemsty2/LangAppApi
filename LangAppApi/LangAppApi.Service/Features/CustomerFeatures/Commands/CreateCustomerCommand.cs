using LangAppApi.Domain.Entities;
using LangAppApi.Persistence;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LangAppApi.Service.Features.CustomerFeatures.Commands
{
    public class CreateCustomerCommand : IRequest<Guid>
    {
        public string CustomerName { get; set; }
        public string ContactName { get; set; }

        public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, Guid>
        {
            private readonly IApplicationDbContext _context;

            public CreateCustomerCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<Guid> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
            {
                var customer = new Customer { CustomerName = request.CustomerName, ContactName = request.ContactName };

                await _context.Customers.AddAsync(customer, cancellationToken);
                await _context.SaveChangesAsync();
                return customer.Id;
            }
        }
    }
}