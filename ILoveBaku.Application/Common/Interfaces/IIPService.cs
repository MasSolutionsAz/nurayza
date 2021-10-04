using System;
using System.Collections.Generic;
using System.Text;

namespace ILoveBaku.Application.Common.Interfaces
{
    public interface IIPService
    {
        T GetRequestIP<T>(bool tryUseXForwardHeader = true);
        T GetHeaderValueAs<T>(string headerName);
    }
}
