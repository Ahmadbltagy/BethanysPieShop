using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BethanysPieShop.Models;

namespace BethanysPieShop.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Pie> PieOfTheWeek { get; }
        public HomeViewModel(IEnumerable<Pie> pieOfTheWeek)
        {
            this.PieOfTheWeek = pieOfTheWeek;
        }
    }
}