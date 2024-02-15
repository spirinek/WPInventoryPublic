using System.Collections.Generic;
using System.Linq;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using WPInventory.Data;

namespace WPInventory.BL.Computers
{
    public class GetComputersSimpleModel : IRequestHandler<GetAllComputersSimpleModelRequest,
        MediatorResult<GetAllComputersSimpleModelResult>>,
        IRequestHandler<GetConcreteComputerRequest, MediatorResult<GetConcreteComputerResult>>
    {
        private readonly ComputerInfoContext _dbContext;
        private readonly ILogger<GetComputersSimpleModel> _logger;
        private readonly IMapper _mapper;

        public GetComputersSimpleModel(ComputerInfoContext dbContext, ILogger<GetComputersSimpleModel> logger, IMapper mapper)
        {
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<MediatorResult<GetAllComputersSimpleModelResult>> Handle(GetAllComputersSimpleModelRequest request, CancellationToken ct)
        {
            var resultData = new GetAllComputersSimpleModelResult();

            var computerQuery = _dbContext.Computers.Select(x => x);
            if (request.IncludeArchived == false)
            {
                computerQuery = computerQuery.Where(x => x.IsArchived == false);
            }

            if (request.DateType.HasValue)
            {
                if (request.From.HasValue)
                {
                    switch (request.DateType.Value)
                    {
                        case FilterDateType.Added:
                            computerQuery = computerQuery.Where(x => x.ScanDates.Added >= request.From.Value);
                            break;
                        case FilterDateType.Changed:
                            computerQuery = computerQuery.Where(x => x.ScanDates.Changed >= request.From.Value);
                            break;
                        case FilterDateType.LastSeen:
                            computerQuery = computerQuery.Where(x => x.ScanDates.LastSeen >= request.From.Value);
                            break;
                    }
                }
                if (request.To.HasValue)
                {
                    switch (request.DateType.Value)
                    {
                        case FilterDateType.Added:
                            computerQuery = computerQuery.Where(x => x.ScanDates.Added <= request.To.Value);
                            break;
                        case FilterDateType.Changed:
                            computerQuery = computerQuery.Where(x => x.ScanDates.Changed <= request.To.Value);
                            break;
                        case FilterDateType.LastSeen:
                            computerQuery = computerQuery.Where(x => x.ScanDates.LastSeen <= request.To.Value);
                            break;
                    }
                }
            }
            
            var computers = await computerQuery.Select(x => new ComputerSimpleModelDto()
            {
                Name = x.Name,
                Description = x.Description,
                LastSeen = x.ScanDates.LastSeen.Value,
                Changed = x.ScanDates.Changed,
                Added = x.ScanDates.Added,
                Guid = x.Guid
            }).ToListAsync(ct);

            resultData.Computers = computers;

            return MediatorResult<GetAllComputersSimpleModelResult>.Success(resultData);
        }

        public async Task<MediatorResult<GetConcreteComputerResult>> Handle(GetConcreteComputerRequest request, CancellationToken cancellationToken)
        {
            var computerStates = await _dbContext.Computers.Where(x => x.Guid == request.ComputerStateId)
                .Include(x => x.RAMs)
                .Include(x => x.MotherBoard)
                .Include(x => x.CPUs)
                .Include(x => x.NWAdapters)
                .Include(x => x.ScanDates)
                .Include(x => x.Monitors)
                .Include(x => x.PhisicalDisks)
                .Include(x => x.ScanDates)
                .Include(x => x.VideoCards)
                .OrderBy(x => x.IsArchived).ThenBy(x => x.ScanDates.LastSeen)
                .ToListAsync(cancellationToken);

            if (computerStates.Any() == false)
            {
                return MediatorResult<GetConcreteComputerResult>.Failed(new ServiceError(ErrorCode.NotFound, $"Computer {request.ComputerStateId} not found"));
            }

            var result = new GetConcreteComputerResult();
            result.AllStates = _mapper.Map<List<ConcreteComputerDto>>(computerStates);

            return MediatorResult<GetConcreteComputerResult>.Success(result);
        }
    }
}
