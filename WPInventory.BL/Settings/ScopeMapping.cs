using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using WPInventory.Data.Models.Entities;

namespace WPInventory.BL.Settings
{
    public class ScopeMapping : Profile
    {
        public ScopeMapping()
        {
            CreateMap<ADScope, AdScopeDto>();
        }
    }
}
