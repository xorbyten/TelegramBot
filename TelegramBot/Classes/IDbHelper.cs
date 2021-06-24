using System;
using System.Collections.Generic;

interface IDbHelper 
{
    List<List<string>> Search(string keyword);
}