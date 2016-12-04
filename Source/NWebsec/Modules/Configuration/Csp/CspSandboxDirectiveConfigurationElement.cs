// Copyright (c) Andr� N. Klingsheim. See License.txt in the project root for license information.

using System.Configuration;
using NWebsec.Core.HttpHeaders.Configuration;

namespace NWebsec.Modules.Configuration.Csp
{
    public class CspSandboxDirectiveConfigurationElement : ConfigurationElement, ICspSandboxDirectiveConfiguration
    {

        [ConfigurationProperty("enabled", IsRequired = true, DefaultValue = false)]
        public bool Enabled
        {
            get { return (bool)this["enabled"]; }
            set { this["enabled"] = value; }
        }

        [ConfigurationProperty("allow-forms", IsRequired = false, DefaultValue = false)]
        public bool AllowForms
        {
            get { return (bool)this["allow-forms"]; }
            set { this["allow-forms"] = value; }
        }

        [ConfigurationProperty("allow-modals", IsRequired = false, DefaultValue = false)]
        public bool AllowModals
        {
            get { return (bool)this["allow-modals"]; }
            set { this["allow-modals"] = value; }
        }

        [ConfigurationProperty("allow-orientation-lock", IsRequired = false, DefaultValue = false)]
        public bool AllowOrientationLock
        {
            get { return (bool)this["allow-orientation-lock"]; }
            set { this["allow-orientation-lock"] = value; }
        }

        [ConfigurationProperty("allow-pointer-lock", IsRequired = false, DefaultValue = false)]
        public bool AllowPointerLock
        {
            get { return (bool)this["allow-pointer-lock"]; }
            set { this["allow-pointer-lock"] = value; }
        }

        [ConfigurationProperty("allow-popups", IsRequired = false, DefaultValue = false)]
        public bool AllowPopups
        {
            get { return (bool)this["allow-popups"]; }
            set { this["allow-popups"] = value; }
        }

        [ConfigurationProperty("allow-popups-to-escape-sandbox", IsRequired = false, DefaultValue = false)]
        public bool AllowPopupsToEscapeSandbox
        {
            get { return (bool)this["allow-popups-to-escape-sandbox"]; }
            set { this["allow-popups-to-escape-sandbox"] = value; }
        }

        [ConfigurationProperty("allow-presentation", IsRequired = false, DefaultValue = false)]
        public bool AllowPresentation
        {
            get { return (bool)this["allow-presentation"]; }
            set { this["allow-presentation"] = value; }
        }

        [ConfigurationProperty("allow-same-origin", IsRequired = false, DefaultValue = false)]
        public bool AllowSameOrigin
        {
            get { return (bool)this["allow-same-origin"]; }
            set { this["allow-same-origin"] = value; }
        }

        [ConfigurationProperty("allow-scripts", IsRequired = false, DefaultValue = false)]
        public bool AllowScripts
        {
            get { return (bool)this["allow-scripts"]; }
            set { this["allow-scripts"] = value; }
        }

        [ConfigurationProperty("allow-top-navigation", IsRequired = false, DefaultValue = false)]
        public bool AllowTopNavigation
        {
            get { return (bool)this["allow-top-navigation"]; }
            set { this["allow-top-navigation"] = value; }
        }
    }
}