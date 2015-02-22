using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace BaseTP1
{
    /// <summary>
    /// Logique d'interaction pour App.xaml
    /// </summary>
    public partial class App : Application
    {
        //Besoin d'un seul objet Random pour l'application, alors il est bien de le rendre static et accessible partout.
        //Seed établis à partir du hash d'un objet GUID qui est une valeur unique où aucune autre invocation sur le même système ou un autre (en réseau ou non) devrait retourner la même chose, du moins, à un très grand degré de certitude.
        //Un seed établis au hasard.

        //https://stackoverflow.com/questions/1785744/how-do-i-seed-a-random-class-to-avoid-getting-duplicate-random-values
        public static Random rand = new Random(Guid.NewGuid().GetHashCode());
    }
}
