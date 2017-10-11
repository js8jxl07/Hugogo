using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hugogo.Model.ExternalModel
{
    /// <summary>
    /// 从身份证获取其他信息Model
    /// </summary>
    public sealed class GetIDCardDetailModel
    {
        public GetIDCardDetailModel()
        {
            isIDCard = false;
            birthDay = ConvertHelper.ToDateTime("1900-01-01");
            customerSex = 0;
            customerType = 0;
        }

        private bool isIDCard;
        /// <summary>
        /// 是否是身份证
        /// </summary>
        public bool IsIDCard
        {
            get { return isIDCard; }
            set { isIDCard = value; }
        }

        private DateTime birthDay;
        /// <summary>
        /// 生日(默认:1900-01-01)
        /// </summary>
        public DateTime BirthDay
        {
            get { return birthDay; }
            set { birthDay = value; }
        }

        private byte customerSex;
        /// <summary>
        /// 性别(0:女-默认  1:男)
        /// </summary>
        public byte CustomerSex
        {
            get { return customerSex; }
            set { customerSex = value; }
        }

        private byte customerType;
        /// <summary>
        /// 游客属性(暂时：0:暂未提供-默认  1:成人  2:儿童)
        /// </summary>
        public byte CustomerType
        {
            get { return customerType; }
            set { customerType = value; }
        }
    }
}
