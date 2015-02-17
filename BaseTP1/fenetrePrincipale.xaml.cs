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
        private ObservableCollection<Joueur> listeParticipantsTournoi;
        private ObservableCollection<Joueur> listeJoueurs;
        private bool isModified = false;

        public fenetrePrincipale()
        {
            InitializeComponent();
            listeJoueurs = new ObservableCollection<Joueur>();
            listeParticipantsTournoi = new ObservableCollection<Joueur>();
            dgTournois.ItemsSource = listeParticipantsTournoi;
            dgJoueur.ItemsSource = listeJoueurs;
            btnFlecheDroite.Content = "\u2192";
            btnFlecheGauche.Content = "\u2190";
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


        private void btnChargement_Click(object sender, RoutedEventArgs e)
        {
            isModified = false;
            listeJoueurs.Clear();
            listeParticipantsTournoi.Clear();
            StringBuilder chaineListe = new StringBuilder();

            // Test de la lecture de fichier, un élément.
            foreach (Joueur j in Joueur.chargerListeJoueurs())
            {
                listeJoueurs.Add(j);
            }
            foreach (Joueur j in listeJoueurs)
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
            Joueur.enregistrerListeJoueurs(listeJoueurs.ToList());
            isModified = false;
        }

        private void Save_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (isModified)
                e.CanExecute = true;
            else
                e.CanExecute = false;
        }

        private void dgJoueur_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            isModified = true;
        }

        private void Appariment_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            StringBuilder message = new StringBuilder();
            Stack<Joueur> stackPairing;
            List<Joueur> listePairing = new List<Joueur>(listeParticipantsTournoi);
            Joueur.randomListeJoueurs(listePairing);
            stackPairing = new Stack<Joueur>(listePairing);
            bool isEven = (listePairing.Count % 2) == 0;
            bool isNewPair = false;
            while (stackPairing.Count != 0)
            {
                message.Append(stackPairing.Pop().Nom);
                if (stackPairing.Count != 0 && !isNewPair)
                    message.Append(" vs ");
                else
                    if (isNewPair)
                        message.Append("\n");
                isNewPair = !isNewPair;
            }
            if (!isEven)
            {
                message.Append(" : Bye\n");
            }
            MessageBox.Show(message.ToString());


        }

        private void Appariment_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (listeParticipantsTournoi.Count >= 2)
            {
                e.CanExecute = true;
            }
            else
                e.CanExecute = false;
        }


    }
}
