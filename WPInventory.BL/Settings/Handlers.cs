using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WPInventory.Data;
using WPInventory.Data.Models.Entities;

namespace WPInventory.BL.Settings
{
    public class SettingsRequestHandlers : IRequestHandler<UpdateScopeRequest, MediatorResult>,
        IRequestHandler<CreateScopeRequest, MediatorResult>,
        IRequestHandler<DeleteScopeRequest, MediatorResult>,
        IRequestHandler<GetScopesRequest, MediatorResult<GetScopesResult>>
    {
        private readonly ComputerInfoContext _dbContext;
        private readonly ILogger<SettingsRequestHandlers> _logger;
        private readonly IMapper _mapper;

        public SettingsRequestHandlers(ComputerInfoContext dbContext, ILogger<SettingsRequestHandlers> logger, IMapper mapper)
        {
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<MediatorResult> Handle(UpdateScopeRequest request, CancellationToken cancellationToken)
        {
            var scope = await _dbContext.AdScopes.Where(x => x.Id == request.Id).FirstOrDefaultAsync(cancellationToken);

            if (scope == null)
            {
                return MediatorResult.Failed(new ServiceError(ErrorCode.NotFound,$"SearchScope id:{request.Id} not found"));
            }
            scope.ScopePath = request.ScopePath;
            scope.IsEnabled = request.IsEnabled;

            _dbContext.Update(scope);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return MediatorResult.Success();
        }

        public async Task<MediatorResult> Handle(CreateScopeRequest request, CancellationToken cancellationToken)
        {
            var scope = new ADScope
            {
                IsEnabled = request.IsEnabled,
                ScopePath = request.ScopePath
            };

            await _dbContext.AddAsync(scope, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return MediatorResult.Success();
        }

        public async Task<MediatorResult> Handle(DeleteScopeRequest request, CancellationToken cancellationToken)
        {
            var scope = await _dbContext.AdScopes.Where(x => x.Id == request.Id).FirstOrDefaultAsync(cancellationToken);
            if (scope == null)
            {
                return MediatorResult.Failed(new ServiceError(ErrorCode.NotFound, $"SearchScope id:{request.Id} not found"));
            }

            _dbContext.Remove(scope);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return MediatorResult.Success();
        }

        public async Task<MediatorResult<GetScopesResult>> Handle(GetScopesRequest request, CancellationToken cancellationToken)
        {
            var scopes = await _dbContext.AdScopes.ToListAsync(cancellationToken);

            var result = new GetScopesResult()
            {
                Scopes = _mapper.Map<List<AdScopeDto>>(scopes)
            };

            return MediatorResult<GetScopesResult>.Success(result);
        }
    }
}
