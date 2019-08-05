using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CompareMoney.Core.Domain.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }  //id

        public string UserName { get; set; } //用户名

        public string Password { get; set; } //密码

        public string AdminPassword { get; set; }//类似支付密码

        public string HospitalName { get; set; } //医院名称

        public string Role { get; set; } //权限


    }
}
