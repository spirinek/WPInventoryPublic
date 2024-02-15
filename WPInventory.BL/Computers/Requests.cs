using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WPInventory.BL.Computers
{
    

    public class GetAllComputersSimpleModelRequest : IRequest<MediatorResult<GetAllComputersSimpleModelResult>>
    {
        public bool IncludeArchived { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public FilterDateType? DateType { get; set; }
        [Range(10, 50)]
        public int PageSize { get; set; }
        [Range(1, int.MaxValue)]
        public int Page { get; set; }
        public string SortBy { get; set; }
        public string OrderBy { get; set; }
    }

    public class GetConcreteComputerRequest : IRequest<MediatorResult<GetConcreteComputerResult>>
    {
        public Guid ComputerStateId { get; set; }
    }
    public enum FilterDateType
    {
        Added = 1,
        Changed,
        LastSeen
    }

}
