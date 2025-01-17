﻿using System.IO;
using Microsoft.OData.ModelBuilder;
using Smartstore.Core.Catalog.Categories;
using Smartstore.Core.Catalog.Discounts;
using Smartstore.Core.Common;
using Smartstore.Core.Content.Media;
using Smartstore.Core.Identity;
using Smartstore.Engine;
using Smartstore.Web.Api.Controllers.OData;

namespace Smartstore.Web.Api
{
    internal class DefaultODataModelProvider : ODataModelProviderBase
    {
        public override void Build(ODataModelBuilder builder, int version)
        {
            builder.EntitySet<Address>("Addresses");
            builder.EntitySet<Category>("Categories");
            builder.EntitySet<Discount>("Discounts");
            builder.EntitySet<Country>("Countries");
            builder.EntitySet<Currency>("Currencies");
            builder.EntitySet<CustomerRoleMapping>("CustomerRoleMappings");
            builder.EntitySet<CustomerRole>("CustomerRoles");
            builder.EntitySet<Customer>("Customers");

            builder.EntitySet<MediaFile>("MediaFiles");
            builder.EntitySet<StateProvince>("StateProvinces");
        }

        public override Stream GetXmlCommentsStream(IApplicationContext appContext)
            => GetModuleXmlCommentsStream(appContext, Module.SystemName);
    }
}
