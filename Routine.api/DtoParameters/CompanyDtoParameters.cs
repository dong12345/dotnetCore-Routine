using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Routine.api.DtoParameters
{
    public class CompanyDtoParameters
    {
        /// <summary>
        /// 公司名称
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// 查询参数
        /// </summary>
        public string SearchTerm { get; set; }
    }
}
