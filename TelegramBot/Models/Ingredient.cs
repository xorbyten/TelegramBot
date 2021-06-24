using System;
using System.Collections.Generic;

#nullable disable

namespace TelegramBot.Models
{
    public partial class Ingredient
    {
        public int IngredientId { get; set; }
        public string Ingr1 { get; set; }
        public string Ingr2 { get; set; }
        public string Ingr3 { get; set; }
        public string Ingr4 { get; set; }
        public string Ingr5 { get; set; }
        public string Ingr6 { get; set; }
        public string Ingr7 { get; set; }
        public string Ingr8 { get; set; }
        public string Ingr9 { get; set; }
        public string Ingr10 { get; set; }

        public virtual Recipe IngredientNavigation { get; set; }
    }
}
