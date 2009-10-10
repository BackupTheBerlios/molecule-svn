using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics.CodeAnalysis;
using System.Web.Mvc;

namespace Molecule.Web.Mvc
{

        public class TagBuilder : System.Web.Mvc.TagBuilder, IDisposable
        {
            HttpResponseBase _httpResponse;
            bool _disposed = false;

            public TagBuilder(string tagName)
                : this(null, tagName)
            {

            }
            public TagBuilder(HttpResponseBase httpResponse, string tagName)
                : base(tagName)
            {
                _httpResponse = httpResponse;
            }

            [SuppressMessage("Microsoft.Security", "CA2123:OverrideLinkDemandsShouldBeIdenticalToBase")]
            public void Dispose()
            {
                Dispose(true /* disposing */);
                GC.SuppressFinalize(this);
            }

            protected virtual void Dispose(bool disposing)
            {
                if (!_disposed)
                {
                    _disposed = true;
                    if(_httpResponse != null)
                        _httpResponse.Write(base.ToString(TagRenderMode.EndTag));
                }
            }

            public void Write(TagRenderMode mode)
            {
                if (_httpResponse != null)
                    _httpResponse.Write(base.ToString(mode));
            }

            public void End()
            {
                Dispose(true);
            }
    }
}
