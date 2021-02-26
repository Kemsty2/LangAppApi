using LangAppApi.Persistence;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LangAppApi.Service.Features.CustomerFeatures.Commands
{
    public class UpdateCustomerCommand : IRequest<Guid>
    {
        public Guid Id { get; set; }
        public string CustomerName { get; set; }
        public string ContactName { get; set; }

        public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, Guid>
        {
            private readonly IApplicationDbContext _context;

            public UpdateCustomerCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<Guid> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
            {
                var cust = _context.Customers.Where(a => a.Id == request.Id).FirstOrDefault();

                if (cust == null)
                {
                    return default;
                }

                cust.CustomerName = request.CustomerName;
                cust.ContactName = request.ContactName;
                _context.Customers.Update(cust);
                await _context.SaveChangesAsync();
                return cust.Id;
            }
        }
    }
}