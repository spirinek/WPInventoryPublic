using System;
using System.Collections.Generic;
using System.Text;

namespace WPInventory.BL.Settings
{
    public class GetScopesResult
    {
        public List<AdScopeDto> Scopes { get; set; }
    }

    public class AdScopeDto
    {
        public int Id { get; set; }
        public string ScopePath { get; set; }
        public bool IsEnabled { get; set; }
    }
}
