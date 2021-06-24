using System;
using System.Collections.Generic;

#nullable disable

namespace TelegramBot.Models
{
    public partial class Recipe
    {
        public int RecipeId { get; set; }
        public string RecipeName { get; set; }
        public string Description { get; set; }

        public virtual Ingredient Ingredient { get; set; }
    }
}
