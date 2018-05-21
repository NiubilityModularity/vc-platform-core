using System.Collections.Generic;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Domain.Commerce.Model
{
    public interface ISeoSupport : IEntity
    {
        string SeoObjectType { get; }
        IList<SeoInfo> SeoInfos { get; set; }
    }
}
