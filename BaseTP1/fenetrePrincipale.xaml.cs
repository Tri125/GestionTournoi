using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BaseTP1
{
    /// <summary>
    /// Logique d'interaction pour fenetrePrincipale.xaml
    /// </summary>
    public partial class fenetrePrincipale : Window
    {
        private List<Joueur> listeJoueurs;
        public fenetrePrincipale()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Cette méthode a été créée simplement pour montrer un exemple d'utilisation du chargement des joueurs.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder chaineListe = new StringBuilder();

            // Test de la lecture de fichier, un élément.
            List<Joueur> listeJoueurs = Joueur.chargerListeJoueurs();

            foreach (Joueur j in listeJoueurs)
            {
                chaineListe.Append("#").Append(j.NoDCI).Append(" : ").Append(j.Prenom).Append(" ").Append(j.Nom).AppendLine(".");
            }
            dgJoueur.ItemsSource = listeJoueurs;
            MessageBox.Show(chaineListe.ToString());

            /*
            // À titre d'exemple d'écriture. L'exécution de ce code vide votre liste alors, attention!
            List<Joueur> lst = new List<Joueur>();
            lst.Add(new Joueur("12345678", "Joe", "Bleau"));

            Joueur.enregistrerListeJoueurs(lst);
            */
        }        
    }
}
