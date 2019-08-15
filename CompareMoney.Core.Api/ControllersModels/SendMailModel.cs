using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompareMoney.Core.Api.ControllersModels
{
    public class SendMailModel
    {
        /// <summary>
        /// SMTP服务器
        /// </summary>
        public string Smtpserver { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Pwd { get; set; }
        /// <summary>
        /// 收件人
        /// </summary>
        public string ToMail { get; set; }
        /// <summary>
        /// 主题
        /// </summary>
        public string Subj { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Bodys { get; set; }
        /// <summary>
        /// 发件人
        /// </summary>
        public string FromMail  { get; set; }

}
}
