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
            teste = new ObservableCollection<Joueur>();
            listeParticipantsTournoi = new ObservableCollection<Joueur>();
            dgTournois.ItemsSource = listeParticipantsTournoi;
            dgJoueur.ItemsSource = teste;
            btnFlecheDroite.Content = "\u2192";
            btnFlecheGauche.Content = "\u2190";
        }

        /// <summary>
        /// Cette méthode a été créée simplement pour montrer un exemple d'utilisation du chargement des joueurs.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            teste.Insert(0, new Joueur("-1", "Prénom", "Nom"));
            dgJoueur.SelectedIndex = 0;
            dgJoueur.ScrollIntoView(dgJoueur.SelectedItem);
        }

        private void btnFlecheGauche_Click(object sender, RoutedEventArgs e)
        {
            if (dgTournois.SelectedIndex >= 0)
            {
                listeParticipantsTournoi.RemoveAt(dgTournois.SelectedIndex);
            }

        }

        private void btnFlecheDroite_Click(object sender, RoutedEventArgs e)
        {
            Joueur selection = dgJoueur.SelectedItem as Joueur;
            if (!listeParticipantsTournoi.Contains(selection))
            {
                listeParticipantsTournoi.Insert(0, selection);
            }
        }

        private void btnAppariment_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnChargement_Click(object sender, RoutedEventArgs e)
        {
            teste.Clear();
            listeParticipantsTournoi.Clear();
            StringBuilder chaineListe = new StringBuilder();

            // Test de la lecture de fichier, un élément.
            foreach (Joueur j in Joueur.chargerListeJoueurs())
            {
                teste.Add(j);
            }
            foreach (Joueur j in teste)
            {
                chaineListe.Append("#").Append(j.NoDCI).Append(" : ").Append(j.Prenom).Append(" ").Append(j.Nom).AppendLine(".");
            }
            //MessageBox.Show(chaineListe.ToString());

            /*
            // À titre d'exemple d'écriture. L'exécution de ce code vide votre liste alors, attention!
            List<Joueur> lst = new List<Joueur>();
            lst.Add(new Joueur("12345678", "Joe", "Bleau"));

            Joueur.enregistrerListeJoueurs(lst);
            */
        }

        private void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Joueur.enregistrerListeJoueurs(teste.ToList());
        }

        private void Save_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

    }
}
