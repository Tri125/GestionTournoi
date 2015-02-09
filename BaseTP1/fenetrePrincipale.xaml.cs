using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private ObservableCollection<Joueur> listeParticipantsTournoi;
        private ObservableCollection<Joueur> teste;

        public fenetrePrincipale()
        {
            InitializeComponent();
            listeParticipantsTournoi = new ObservableCollection<Joueur>();
            dgTournois.ItemsSource = listeParticipantsTournoi;
            btnFlecheDroite.Content = "\u2192";
            btnFlecheGauche.Content = "\u2190";
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
            listeJoueurs = Joueur.chargerListeJoueurs();
            teste = new ObservableCollection<Joueur>(listeJoueurs);
            foreach (Joueur j in listeJoueurs)
            {
                chaineListe.Append("#").Append(j.NoDCI).Append(" : ").Append(j.Prenom).Append(" ").Append(j.Nom).AppendLine(".");
            }
            dgJoueur.ItemsSource = teste;
            //MessageBox.Show(chaineListe.ToString());

            /*
            // À titre d'exemple d'écriture. L'exécution de ce code vide votre liste alors, attention!
            List<Joueur> lst = new List<Joueur>();
            lst.Add(new Joueur("12345678", "Joe", "Bleau"));

            Joueur.enregistrerListeJoueurs(lst);
            */
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            teste.Insert(0,new Joueur("-1", "Prénom", "Nom"));
            dgJoueur.SelectedIndex = 0;
            dgJoueur.ScrollIntoView(dgJoueur.SelectedItem);
        }

        private void btnFlecheGauche_Click(object sender, RoutedEventArgs e)
        {
            Joueur selection = dgJoueur.SelectedItem as Joueur;
            listeParticipantsTournoi.Insert(0, selection);
        }

        private void btnFlecheDroite_Click(object sender, RoutedEventArgs e)
        {
            listeParticipantsTournoi.RemoveAt(dgTournois.SelectedIndex);
        }
     
    }
}
