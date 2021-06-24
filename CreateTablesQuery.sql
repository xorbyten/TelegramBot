CREATE TABLE Recipes (
RecipeId INT IDENTITY PRIMARY KEY,
RecipeName NVARCHAR(255) NOT NULL,
Description NTEXT NOT NULL
);

CREATE TABLE Ingredients (
IngredientId INT IDENTITY PRIMARY KEY,
FOREIGN KEY (IngredientId) REFERENCES Recipes(RecipeId),
ingr1 NTEXT,
ingr2 NTEXT,
ingr3 NTEXT,
ingr4 NTEXT,
ingr5 NTEXT,
ingr6 NTEXT,
ingr7 NTEXT,
ingr8 NTEXT,
ingr9 NTEXT,
ingr10 NTEXT,
);