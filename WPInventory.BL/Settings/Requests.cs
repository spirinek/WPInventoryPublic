using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace WPInventory.BL.Settings
{
    public class CreateScopeRequest : IRequest<MediatorResult>
    {
        public string ScopePath { get; set; }
        public bool IsEnabled { get; set; }
    }

    public class GetScopesRequest : IRequest<MediatorResult<GetScopesResult>>
    {
    }

    public class UpdateScopeRequest : IRequest<MediatorResult>
    {
        public int Id { get; set; }
        public string ScopePath { get; set; }
        public bool IsEnabled { get; set; }
    }

    public class DeleteScopeRequest : IRequest<MediatorResult>
    {
        public int Id { get; set; }
    }
}
