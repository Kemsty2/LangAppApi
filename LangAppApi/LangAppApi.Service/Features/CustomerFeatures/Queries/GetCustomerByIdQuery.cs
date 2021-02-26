using LangAppApi.Domain.Entities;
using LangAppApi.Persistence;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LangAppApi.Service.Features.CustomerFeatures.Queries
{
    public class GetCustomerByIdQuery : IRequest<Customer>
    {
        public Guid Id { get; set; }

        public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, Customer>
        {
            private readonly IApplicationDbContext _context;

            public GetCustomerByIdQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<Customer> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
            {
                var customer = _context.Customers.FirstOrDefault(a => a.Id == request.Id);
                return customer;
            }
        }
    }
}