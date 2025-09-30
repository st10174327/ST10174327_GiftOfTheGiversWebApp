using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ST10174327_GiftOfTheGiversWebApp.Models
{
    public class Money
    {
        [Key]
        public int MoneyId { get; set; }

        [DataType(DataType.Currency)]
        public decimal TotalMoney { get; set; }

        [DataType(DataType.Currency)]
        public decimal RemainingMoney { get; set; }
    }
}