﻿namespace Smartstore.Web.Api.Models
{
    public class WebApiState
    {
        public bool IsActive { get; init; }
        public string ModuleVersion { get; init; }
        public int MaxTop { get; init; }
        public int MaxExpansionDepth { get; init; }

        public string Version => $"1 {ModuleVersion}";
    }
}