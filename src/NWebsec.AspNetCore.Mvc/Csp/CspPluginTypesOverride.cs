﻿// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

namespace NWebsec.AspNetCore.Mvc.Csp
{
    public class CspPluginTypesOverride
    {
        public bool Enabled { get; set; }
        public bool InheritMediaTypes { get; set; }
        public string[] MediaTypes { get; set; }
    }
}
